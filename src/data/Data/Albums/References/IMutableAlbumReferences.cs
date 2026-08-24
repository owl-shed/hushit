namespace OwlShed.Hushit.Data.Albums.References;

/// <summary>
/// 	Represents a mutable version of the <see cref="IAlbumReferencesInfo"/>.
/// </summary>
public interface IMutableAlbumReferences
{
	#region Properties
	/// <inheritdoc cref="IAlbumReferencesInfo.AlbumIds"/>
	IList<string> AlbumIds { get; set; }
	#endregion
}

/// <summary>
/// 	Represents a mutable version of the <see cref="IAlbumReferencesInfo"/>.
/// </summary>
/// <typeparam name="TSelf">The type that implements the mutable interface.</typeparam>
public interface IMutableAlbumReferences<TSelf> : IMutableAlbumReferences
	where TSelf : notnull, IMutableAlbumReferences<TSelf>
{
}

/// <summary>
/// 	Contains various extensions related to the <see cref="IMutableAlbumReferences"/>.
/// </summary>
public static class IMutableAlbumReferencesExtensions
{
	extension(IMutableAlbumReferences mutable)
	{
		#region Methods
		/// <summary>Adds a new related album.</summary>
		/// <param name="id">The id of the album to add.</param>
		/// <returns>The used mutable album references instance.</returns>
		public IMutableAlbumReferences AddAlbum(string id)
		{
			mutable.AlbumIds.Add(id);
			return mutable;
		}

		/// <summary>Removes a related album.</summary>
		/// <param name="id">The id of the album to remove.</param>
		/// <returns>The used mutable album references instance.</returns>
		public IMutableAlbumReferences RemoveAlbum(string id)
		{
			mutable.AlbumIds.Remove(id);
			return mutable;
		}

		/// <summary>Sets the new album ids.</summary>
		/// <param name="albumIds">The ids of the new related albums.</param>
		/// <returns>The used mutable album references instance.</returns>
		public IMutableAlbumReferences WithAlbums(params IReadOnlyList<string> albumIds)
		{
			mutable.AlbumIds = [.. albumIds];
			return mutable;
		}
		#endregion
	}

	extension<TMutable>(IMutableAlbumReferences<TMutable> mutable) where TMutable : notnull, IMutableAlbumReferences<TMutable>
	{
		#region Methods
		/// <summary>Adds a new related album.</summary>
		/// <param name="album">The album to add.</param>
		/// <returns>The used mutable album references instance.</returns>
		public TMutable AddAlbum(IAlbumInfo album)
		{
			mutable.AlbumIds.Add(album.Id);
			return (TMutable)mutable;
		}

		/// <summary>Removes a related album.</summary>
		/// <param name="album">The album to remove.</param>
		/// <returns>The used mutable album references instance.</returns>
		public TMutable RemoveAlbum(IAlbumInfo album)
		{
			mutable.AlbumIds.Remove(album.Id);
			return (TMutable)mutable;
		}

		/// <summary>Sets the new album ids.</summary>
		/// <param name="albums">The new related albums.</param>
		/// <returns>The used mutable album references instance.</returns>
		public TMutable WithAlbums(params IReadOnlyList<IAlbumInfo> albums)
		{
			mutable.AlbumIds = [.. albums.Select(static album => album.Id)];
			return (TMutable)mutable;
		}
		#endregion
	}
}
