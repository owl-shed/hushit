namespace OwlShed.Hushit.Data.AudioFiles;

internal static class AudioMetadataExtractor
{
	#region Functions
	public static async ValueTask ExtractAsync(string path, MutableAudioFile file, CancellationToken cancellation = default)
	{
		ProcessStartInfo startInfo = new("ffprobe",
		[
			"-v", "quiet", "-hide_banner",
			"-of", "json",
			"-show_format", "-show_streams",
			"-i", path
		])
		{
			RedirectStandardOutput = true
		};

		Process? process = Process.Start(startInfo);
		if (process is null)
			return;

		await process.WaitForExitAsync(cancellation).ConfigureAwait(false);
		string output = await process.StandardOutput.ReadToEndAsync(cancellation).ConfigureAwait(false);

		JsonElement json = JsonDocument.Parse(output).RootElement;

		JsonElement format = FindFormat(json);
		JsonElement audio = FindAudioStream(json);
		JsonElement tags = FindTags(json);

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
				file.WithTrack(value);
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
	#endregion

	#region Helpers
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
