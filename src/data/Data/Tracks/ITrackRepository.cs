namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents a repository for tracks.
/// </summary>
public interface ITrackRepository : IDataRepository<ITrackInfo, ITrackUpdate>
{
	#region Methods
	/// <inheritdoc cref="IDataRepository{TModel, TUpdate}.CreateAsync(Action{TUpdate}, CancellationToken)"/>
	/// <remarks>
	/// 	Every track requires:
	/// 	<list type="bullet">
	/// 		<item>A name.</item>
	/// 		<item>A duration.</item>
	/// 	</list>
	/// </remarks>
	new ValueTask<ITrackInfo> CreateAsync(Action<ITrackUpdate> callback, CancellationToken cancellation = default);
	#endregion
}
