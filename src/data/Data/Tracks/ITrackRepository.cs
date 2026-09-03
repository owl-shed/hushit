namespace OwlShed.Hushit.Data.Tracks;

/// <summary>
/// 	Represents a repository for tracks.
/// </summary>
public interface ITrackRepository : IDataRepository<ITrackInfo, MutableTrack, TrackUpdate>
{
	#region Methods
	/// <inheritdoc cref="IDataRepository{TModel, TMutable}.CreateAsync(Action{TMutable}, CancellationToken)"/>
	/// <remarks>
	/// 	Every track requires:
	/// 	<list type="bullet">
	/// 		<item>A name.</item>
	/// 		<item>A duration.</item>
	/// 	</list>
	/// </remarks>
	new ValueTask<ITrackInfo> CreateAsync(Action<MutableTrack> callback, CancellationToken cancellation = default);
	#endregion
}
