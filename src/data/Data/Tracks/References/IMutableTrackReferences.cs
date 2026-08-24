namespace OwlShed.Hushit.Data.Tracks.References;

/// <summary>
/// 	Represents a mutable version of the <see cref="ITrackReferencesInfo"/>.
/// </summary>
public interface IMutableTrackReferences
{
	#region Properties
	/// <inheritdoc cref="ITrackReferencesInfo.TrackIds"/>
	IList<string> TrackIds { get; set; }
	#endregion
}

/// <summary>
/// 	Represents a mutable version of the <see cref="ITrackReferencesInfo"/>.
/// </summary>
/// <typeparam name="TSelf">The type that implements the mutable interface.</typeparam>
public interface IMutableTrackReferences<TSelf> : IMutableTrackReferences
	where TSelf : notnull, IMutableTrackReferences<TSelf>
{
}

/// <summary>
/// 	Contains various extensions related to the <see cref="IMutableTrackReferences"/>.
/// </summary>
public static class IMutableTrackReferencesExtensions
{
	extension(IMutableTrackReferences mutable)
	{
		#region Methods
		/// <summary>Adds a new related track.</summary>
		/// <param name="id">The id of the track to add.</param>
		/// <returns>The used mutable track references instance.</returns>
		public IMutableTrackReferences AddTrack(string id)
		{
			mutable.TrackIds.Add(id);
			return mutable;
		}

		/// <summary>Removes a related track.</summary>
		/// <param name="id">The id of the track to remove.</param>
		/// <returns>The used mutable track references instance.</returns>
		public IMutableTrackReferences RemoveTrack(string id)
		{
			mutable.TrackIds.Remove(id);
			return mutable;
		}

		/// <summary>Sets the new track ids.</summary>
		/// <param name="trackIds">The ids of the new related tracks.</param>
		/// <returns>The used mutable track references instance.</returns>
		public IMutableTrackReferences WithTracks(params IReadOnlyList<string> trackIds)
		{
			mutable.TrackIds = [.. trackIds];
			return mutable;
		}
		#endregion
	}

	extension<TMutable>(IMutableTrackReferences<TMutable> mutable) where TMutable : notnull, IMutableTrackReferences<TMutable>
	{
		#region Methods
		/// <summary>Adds a new related track.</summary>
		/// <param name="track">The track to add.</param>
		/// <returns>The used mutable track references instance.</returns>
		public TMutable AddTrack(ITrackInfo track)
		{
			mutable.TrackIds.Add(track.Id);
			return (TMutable)mutable;
		}

		/// <summary>Removes a related track.</summary>
		/// <param name="track">The track to remove.</param>
		/// <returns>The used mutable track references instance.</returns>
		public TMutable RemoveTrack(ITrackInfo track)
		{
			mutable.TrackIds.Remove(track.Id);
			return (TMutable)mutable;
		}

		/// <summary>Sets the new track ids.</summary>
		/// <param name="tracks">The new related tracks.</param>
		/// <returns>The used mutable track references instance.</returns>
		public TMutable WithTracks(params IReadOnlyList<ITrackInfo> tracks)
		{
			mutable.TrackIds = [.. tracks.Select(static track => track.Id)];
			return (TMutable)mutable;
		}
		#endregion
	}
}
