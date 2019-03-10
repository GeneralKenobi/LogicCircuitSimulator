using PropertyChanged;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Base class for view models in this application
	/// </summary>
	public class BaseViewModel : INotifyPropertyChanged
	{
		#region Property Changed

		/// <summary>
		/// The event that is fired when any child property changed its value
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// This method fires the PropertyChanged event. Given the fact that this ViewModel utilizes
		/// the PropertyChanged.Fody NuGet, it's mostly unnecesary, although if it would be required
		/// to manually call it, it's possible
		/// </summary>
		/// <param name="propertyName"></param>
		public void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		/// <summary>
		/// Fires property changed for every given property name
		/// </summary>
		/// <param name="names"></param>
		public void OnPropertyChanged(params string[] names)
		{
			foreach(var item in names)
			{
				OnPropertyChanged(item);
			}
		}

		/// <summary>
		/// Fires property changed event for the given property after the delay
		/// </summary>
		/// <param name="propertyName">Name of the property to notify for</param>
		/// <param name="delay">Delay before the property changed is fired, in ms (0-10000)</param>
		/// <returns></returns>
		public async Task DelayedOnPropertyChanged(string propertyName, int delay = 10)
		{
			// Check for the minimum value
			if(delay< MinimumDelay)
			{
				delay = MinimumDelay;
			}

			// Check for the maximum value
			if(delay > MaximumDelay)
			{
				delay = MaximumDelay;
			}

			// Await the delay
			await Task.Delay(delay);

			// Fire the property changed event
			OnPropertyChanged(propertyName);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Minimum delay for <see cref="DelayedOnPropertyChanged(string, int)"/>
		/// </summary>
		public int MinimumDelay => 0;

		/// <summary>
		/// Maximum delay for <see cref="DelayedOnPropertyChanged(string, int)"/>
		/// </summary>
		public int MaximumDelay => 10000;

		#endregion
	}
}