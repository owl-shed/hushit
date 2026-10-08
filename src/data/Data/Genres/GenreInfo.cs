namespace OwlShed.Hushit.Data.Genres;

internal sealed class GenreInfo : DataModelBase<IGenreInfo, MutableGenre, GenreUpdate>, IGenreInfo
{
	#region Properties
	/// <inheritdoc/>
	protected override IDataRepository<IGenreInfo, MutableGenre, GenreUpdate> Repository => Data.Genres;

	/// <inheritdoc/>
	public string Name { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc cref="ITrackReferencesInfo.TrackIds"/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public ObservableCollection<string> TrackIds { get; } = [];

	/// <inheritdoc cref="IAlbumReferencesInfo.AlbumIds"/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public ObservableCollection<string> AlbumIds { get; } = [];

	/// <inheritdoc cref="IArtistReferencesInfo.ArtistIds"/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public ObservableCollection<string> ArtistIds { get; } = [];

	ReadOnlyObservableCollection<string> ITrackReferencesInfo.TrackIds => new(TrackIds);
	ReadOnlyObservableCollection<string> IAlbumReferencesInfo.AlbumIds => new(AlbumIds);
	ReadOnlyObservableCollection<string> IArtistReferencesInfo.ArtistIds => new(ArtistIds);
	#endregion

	#region Constructors
	public GenreInfo(IHushitData data, string id, MutableGenre initialState) : base(data, id, initialState)
	{
		Debug.Assert(Name is not null);
	}
	#endregion

	#region Methods
	public IAsyncEnumerable<IAlbumInfo> GetAlbumsAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.Albums, AlbumIds, cancellation);
	public IAsyncEnumerable<IArtistInfo> GetArtistsAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.Artists, ArtistIds, cancellation);
	public IAsyncEnumerable<ITrackInfo> GetTracksAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.Tracks, TrackIds, cancellation);
	public override MutableGenre ToMutable()
	{
		return new()
		{
			AddedOn = AddedOn,
			ModifiedOn = ModifiedOn,
			Name = Name
		};
	}
	internal override void CopyState(MutableGenre state)
	{
		using ReaderWriterWriteLock _ = Lock.WriteLock();

		if (state.Name is null)
			ThrowHelper.ThrowArgumentException(nameof(state), $"Expected the new state to have a name.");

		AddedOn = state.AddedOn;
		ModifiedOn = state.ModifiedOn;
		Name = state.Name;
	}
	#endregion
}
