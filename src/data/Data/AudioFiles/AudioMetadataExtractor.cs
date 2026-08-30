namespace OwlShed.Hushit.Data.AudioFiles;

internal static class AudioMetadataExtractor
{
	#region Functions
	public static async ValueTask<string?> ExtractCoverAsync(string path, CancellationToken cancellation = default)
	{
		JsonElement? metadata = await GetMetadataAsync(path, cancellation);
		if (metadata is null)
			return null;

		if (TryGetCover(metadata.Value, out int stream, out string? format) is false)
			return null;

		string extension = format switch
		{
			"png" => ".png",
			"mjpeg" => ".jpg",
			_ => ".png",
		};

		DirectoryInfo directory = Directory.CreateTempSubdirectory("owlshed.hushit.covers.");
		string outputPath = Path.Combine(directory.FullName, "cover" + extension);

		_ = await GetOutputAsync("ffmpeg",
		[
			"-i", path,
			"-map", $"0:{stream}",
			"-frames:v", "1",
			"-c", "copy",
			outputPath
		]);

		if (File.Exists(outputPath))
			return outputPath;

		return null;
	}
	private static bool TryGetCover(JsonElement root, out int stream, out string? format)
	{
		stream = -1;
		int lastVideo = -1;

		foreach (JsonElement streamElement in root.GetProperty("streams").EnumerateArray())
		{
			stream++;
			if (streamElement.TryGetProperty("codec_type", out JsonElement codec) && codec.GetString() == "video")
			{
				lastVideo = stream;

				format = streamElement.GetProperty("codec_name").GetString();
				if (format is "mpjeg" or "png")
					return true;
			}
		}

		format = default;
		stream = lastVideo;
		return lastVideo >= 0;
	}

	public static async ValueTask ExtractAsync(string path, MutableAudioFile file, CancellationToken cancellation = default)
	{
		FingerprintInfo? fingerprint = await GetChromaprintAsync(path, cancellation).ConfigureAwait(false);

		if (fingerprint is not null)
			file.Fingerprint = fingerprint;

		JsonElement? json = await GetMetadataAsync(path, cancellation).ConfigureAwait(false);
		if (json is null)
			return;

		JsonElement format = FindFormat(json.Value);
		JsonElement audio = FindAudioStream(json.Value);
		JsonElement tags = FindTags(json.Value);

		ExtractMetadata(format, audio, tags, file);
	}
	private static void ExtractMetadata(JsonElement format, JsonElement audio, JsonElement tags, MutableAudioFile file)
	{
		bool TryGetFormatProperty(out JsonElement element, params ReadOnlySpan<string> names)
		{
			return AudioMetadataExtractor.TryGetFormatProperty(audio, format, out element, names);
		}

		if (TryGetFormatProperty(out JsonElement duration, "duration") && TryGetDouble(duration, out double seconds))
			file.Duration = TimeSpan.FromSeconds(seconds);

		if (TryGetFormatProperty(out JsonElement container, "format_name", "format_long_name"))
			file.ContainerFormat = container.GetString();

		if (TryGetFormatProperty(out JsonElement codec, "codec_name", "codec_long_name"))
			file.AudioFormat = codec.GetString();

		if (TryGetFormatProperty(out JsonElement bitrateElement, "bit_rate") && TryGetInt32(bitrateElement, out int bitrate))
			file.BitRate = bitrate;

		if (TryGetFormatProperty(out JsonElement sampleRateElement, "sample_rate") && TryGetInt32(sampleRateElement, out int sampleRate))
			file.SampleRate = sampleRate;

		if (TryGetFormatProperty(out JsonElement channelElement, "channels") && TryGetInt32(channelElement, out int channels))
			file.Channels = channels;

		foreach (JsonProperty tag in tags.EnumerateObject())
		{
			string value = (tag.Value.ValueKind is JsonValueKind.String ? tag.Value.GetString() : null) ?? tag.Value.ToString();

			if (IsTagName(tag, "title"))
				file.WithTrackName(value);
			else if (IsTagName(tag, "album"))
				file.WithAlbumName(value);
			else if (IsTagName(tag, "artist", "artists", "track_artist", "track_artists"))
				TryAddUnique(file.TrackArtists, value);
			else if (IsTagName(tag, "album_artist", "album_artists"))
				TryAddUnique(file.AlbumArtists, value);
			else if (IsTagName(tag, "genre", "genres", "track_genre", "track_genres"))
				TryAddUnique(file.TrackGenres, value);
			else if (IsTagName(tag, "album_genre", "album_genres"))
				TryAddUnique(file.AlbumGenres, value);
			else if (IsTagName(tag, "date"))
			{
				if (DateOnly.TryParse(value, out DateOnly date))
					file.WithTrackDate(new(date.Year, date.Month, date.Day));
				else if (TryGetInt32(tag.Value, out int year))
					file.WithTrackDate(new(year, null, null));
			}
			else if (IsTagName(tag, "track"))
			{
				if (TryGetInt32(tag.Value, out int track))
					file.TrackNumber = track;
			}
			else if (IsTagName(tag, "track_total", "total_track", "total_tracks"))
			{
				if (TryGetInt32(tag.Value, out int totalTracks))
					file.TotalTracks = totalTracks;
			}
		}
	}
	private static async ValueTask<FingerprintInfo?> GetChromaprintAsync(string path, CancellationToken cancellation = default)
	{
		string? output = await GetOutputAsync("fpcalc", ["-plain", path], cancellation).ConfigureAwait(false);
		if (output is null)
			return null;

		return new("chromaprint", output);
	}
	#endregion

	#region Helpers
	private static async ValueTask<JsonElement?> GetMetadataAsync(string path, CancellationToken cancellation = default)
	{
		string? output = await GetOutputAsync("ffprobe",
				[
					"-v", "quiet", "-hide_banner",
			"-of", "json",
			"-show_format", "-show_streams",
			"-i", path
				], cancellation).ConfigureAwait(false);

		if (output is null)
			return null;

		JsonElement json = JsonDocument.Parse(output).RootElement;
		return json;
	}
	private static async ValueTask<string?> GetOutputAsync(string path, IReadOnlyList<string> arguments, CancellationToken cancellation = default)
	{
		ProcessStartInfo startInfo = new(path, arguments)
		{
			RedirectStandardOutput = true,
			RedirectStandardError = true,
		};

		Process? process = Process.Start(startInfo);
		if (process is null)
			return null;

		await process.WaitForExitAsync(cancellation).ConfigureAwait(false);
		string output = await process.StandardOutput.ReadToEndAsync(cancellation).ConfigureAwait(false);
		output = output.Trim();

		if (output.IsWhiteSpace())
			return null;

		return output;
	}
	private static void TryAddUnique(IList<string> list, string? value)
	{
		if (value is null)
			return;

		if (list.Contains(value) is false)
			list.Add(value);
	}
	private static bool IsTagName(JsonProperty property, params ReadOnlySpan<string> names)
	{
		foreach (string name in names)
		{
			if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase))
				return true;

			if (string.Equals(property.Name.Replace("_", ""), name, StringComparison.OrdinalIgnoreCase))
				return true;
		}

		return false;
	}
	private static bool TryGetDouble(JsonElement element, out double value)
	{
		if (element.ValueKind is JsonValueKind.Number && element.TryGetDouble(out value))
			return true;

		if (element.ValueKind is JsonValueKind.String)
		{
			string? text = element.GetString();
			if (double.TryParse(text, out value))
				return true;
		}

		value = default;
		return false;
	}
	private static bool TryGetInt32(JsonElement element, out int value)
	{
		if (element.ValueKind is JsonValueKind.Number && element.TryGetInt32(out value))
			return true;

		if (element.ValueKind is JsonValueKind.String)
		{
			string? text = element.GetString();
			if (int.TryParse(text, out value))
				return true;
		}

		value = default;
		return false;
	}
	private static bool TryGetFormatProperty(JsonElement audio, JsonElement format, out JsonElement element, params ReadOnlySpan<string> names)
	{
		foreach (string name in names)
		{
			if (audio.TryGetProperty(name, out element))
				return true;

			if (audio.TryGetProperty(name.Replace("_", ""), out element))
				return true;
		}

		foreach (string name in names)
		{
			if (format.TryGetProperty(name, out element))
				return true;

			if (format.TryGetProperty(name.Replace("_", ""), out element))
				return true;
		}

		element = default;
		return false;
	}
	private static JsonElement FindFormat(JsonElement root) => root.GetProperty("format");
	private static JsonElement FindAudioStream(JsonElement root)
	{
		foreach (JsonElement stream in root.GetProperty("streams").EnumerateArray())
		{
			if (stream.TryGetProperty("codec_type", out JsonElement codec))
			{
				if (codec.GetString() == "audio")
					return stream;
			}
		}

		ThrowHelper.ThrowInvalidOperationException("No audio stream was present.");
		return default;
	}
	private static JsonElement FindTags(JsonElement root)
	{
		if (root.TryGetProperty("format", out JsonElement format))
		{
			if (format.TryGetProperty("tags", out JsonElement tags))
				return tags;
		}

		JsonElement audio = FindAudioStream(root);
		return audio.GetProperty("tags");
	}
	#endregion
}
