namespace OwlShed.Hushit.Data.Artists.References;

/// <summary>
/// 	Represents a mutable version of the <see cref="IArtistReferencesInfo"/>.
/// </summary>
public interface IMutableArtistReferences
{
	#region Properties
	/// <inheritdoc cref="IArtistReferencesInfo.ArtistIds"/>
	IList<string> ArtistIds { get; set; }
	#endregion
}

/// <summary>
/// 	Represents a mutable version of the <see cref="IArtistReferencesInfo"/>.
/// </summary>
/// <typeparam name="TSelf">The type that implements the mutable interface.</typeparam>
public interface IMutableArtistReferences<TSelf> : IMutableArtistReferences
	where TSelf : notnull, IMutableArtistReferences<TSelf>
{
}

/// <summary>
/// 	Contains various extensions related to the <see cref="IMutableArtistReferences"/>.
/// </summary>
public static class IMutableArtistReferencesExtensions
{
	extension(IMutableArtistReferences mutable)
	{
		#region Methods
		/// <summary>Adds a new related artist.</summary>
		/// <param name="id">The id of the artist to add.</param>
		/// <returns>The used mutable artist references instance.</returns>
		public IMutableArtistReferences AddArtist(string id)
		{
			mutable.ArtistIds.Add(id);
			return mutable;
		}

		/// <summary>Removes a related artist.</summary>
		/// <param name="id">The id of the artist to remove.</param>
		/// <returns>The used mutable artist references instance.</returns>
		public IMutableArtistReferences RemoveArtist(string id)
		{
			mutable.ArtistIds.Remove(id);
			return mutable;
		}

		/// <summary>Sets the new artist ids.</summary>
		/// <param name="artistIds">The ids of the new related artists.</param>
		/// <returns>The used mutable artist references instance.</returns>
		public IMutableArtistReferences WithArtists(params IReadOnlyList<string> artistIds)
		{
			mutable.ArtistIds = [.. artistIds];
			return mutable;
		}
		#endregion
	}

	extension<TMutable>(IMutableArtistReferences<TMutable> mutable) where TMutable : notnull, IMutableArtistReferences<TMutable>
	{
		#region Methods
		/// <summary>Adds a new related artist.</summary>
		/// <param name="artist">The artist to add.</param>
		/// <returns>The used mutable artist references instance.</returns>
		public TMutable AddArtist(IArtistInfo artist)
		{
			mutable.ArtistIds.Add(artist.Id);
			return (TMutable)mutable;
		}

		/// <summary>Removes a related artist.</summary>
		/// <param name="artist">The artist to remove.</param>
		/// <returns>The used mutable artist references instance.</returns>
		public TMutable RemoveArtist(IArtistInfo artist)
		{
			mutable.ArtistIds.Remove(artist.Id);
			return (TMutable)mutable;
		}

		/// <summary>Sets the new artist ids.</summary>
		/// <param name="artists">The new related artists.</param>
		/// <returns>The used mutable artist references instance.</returns>
		public TMutable WithArtists(params IReadOnlyList<IArtistInfo> artists)
		{
			mutable.ArtistIds = [.. artists.Select(static artist => artist.Id)];
			return (TMutable)mutable;
		}
		#endregion
	}
}
