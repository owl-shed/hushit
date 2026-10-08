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

	/// <summary>Atomically gets or creates an artist with the given <paramref name="name"/>.</summary>
	/// <param name="name">The name of the artist to get or create.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns>The obtained, or created artist.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask<IArtistInfo> GetOrCreateAsync(string name, CancellationToken cancellation = default);
	#endregion
}
