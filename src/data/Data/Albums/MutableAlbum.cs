namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents a mutable version of the <see cref="IAlbumInfo"/>.
/// </summary>
public sealed class MutableAlbum : MutableDataModelBase<MutableAlbum, AlbumUpdate>
{
	#region Properties
	/// <inheritdoc cref="IAlbumInfo.Name"/>
	public string? Name { get; set; }

	/// <inheritdoc cref="IArtistReferencesInfo.ArtistIds"/>
	public IList<string> ArtistIds { get; set; } = [];
	#endregion

	#region Update methods
	/// <inheritdoc/>
	public override AlbumUpdate GetUpdateFrom(MutableAlbum oldState)
	{
		if (Name is null)
			ThrowHelper.ThrowInvalidOperationException($"Expected the new '{nameof(Name)}' to have a value.");

		return new()
		{
			Name = Update.Value(oldState.Name, Name),
			ArtistIds = Update.List(oldState.ArtistIds, ArtistIds),
		};
	}
	#endregion
}
