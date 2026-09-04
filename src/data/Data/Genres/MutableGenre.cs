namespace OwlShed.Hushit.Data.Genres;

/// <summary>
///   Represents a mutable version of the <see cref="IGenreInfo"/>.
/// </summary>
public sealed class MutableGenre : MutableDataModelBase<MutableGenre, GenreUpdate>
{
	#region Properties
	/// <inheritdoc cref="IGenreInfo.Name"/>
	public string? Name { get; set; }
	#endregion

	#region Update methods
	/// <inheritdoc/>
	public override GenreUpdate GetUpdateFrom(MutableGenre oldState)
	{
		if (Name is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Name)}' to have a value.");

		return new()
		{
			Name = Update.Value(oldState.Name, Name),
		};
	}
	#endregion
}
