namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents an update for the <see cref="IGenreInfo"/>.
/// </summary>
public sealed class GenreUpdate : DataModelUpdateBase
{
	#region Properties
	/// <inheritdoc cref="IGenreInfo.Name"/>
	public ValueUpdate<string> Name { get; set; }
	#endregion
}
