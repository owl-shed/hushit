namespace OwlShed.Hushit.Data.Genres.References;

/// <summary>
/// 	Represents an update to the <see cref="IGenreReferencesInfo"/>.
/// </summary>
public interface IGenreReferencesUpdate
{
	#region Properties
	/// <inheritdoc cref="IGenreReferencesInfo.GenreIds"/>
	ListUpdate<string> GenreIds { get; set; }
	#endregion
}
