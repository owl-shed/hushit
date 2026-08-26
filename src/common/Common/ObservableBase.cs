namespace OwlShed.Hushit.Common;

/// <summary>
/// 	Represents a base class for implementing the <see cref="INotifyPropertyChanged"/> pattern.
/// </summary>
public abstract class ObservableBase : INotifyPropertyChanged
{
	#region Events
	/// <inheritdoc/>
	public event PropertyChangedEventHandler? PropertyChanged;
	#endregion

	#region Methods
	/// <summary>Tries to update the <paramref name="field"/> with the <paramref name="newValue"/>, but only if they are different.</summary>
	/// <typeparam name="T">The type of the value that is being updated.</typeparam>
	/// <param name="field">The field where the property is being stored.</param>
	/// <param name="newValue">The new value to try and set the property to.</param>
	/// <param name="property">The name of the property that is being updated.</param>
	/// <returns></returns>
	/// <remarks>Equality is determined using <see cref="EqualityComparer{T}.Default"/>.</remarks>
	protected virtual bool TrySet<T>(ref T field, T newValue, [CallerMemberName] string? property = null)
	{
		if (EqualityComparer<T>.Default.Equals(field, newValue))
			return false;

		field = newValue;
		RaisePropertyChanged(property);

		return true;
	}

	/// <summary>Raises the <see cref="PropertyChanged"/> event.</summary>
	/// <param name="property">The name of the property to raise the event for, a <see langword="null"/> value means all properties.</param>
	protected void RaisePropertyChanged([CallerMemberName] string? property = null) => PropertyChanged?.Invoke(this, new(property));
	#endregion
}
