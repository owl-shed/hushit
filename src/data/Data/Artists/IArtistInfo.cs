namespace OwlShed.Hushit.Data.Artists;

/// <summary>
/// 	Represents information about an artist.
/// </summary>
public interface IArtistInfo : IDataModel<MutableArtist, ArtistUpdate>, IAlbumReferencesInfo, ITrackReferencesInfo, IGenreReferencesInfo
{
	#region Properties
	/// <summary>The primary name of the artist.</summary>
	string Name { get; }

	/// <summary>Any aliases that the artist may have.</summary>
	ReadOnlyObservableCollection<string> Aliases { get; }
	#endregion

	#region Mutable methods
	/// <inheritdoc cref="IDataModel{T}.ToMutable"/>
	new MutableArtist ToMutable();
	MutableArtist IDataModel<MutableArtist>.ToMutable() => ToMutable();
	IMutableAlbumReferences IDataModel<IMutableAlbumReferences>.ToMutable() => ToMutable();
	IMutableGenreReferences IDataModel<IMutableGenreReferences>.ToMutable() => ToMutable();
	IMutableTrackReferences IDataModel<IMutableTrackReferences>.ToMutable() => ToMutable();
	#endregion
}
