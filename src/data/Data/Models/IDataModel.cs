namespace OwlShed.Hushit.Data.Models;

/// <summary>
/// 	Represents the base interface for a data model.
/// </summary>
public interface IDataModel : INotifyPropertyChanged
{
	#region Properties
	/// <summary>The id of the data model.</summary>
	string Id { get; }

	/// <summary>whether the model still exists in the repository.</summary>
	bool Exists { get; }
	#endregion

	#region Methods
	/// <summary>Tries to remove the data model from the repository.</summary>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns><see langword="true"/> if the model was removed, <see langword="false"/> if it didn't exist any more before this call.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask<bool> RemoveAsync(CancellationToken cancellation = default);
	#endregion
}

/// <summary>
/// 	Represents the base interface for a data model.
/// </summary>
/// <typeparam name="TUpdate">The type that represents the model update builder.</typeparam>
public interface IDataModel<out TUpdate> : IDataModel
	where TUpdate : notnull
{
	#region Methods
	/// <summary>Updates the data model.</summary>
	/// <param name="callback">The callback which can be used to customise the data model.</param>
	/// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
	/// <returns><see langword="true"/> if any changes were made, <see langword="false"/> otherwise.</returns>
	/// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
	ValueTask<bool> UpdateAsync(Action<TUpdate> callback, CancellationToken cancellation = default);
	#endregion
}

/// <summary>
/// 	Represents the base interface for a data model.
/// </summary>
/// <typeparam name="TUpdate">The type that represents the model update builder.</typeparam>
/// <typeparam name="TMutable">The type that represents the mutable version of the data model.</typeparam>
public interface IDataModel<out TUpdate, out TMutable> : IDataModel<TUpdate>
	where TUpdate : notnull
	where TMutable : notnull, IMutableDataModel<TMutable, TUpdate>
{
	#region Methods
	/// <summary>Gets a mutable version of the data model, which can be used to generate an update.</summary>
	/// <returns>A copy of the current data model, as a mutable version.</returns>
	TMutable ToMutable();
	#endregion
}
