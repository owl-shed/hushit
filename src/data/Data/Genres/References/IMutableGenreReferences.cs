namespace OwlShed.Hushit.Data.Genres.References;

/// <summary>
/// 	Represents a mutable version of the <see cref="IGenreReferencesInfo"/>.
/// </summary>
public interface IMutableGenreReferences
{
	#region Properties
	/// <inheritdoc cref="IGenreReferencesInfo.GenreIds"/>
	IList<string> GenreIds { get; set; }
	#endregion
}
