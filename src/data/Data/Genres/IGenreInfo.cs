namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents information about a genre.
/// </summary>
public interface IGenreInfo : IDataModel<IGenreUpdate>
{
	#region Properties
	/// <summary>The name of the genre.</summary>
	string Name { get; }
	#endregion
}
