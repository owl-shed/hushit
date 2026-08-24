namespace OwlShed.Hushit.Data.Artists;

/// <summary>
/// 	Represents a repository for artists.
/// </summary>
public interface IArtistRepository : IDataRepository<IArtistInfo, MutableArtist, ArtistUpdate>
{
	#region Methods
	/// <inheritdoc cref="IDataRepository{TModel, TMutable}.CreateAsync(Action{TMutable}, CancellationToken)"/>
	/// <remarks>
	/// 	Every artists requires:
	/// 	<list type="bullet">
	/// 		<item>A name.</item>
	/// 	</list>
	/// </remarks>
	new ValueTask<IArtistInfo> CreateAsync(Action<MutableArtist> callback, CancellationToken cancellation = default);
	#endregion
}
