namespace OwlShed.Hushit.Data.Genres;

/// <summary>
/// 	Represents an update to the <see cref="IGenreReferencesInfo"/>.
/// </summary>
public interface IGenreReferencesUpdate
{
	#region Methods
	/// <summary>Adds a new related genre.</summary>
	/// <param name="id">The id of the genre to add.</param>
	/// <returns>The used update builder.</returns>
	IGenreReferencesUpdate AddGenre(string id);

	/// <summary>Removes a related genre.</summary>
	/// <param name="id">The id of the genre to remove.</param>
	/// <returns>The used update builder.</returns>
	IGenreReferencesUpdate RemoveGenre(string id);

	/// <summary>Sets the new genre ids.</summary>
	/// <param name="genreIds">The ids of the new related genres.</param>
	/// <returns>The used update builder.</returns>
	IGenreReferencesUpdate WithGenres(params IReadOnlyList<string> genreIds);
	#endregion
}

/// <summary>
/// 	Represents an update to the <see cref="IGenreReferencesInfo"/>.
/// </summary>
/// <typeparam name="TBuilder">The type of the actual builder used.</typeparam>
public interface IGenreReferencesUpdate<TBuilder> : IGenreReferencesUpdate
	where TBuilder : notnull, IGenreReferencesUpdate<TBuilder>
{
	#region Methods
	/// <inheritdoc cref="IGenreReferencesUpdate.AddGenre(string)"/>
	new TBuilder AddGenre(string id);

	/// <inheritdoc cref="IGenreReferencesUpdate.RemoveGenre(string)"/>
	new TBuilder RemoveGenre(string id);

	/// <inheritdoc cref="IGenreReferencesUpdate.WithGenres(IReadOnlyList{string})"/>
	new TBuilder WithGenres(params IReadOnlyList<string> genreIds);

	IGenreReferencesUpdate IGenreReferencesUpdate.AddGenre(string id) => AddGenre(id);
	IGenreReferencesUpdate IGenreReferencesUpdate.RemoveGenre(string id) => RemoveGenre(id);
	IGenreReferencesUpdate IGenreReferencesUpdate.WithGenres(params IReadOnlyList<string> genreIds) => WithGenres(genreIds);
	#endregion
}

/// <summary>
/// 	Contains various extensions related to the <see cref="IGenreReferencesUpdate"/>.
/// </summary>
public static class IGenreReferencesUpdateExtensions
{
	extension(IGenreReferencesUpdate update)
	{
		#region Methods
		/// <summary>Adds a new related genre.</summary>
		/// <param name="genre">The genre to add.</param>
		/// <returns>The used update builder.</returns>
		public IGenreReferencesUpdate AddGenre(IGenreInfo genre) => update.AddGenre(genre.Id);

		/// <summary>Removes a related genre.</summary>
		/// <param name="genre">The genre to remove.</param>
		/// <returns>The used update builder.</returns>
		public IGenreReferencesUpdate RemoveGenre(IGenreInfo genre) => update.RemoveGenre(genre.Id);

		/// <summary>Sets the new genre ids.</summary>
		/// <param name="genres">The new related genres.</param>
		/// <returns>The used update builder.</returns>
		public IGenreReferencesUpdate WithGenres(params IReadOnlyList<IGenreInfo> genres)
		{
			string[] ids = genres.Select(static genre => genre.Id).ToArray();
			return update.WithGenres(ids);
		}
		#endregion
	}
	extension<TBuilder>(IGenreReferencesUpdate<TBuilder> update) where TBuilder : notnull, IGenreReferencesUpdate<TBuilder>
	{
		#region Methods
		/// <summary>Adds a new related genre.</summary>
		/// <param name="genre">The genre to add.</param>
		/// <returns>The used update builder.</returns>
		public TBuilder AddGenre(IGenreInfo genre) => update.AddGenre(genre.Id);

		/// <summary>Removes a related genre.</summary>
		/// <param name="genre">The genre to remove.</param>
		/// <returns>The used update builder.</returns>
		public TBuilder RemoveGenre(IGenreInfo genre) => update.RemoveGenre(genre.Id);

		/// <summary>Sets the new genre ids.</summary>
		/// <param name="genres">The new related genres.</param>
		/// <returns>The used update builder.</returns>
		public TBuilder WithGenres(params IReadOnlyList<IGenreInfo> genres)
		{
			string[] ids = genres.Select(static genre => genre.Id).ToArray();
			return update.WithGenres(ids);
		}
		#endregion
	}
}
