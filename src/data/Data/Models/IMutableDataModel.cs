namespace OwlShed.Hushit.Data.Models;

/// <summary>
/// 	Represents a mutable version of an <see cref="IDataModel"/>.
/// </summary>
public interface IMutableDataModel
{
}

/// <summary>
/// 	Represents a mutable version of an <see cref="IDataModel"/>.
/// </summary>
/// <typeparam name="TSelf">The type of the mutable data model itself.</typeparam>
public interface IMutableDataModel<TSelf> : IMutableDataModel
	where TSelf : notnull, IMutableDataModel<TSelf>
{
}

/// <summary>
/// 	Represents a mutable version of an <see cref="IDataModel"/>.
/// </summary>
/// <typeparam name="TSelf">The type of the mutable data model itself.</typeparam>
/// <typeparam name="TUpdate">The type that represents an update between two mutable data models.</typeparam>
public interface IMutableDataModel<TSelf, TUpdate> : IMutableDataModel<TSelf>
	where TSelf : notnull, IMutableDataModel<TSelf, TUpdate>
	where TUpdate : notnull
{
	#region Methods
	/// <summary>Gets an update from the <paramref name="oldState"/>, to the current state of the data model.</summary>
	/// <param name="oldState">The old state of the data model.</param>
	/// <returns>The update from the <paramref name="oldState"/>, to the current state of the data model.</returns>
	TUpdate GetUpdateFrom(TSelf oldState);

	/// <summary>Gets an update from the current state, to the <paramref name="newState"/> of the data model.</summary>
	/// <param name="newState">The new state of the data model.</param>
	/// <returns>The update from the current state, to the <paramref name="newState"/> of the data model.</returns>
	TUpdate GetUpdateTo(TSelf newState);
	#endregion
}

/// <summary>
/// 	Represents a base type for mutable data models.
/// </summary>
/// <typeparam name="TSelf">The type of the mutable data model itself.</typeparam>
/// <typeparam name="TUpdate">The type that represents an update between two mutable data models.</typeparam>
public abstract class MutableDataModelBase<TSelf, TUpdate> : IMutableDataModel<TSelf, TUpdate>
	where TSelf : notnull, MutableDataModelBase<TSelf, TUpdate>, new()
	where TUpdate : notnull
{
	#region Methods
	/// <inheritdoc/>
	public abstract TUpdate GetUpdateFrom(TSelf oldState);

	/// <inheritdoc/>
	public virtual TUpdate GetUpdateTo(TSelf newState) => newState.GetUpdateFrom((TSelf)this);
	#endregion
}
