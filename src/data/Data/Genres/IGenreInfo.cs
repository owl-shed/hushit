namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents information about a genre.
/// </summary>
public interface IGenreInfo : IDataModel<MutableGenre, GenreUpdate>, IArtistReferencesInfo, IAlbumReferencesInfo, ITrackReferencesInfo
{
	#region Properties
	/// <summary>The name of the genre.</summary>
	string Name { get; }
	#endregion

	#region Mutable methods
	/// <inheritdoc cref="IDataModel{T}.ToMutable"/>
	new MutableGenre ToMutable();
	MutableGenre IDataModel<MutableGenre>.ToMutable() => ToMutable();
	IMutableArtistReferences IDataModel<IMutableArtistReferences>.ToMutable() => ToMutable();
	IMutableAlbumReferences IDataModel<IMutableAlbumReferences>.ToMutable() => ToMutable();
	IMutableTrackReferences IDataModel<IMutableTrackReferences>.ToMutable() => ToMutable();
	#endregion
}
