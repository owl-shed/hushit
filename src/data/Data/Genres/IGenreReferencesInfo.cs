namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents a data model that contains references to genres.
/// </summary>
public interface IGenreReferencesInfo : IDataModel
{
	#region Properties
	/// <summary>The ids of the related genres.</summary>
	ReadOnlyObservableCollection<string> GenreIds { get; }
	#endregion
}
