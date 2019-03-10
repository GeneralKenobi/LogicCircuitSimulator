using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public abstract class MuxDmuxBaseStructure<MultiSocketType, SingleSocketType> : BasePart
		where MultiSocketType : Socket, new()
		where SingleSocketType : Socket, new()
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public MuxDmuxBaseStructure(string name) : base(name)
		{
			// Assign the default selected input
			mSelectedMultiSocket = mMultiSockets[0];

			// Subscribe to internal state changes in addresses
			mAddresses.InternalStateChanged += AddressChanged;
			Size = 2;
			Height = Math.Pow(2, Size) * 25;
			Width = Size * 25;
		}

		#endregion

		#region Events

		/// <summary>
		/// Event fired whenever selected socket changes
		/// </summary>
		public SelectedSocketChangedEventHandler SelectedSocketChanged;

		#endregion

		#region Private Members

		/// <summary>
		/// Addresses of this part
		/// </summary>
		private readonly AddressModule mAddresses = new AddressModule();

		/// <summary>
		/// Multiple sockets of this part (in general, inputs for MUX and outputs for DMUX)
		/// </summary>
		private readonly ExpSocketCollection<MultiSocketType> mMultiSockets = new ExpSocketCollection<MultiSocketType>();

		/// <summary>
		/// Single input of this part (in general, output for MUX and input for DMUX)
		/// </summary>
		private readonly SingleSocketType mSingleSocket = new SingleSocketType();

		/// <summary>
		/// MultiSocketType selected by the addresses
		/// </summary>
		private MultiSocketType mSelectedMultiSocket = null;

		/// <summary>
		/// Size of this part
		/// </summary>
		private int mSize = 1;

		#endregion

		#region Protected Properties

		/// <summary>
		/// MultiSocketType selected by the addresses
		/// </summary>
		protected MultiSocketType SelectedMultiSocket => mSelectedMultiSocket;

		#endregion

		#region Public Properties

		/// <summary>
		/// Addresses of this Multiplexer
		/// </summary>
		public AddressModule Addresses => mAddresses;

		/// <summary>
		/// Multiple sockets of this part (in general, inputs for MUX and outputs for DMUX)
		/// </summary>
		public ExpSocketCollection<MultiSocketType> MultiSockets => mMultiSockets;

		/// <summary>
		/// Single input of this part (in general, output for MUX and input for DMUX)
		/// </summary>
		public SingleSocketType SingleSocket => mSingleSocket;

		/// <summary>
		/// Size of this part
		/// </summary>
		public int Size
		{
			get => mSize;
			set
			{
				if (value == mSize)
				{
					// If the value is the same return
					return;
				}

				else if(value < MinimumSize)
				{
					// If it's smaller than the smallest possible, adjust
					value = MinimumSize;
				}

				else if(value > MaximumSize)
				{
					// If it's greater than the greatest possible, adjust
					value = MaximumSize;
				}

				// Assign the value
				mSize = value;

				// And modify the socket collections
				mMultiSockets.Size = mSize;
				mAddresses.Size = mSize;

				Height = Math.Pow(2, Size) * 25;
				Width = Size * 25;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method to call when address changes. Reassigns subscription to InternalStateChanged to the new selected input
		/// </summary>
		/// <param name="sender"></param>
		private void AddressChanged(object sender)
		{
			// Create the argument for the event
			SelectedSocketChangedEventArgs args =
				new SelectedSocketChangedEventArgs(mSelectedMultiSocket, MultiSockets[Addresses.SelectedPosition]);

			// Assign the new selected input
			mSelectedMultiSocket = MultiSockets[Addresses.SelectedPosition];

			// Fire the event
			SelectedSocketChanged?.Invoke(this, args);
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Disposes this part
		/// </summary>
		public override void Dispose() { }

		#endregion

		#region Public Static Properties

		/// <summary>
		/// Minimum size for a MUX/DMUX
		/// </summary>
		public static int MinimumSize => 1;

		/// <summary>
		/// Maximum size for a MUX/DMUX
		/// </summary>
		public static int MaximumSize => 4;

		#endregion
	}
}
