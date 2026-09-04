namespace OwlShed.Hushit.Data.Updates;

/// <summary>
///   Represents an update between two data models.
/// </summary>
public interface IDataModelUpdate
{
	#region Properties
	/// <inheritdoc cref="IDataModel.AddedOn"/>
	ValueUpdate<DateTimeOffset> AddedOn { get; set; }

	/// <inheritdoc cref="IDataModel.ModifiedOn"/>
	ValueUpdate<DateTimeOffset> ModifiedOn { get; set; }
	#endregion
}

/// <summary>
/// 	Represents the base class for an update between two data models.
/// </summary>
public abstract class DataModelUpdateBase : IDataModelUpdate
{
	#region Properties
	/// <inheritdoc/>
	public ValueUpdate<DateTimeOffset> AddedOn { get; set; }

	/// <inheritdoc/>
	public ValueUpdate<DateTimeOffset> ModifiedOn { get; set; }
	#endregion
}
