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

/// <summary>
/// 	Represents a mutable version of the <see cref="IGenreReferencesInfo"/>.
/// </summary>
/// <typeparam name="TSelf">The type that implements the mutable interface.</typeparam>
public interface IMutableGenreReferences<TSelf> : IMutableGenreReferences
	where TSelf : notnull, IMutableGenreReferences<TSelf>
{
}

/// <summary>
/// 	Contains various extensions related to the <see cref="IMutableGenreReferences"/>.
/// </summary>
public static class IMutableGenreReferencesExtensions
{
	extension(IMutableGenreReferences mutable)
	{
		#region Methods
		/// <summary>Adds a new related genre.</summary>
		/// <param name="id">The id of the genre to add.</param>
		/// <returns>The used mutable genre references instance.</returns>
		public IMutableGenreReferences AddGenre(string id)
		{
			mutable.GenreIds.Add(id);
			return mutable;
		}

		/// <summary>Removes a related genre.</summary>
		/// <param name="id">The id of the genre to remove.</param>
		/// <returns>The used mutable genre references instance.</returns>
		public IMutableGenreReferences RemoveGenre(string id)
		{
			mutable.GenreIds.Remove(id);
			return mutable;
		}

		/// <summary>Sets the new genre ids.</summary>
		/// <param name="genreIds">The ids of the new related genres.</param>
		/// <returns>The used mutable genre references instance.</returns>
		public IMutableGenreReferences WithGenres(params IReadOnlyList<string> genreIds)
		{
			mutable.GenreIds = [.. genreIds];
			return mutable;
		}
		#endregion
	}

	extension<TMutable>(IMutableGenreReferences<TMutable> mutable) where TMutable : notnull, IMutableGenreReferences<TMutable>
	{
		#region Methods
		/// <summary>Adds a new related genre.</summary>
		/// <param name="genre">The genre to add.</param>
		/// <returns>The used mutable genre references instance.</returns>
		public TMutable AddGenre(IGenreInfo genre)
		{
			mutable.GenreIds.Add(genre.Id);
			return (TMutable)mutable;
		}

		/// <summary>Removes a related genre.</summary>
		/// <param name="genre">The genre to remove.</param>
		/// <returns>The used mutable genre references instance.</returns>
		public TMutable RemoveGenre(IGenreInfo genre)
		{
			mutable.GenreIds.Remove(genre.Id);
			return (TMutable)mutable;
		}

		/// <summary>Sets the new genre ids.</summary>
		/// <param name="genres">The new related genres.</param>
		/// <returns>The used mutable genre references instance.</returns>
		public TMutable WithGenres(params IReadOnlyList<IGenreInfo> genres)
		{
			mutable.GenreIds = [.. genres.Select(static genre => genre.Id)];
			return (TMutable)mutable;
		}
		#endregion
	}
}
