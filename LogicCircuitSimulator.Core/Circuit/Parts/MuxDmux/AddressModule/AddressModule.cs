using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	public class AddressModule : BaseViewModel
	{
		#region Constructor
		
		/// <summary>
		/// Default Constructor
		/// </summary>
		public AddressModule()
		{
			Size = MinimumSize;
		}

		#endregion

		#region Private Members

		/// <summary>
		/// InputSockets in this address module
		/// </summary>
		private readonly ObservableCollection<InputSocket> mAddresses = new ObservableCollection<InputSocket>();

		#endregion

		#region Public Members

		/// <summary>
		/// Event fired whenever internal state in one of the addresses changes
		/// </summary>
		public InternalStateChangedEventHandler InternalStateChanged;

		#endregion

		#region Public Properties

		/// <summary>
		/// Getter to the sockets, copies the collection and returns a copy. Changing size is possible only
		/// using the <see cref="Size"/> property
		/// </summary>
		public ObservableCollection<InputSocket> Addresses => new ObservableCollection<InputSocket>(mAddresses);

		/// <summary>
		/// Size of this collection, number of sockets is 2^size
		/// </summary>
		public int Size
		{
			get => mAddresses.Count;
			set
			{
				// Check if value isn't identical
				if (value == mAddresses.Count)
				{
					return;
				}

				// Check the minimum
				if (value < MinimumSize)
				{
					value = MinimumSize;
				}

				// If the value is smaller
				if (value < Size)
				{
					// Remove unnecessary addresses
					while (mAddresses.Count > value)
					{
						mAddresses[mAddresses.Count - 1].InternalStateChanged -= PropagateInternalStateChanged;
						mAddresses.RemoveAt(mAddresses.Count - 1);
					}
				}
				else
				{
					// Otherwise add new addresses
					while (mAddresses.Count < value)
					{
						var newInput = new InputSocket();
						newInput.InternalStateChanged += PropagateInternalStateChanged;
						mAddresses.Add(newInput);
					}
				}

				// Notify about changed properties
				OnPropertyChanged(nameof(Size), nameof(Addresses), nameof(SelectedPosition));
			}
		}
		
		/// <summary>
		/// Position selected by the addresses
		/// (states written as a binary number converted to decimal, 0-th address is highest priority)
		/// </summary>
		public int SelectedPosition
		{
			get
			{
				int result = 0;

				for(int i=0; i<mAddresses.Count; ++i)
				{
					if(mAddresses[i].Value)
					{
						// The i-th address from the beginning contributes as 2 to power total-i-1
						result += (int)Math.Pow(2, mAddresses.Count - i - 1);
					}
				}

				return result;
			}
		}

		#endregion

		#region Public Static Properties

		/// <summary>
		/// Minimum size of the address module
		/// </summary>
		public static int MinimumSize => 1;

		#endregion

		#region Private Methods

		/// <summary>
		/// Propagates internal state changed from addresses
		/// </summary>
		/// <param name="sender"></param>
		private void PropagateInternalStateChanged(object sender) => InternalStateChanged?.Invoke(sender);

		#endregion

		#region Operators

		/// <summary>
		/// Getter to the i-th socket in the collection
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public InputSocket this[int i] => mAddresses[i];

		/// <summary>
		/// Increments the size of the module
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static AddressModule operator ++(AddressModule m)
		{
			++m.Size;
			return m;
		}

		/// <summary>
		/// Decrements the size of the module
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static AddressModule operator --(AddressModule m)
		{
			--m.Size;
			return m;
		}

		#endregion
	}
}
