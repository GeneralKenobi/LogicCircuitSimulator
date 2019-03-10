using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public class DMUX : MuxDmuxBaseStructure<OutputSocket, InputSocket>
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public DMUX() : base("Demultiplexer")
		{
			// Subscribe to selected socket changed
			SelectedSocketChanged += SelectedOutputChanged;
			SingleSocket.InternalStateChanged += InputChanged;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method to call when address changes. Resets the old socket
		/// </summary>
		/// <param name="sender"></param>
		private void SelectedOutputChanged(object sender, SelectedSocketChangedEventArgs args)
		{
			if (args.OldValue is OutputSocket oldS)
			{
				oldS.Value = false;
			}

			if(args.NewValue is OutputSocket newS)
			{
				newS.Value = SingleSocket.Value;
			}
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Method to call when the input changes
		/// </summary>
		/// <param name="sender"></param>
		protected void InputChanged(object sender)
		{
			if (SelectedMultiSocket != null)
			{
				SelectedMultiSocket.Value = SingleSocket.Value;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Calculates coordinates for sockets
		/// </summary>
		public override void ComputeSocketCoords()
		{
			// TODO: Implement when UI is ready
			throw new NotImplementedException();
		}

		/// <summary>
		/// Disposes this part
		/// </summary>
		public override void Dispose() { }

		#endregion
	}
}
