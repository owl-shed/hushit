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
		if (format.TryGetProperty("duration", out JsonElement durationElement) && double.TryParse(durationElement.GetString(), null, out double seconds))
			file.Duration = TimeSpan.FromSeconds(seconds);

		if (format.TryGetProperty("format_name", out JsonElement container))
			file.ContainerFormat = container.GetString();

		if (audio.TryGetProperty("codec_name", out JsonElement codec))
			file.AudioFormat = codec.GetString();

		foreach (JsonProperty tag in tags.EnumerateObject())
		{
			string tagName = tag.Name.ToLower();
			string? value = tag.Value.GetString() ?? tag.Value.ToString();

			if (value.IsWhiteSpace())
				continue;

			if (tagName is "title")
			{
				file.WithTrackName(value);
				continue;
			}

			if (tagName is "album")
			{
				file.WithAlbumName(value);
				continue;
			}

			if (tagName is "artist" or "artists")
			{
				if (file.TrackArtists.Contains(value) is false)
					file.TrackArtists.Add(value);

				continue;
			}

			if (tagName is "albumartist" or "album_artist" or "albumartists" or "album_artists")
			{
				if (file.AlbumArtists.Contains(value) is false)
					file.AlbumArtists.Add(value);

				continue;
			}

			if (tagName is "date")
			{
				if (DateOnly.TryParse(value, out DateOnly date))
					file.WithTrackDate(new(date.Year, date.Month, date.Day));
				else if (int.TryParse(value, out int year))
					file.WithTrackDate(new(year, null, null));

				continue;
			}

			if (tagName is "track")
			{
				if (int.TryParse(value, out int track))
					file.TrackNumber = track;

				continue;
			}

			if (tagName is "tracktotal" or "track_total" or "total_tracks" or "totaltracks")
			{
				if (int.TryParse(value, out int totalTracks))
					file.TotalTracks = totalTracks;

				continue;
			}
		}
	}
	#endregion

	#region Helpers
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
