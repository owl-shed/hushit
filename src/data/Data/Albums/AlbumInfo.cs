namespace OwlShed.Hushit.Data.Albums;

internal sealed class AlbumInfo : DataModelBase<IAlbumInfo, MutableAlbum, AlbumUpdate>, IAlbumInfo
{
	#region Fields
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly ObservableCollection<string> _artistIds = [];
	#endregion

	#region Properties
	/// <inheritdoc/>
	protected override IDataRepository<IAlbumInfo, MutableAlbum, AlbumUpdate> Repository => Data.Albums;

	/// <inheritdoc/>
	public string Name { get => Read(ref field); private set => TrySet(ref field, value); }

	/// <inheritdoc/>
	public ReadOnlyObservableCollection<string> ArtistIds => new(_artistIds);

	/// <inheritdoc cref="ITrackReferencesInfo.TrackIds"/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public ObservableCollection<string> TrackIds { get; } = [];

	/// <inheritdoc cref="IGenreReferencesInfo.GenreIds"/>
	/// <remarks>This should only be modified by the repository.</remarks>
	public ObservableCollection<string> GenreIds { get; } = [];

	ReadOnlyObservableCollection<string> ITrackReferencesInfo.TrackIds => new(TrackIds);
	ReadOnlyObservableCollection<string> IGenreReferencesInfo.GenreIds => new(GenreIds);
	#endregion

	#region Constructors
	public AlbumInfo(IHushitData data, string id, MutableAlbum initialState) : base(data, id, initialState)
	{
		Debug.Assert(Name is not null);
	}
	#endregion

	public IAsyncEnumerable<IArtistInfo> GetArtistsAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.Artists, ArtistIds, cancellation);
	public IAsyncEnumerable<IGenreInfo> GetGenresAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.Genres, GenreIds, cancellation);
	public IAsyncEnumerable<ITrackInfo> GetTracksAsync(CancellationToken cancellation = default) => GetReferencedAsync(Data.Tracks, TrackIds, cancellation);
	public override MutableAlbum ToMutable()
	{
		return new()
		{
			AddedOn = AddedOn,
			ModifiedOn = ModifiedOn,
			Name = Name,
			ArtistIds = [.. _artistIds]
		};
	}
	internal override void CopyState(MutableAlbum state)
	{
		using ReaderWriterWriteLock _ = Lock.WriteLock();

		if (state.Name is null)
			ThrowHelper.ThrowArgumentException(nameof(state), $"Expected the new state to have a name.");

		AddedOn = state.AddedOn;
		ModifiedOn = state.ModifiedOn;
		Name = state.Name;
		_artistIds.Replace(state.ArtistIds);
	}
}
