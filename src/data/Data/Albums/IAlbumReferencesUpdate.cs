namespace OwlShed.Hushit.Data.Albums;

/// <summary>
/// 	Represents an update to the <see cref="IAlbumReferencesInfo"/>.
/// </summary>
public interface IAlbumReferencesUpdate
{
	#region Methods
	/// <summary>Adds a new related album.</summary>
	/// <param name="id">The id of the album to add.</param>
	/// <returns>The used update builder.</returns>
	IAlbumReferencesUpdate AddAlbum(string id);

	/// <summary>Removes a related album.</summary>
	/// <param name="id">The id of the album to remove.</param>
	/// <returns>The used update builder.</returns>
	IAlbumReferencesUpdate RemoveAlbum(string id);

	/// <summary>Sets the new album ids.</summary>
	/// <param name="albumIds">The ids of the new related albums.</param>
	/// <returns>The used update builder.</returns>
	IAlbumReferencesUpdate WithAlbums(params IReadOnlyList<string> albumIds);
	#endregion
}

/// <summary>
/// 	Represents an update to the <see cref="IAlbumReferencesInfo"/>.
/// </summary>
/// <typeparam name="TBuilder">The type of the actual builder used.</typeparam>
public interface IAlbumReferencesUpdate<TBuilder> : IAlbumReferencesUpdate
	where TBuilder : notnull, IAlbumReferencesUpdate<TBuilder>
{
	#region Methods
	/// <inheritdoc cref="IAlbumReferencesUpdate.AddAlbum(string)"/>
	new TBuilder AddAlbum(string id);

	/// <inheritdoc cref="IAlbumReferencesUpdate.RemoveAlbum(string)"/>
	new TBuilder RemoveAlbum(string id);

	/// <inheritdoc cref="IAlbumReferencesUpdate.WithAlbums(IReadOnlyList{string})"/>
	new TBuilder WithAlbums(params IReadOnlyList<string> albumIds);

	IAlbumReferencesUpdate IAlbumReferencesUpdate.AddAlbum(string id) => AddAlbum(id);
	IAlbumReferencesUpdate IAlbumReferencesUpdate.RemoveAlbum(string id) => RemoveAlbum(id);
	IAlbumReferencesUpdate IAlbumReferencesUpdate.WithAlbums(params IReadOnlyList<string> albumIds) => WithAlbums(albumIds);
	#endregion
}
