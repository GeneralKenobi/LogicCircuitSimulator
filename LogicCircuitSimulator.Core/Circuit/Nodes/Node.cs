using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// The most basic node 
	/// </summary>
	public class Node : Identifiable, INotifyPropertyChanged, IDisposable
	{		
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Node()
		{
			
		}

		#endregion		

		#region Property Changed Event

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region InternalStateChangedEvent

		/// <summary>
		/// Event fired when the internal state of this node changes
		/// </summary>
		public InternalStateChangedEventHandler InternalStateChanged;

		#endregion

		#region Private Members

		/// <summary>
		/// The backing store for <see cref="Value"/>
		/// </summary>
		private bool mValue;

		/// <summary>
		/// When true, this node is ready to update (There's a minimum 20ms delay between
		/// updates in order to avoid StackOverflowExcpetions, for example when a NOT gate
		/// is connected to itself)
		/// </summary>
		private bool mReadyToUpdate = true;

		/// <summary>
		/// The last caught value which will be set when the current Delay has ended
		/// </summary>
		private bool lastCaughtValue;

		#endregion

		#region Public Properties

		/// <summary>
		/// Value assigned to this node. The shortest amount of time between updates
		/// is 20ms (Only the last value in that interval will be assigned)
		/// </summary>
		public virtual bool Value
		{
			get => mValue;
			set
			{
				// If we're ready to update
				if (mReadyToUpdate)
				{
					if (mValue != value)
					{
						// Assign the value to mValue and as the last caught value
						mValue = value;
						lastCaughtValue = value;

						// Begin delay
						Delay();

						// Fire Events
						OnValueChanged(this);
					}
				}
				else
				{
					// If we're not, update the lastCaughtValue
					lastCaughtValue = value;
				}
			}
		}
		
		#endregion

		#region Private Methods

		/// <summary>
		/// Delays the update of this Node (to avoid infinite update loops and
		/// StackOverflowExceptions, for example when NOT gate is connected to itself)
		/// </summary>
		private async void Delay()
		{
			mReadyToUpdate = false;
			await Task.Delay(10);
			mReadyToUpdate = true;
			Value = lastCaughtValue;
		}

		/// <summary>
		/// Calls InternalStateChanged and PropertyChanged events for when Value has changed
		/// </summary>
		/// <param name="sender"></param>
		private void OnValueChanged(object sender)
		{
			InternalStateChanged?.Invoke(this);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
		}

		#endregion

		#region IDisposable

		/// <summary>
		/// Disposes of this node (arranges to have all objects that reference it
		/// stop referencing it)
		/// </summary>
		public void Dispose()
		{
			Value = false;
		}

		#endregion				
	}
}
