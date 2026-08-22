namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents an update to the <see cref="ITrackReferencesInfo"/>.
/// </summary>
public interface ITrackReferencesUpdate
{
	#region Methods
	/// <summary>Adds a new related track.</summary>
	/// <param name="id">The id of the track to add.</param>
	/// <returns>The used update builder.</returns>
	ITrackReferencesUpdate AddTrack(string id);

	/// <summary>Removes a related track.</summary>
	/// <param name="id">The id of the track to remove.</param>
	/// <returns>The used update builder.</returns>
	ITrackReferencesUpdate RemoveTrack(string id);

	/// <summary>Sets the new track ids.</summary>
	/// <param name="trackIds">The ids of the new related tracks.</param>
	/// <returns>The used update builder.</returns>
	ITrackReferencesUpdate WithTracks(params IReadOnlyList<string> trackIds);
	#endregion
}

/// <summary>
/// 	Represents an update to the <see cref="ITrackReferencesInfo"/>.
/// </summary>
/// <typeparam name="TBuilder">The type of the actual builder used.</typeparam>
public interface ITrackReferencesUpdate<TBuilder> : ITrackReferencesUpdate
	where TBuilder : notnull, ITrackReferencesUpdate<TBuilder>
{
	#region Methods
	/// <inheritdoc cref="ITrackReferencesUpdate.AddTrack(string)"/>
	new TBuilder AddTrack(string id);

	/// <inheritdoc cref="ITrackReferencesUpdate.RemoveTrack(string)"/>
	new TBuilder RemoveTrack(string id);

	/// <inheritdoc cref="ITrackReferencesUpdate.WithTracks(IReadOnlyList{string})"/>
	new TBuilder WithTracks(params IReadOnlyList<string> trackIds);

	ITrackReferencesUpdate ITrackReferencesUpdate.AddTrack(string id) => AddTrack(id);
	ITrackReferencesUpdate ITrackReferencesUpdate.RemoveTrack(string id) => RemoveTrack(id);
	ITrackReferencesUpdate ITrackReferencesUpdate.WithTracks(params IReadOnlyList<string> trackIds) => WithTracks(trackIds);
	#endregion
}
