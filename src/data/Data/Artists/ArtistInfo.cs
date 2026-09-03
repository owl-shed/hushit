namespace OwlShed.Hushit.Data.Artists;

internal sealed class ArtistInfo : DataModelBase<IArtistInfo, MutableArtist, ArtistUpdate>, IArtistInfo
{
	#region Fields
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly ObservableCollection<string> _aliases = [];
	#endregion

	#region Properties
	/// <inheritdoc/>
	protected override IDataRepository<IArtistInfo, MutableArtist, ArtistUpdate> Repository => Data.Artists;

	/// <inheritdoc/>
	public string Name { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public ReadOnlyObservableCollection<string> Aliases => new(_aliases);

	/// <inheritdoc cref="IAlbumReferencesInfo.AlbumIds"/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public ObservableCollection<string> AlbumIds { get; } = [];

	/// <inheritdoc cref="ITrackReferencesInfo.TrackIds"/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public ObservableCollection<string> TrackIds { get; } = [];

	/// <inheritdoc cref="IGenreReferencesInfo.GenreIds"/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public ObservableCollection<string> GenreIds { get; } = [];

	ReadOnlyObservableCollection<string> IAlbumReferencesInfo.AlbumIds => new(AlbumIds);
	ReadOnlyObservableCollection<string> ITrackReferencesInfo.TrackIds => new(TrackIds);
	ReadOnlyObservableCollection<string> IGenreReferencesInfo.GenreIds => new(GenreIds);
	#endregion

	#region Constructors
	public ArtistInfo(IHushitData data, string id, string name) : base(data, id)
	{
		Name = name;
	}
	public ArtistInfo(IHushitData data, string id, MutableArtist initialState) : base(data, id)
	{
		CopyState(initialState);

		Debug.Assert(Name is not null);
	}
	#endregion

	#region Methods
	public IAsyncEnumerable<IAlbumInfo> GetAlbumsAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.Albums, AlbumIds, cancellation);
	public IAsyncEnumerable<IGenreInfo> GetGenresAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.Genres, GenreIds, cancellation);
	public IAsyncEnumerable<ITrackInfo> GetTracksAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.Tracks, TrackIds, cancellation);
	public override MutableArtist ToMutable()
	{
		return new()
		{
			Name = Name,
			Aliases = [.. Aliases],
		};
	}
	internal override void CopyState(MutableArtist state)
	{
		using ReaderWriterWriteLock _ = Lock.WriteLock();

		if (state.Name is null)
			ThrowHelper.ThrowArgumentException(nameof(state), $"Expected the new state to have a name.");

		Name = state.Name;
		_aliases.Replace(state.Aliases);
	}
	#endregion
}
