namespace OwlShed.Hushit.Data.Artists;

/// <summary>
/// 	Represents a repository for artists.
/// </summary>
public interface IArtistRepository : IDataRepository<IArtistInfo, IArtistUpdate>
{
	#region Methods
	/// <inheritdoc cref="IDataRepository{TModel, TUpdate}.CreateAsync(Action{TUpdate}, CancellationToken)"/>
	/// <remarks>
	/// 	Every artists requires:
	/// 	<list type="bullet">
	/// 		<item>A name.</item>
	/// 	</list>
	/// </remarks>
	new ValueTask<IArtistInfo> CreateAsync(Action<IArtistUpdate> callback, CancellationToken cancellation = default);
	#endregion
}
