using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	public class MUX : MuxDmuxBaseStructure<InputSocket, OutputSocket>
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public MUX() : base("Multiplexer")
		{
			// Subscribe to selected socket changed
			SelectedSocketChanged += SelectedInputChanged;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method to call when address changes. Reassigns subscription to InternalStateChanged to the new selected input
		/// </summary>
		/// <param name="sender"></param>
		private void SelectedInputChanged(object sender, SelectedSocketChangedEventArgs args)
		{
			if (args.OldValue != null)
			{				
				args.OldValue.InternalStateChanged -= InputChanged;
			}

			if(args.NewValue != null)
			{
				args.NewValue.InternalStateChanged -= InputChanged;
			}
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Method to call when the selected input changes
		/// </summary>
		/// <param name="sender"></param>
		protected void InputChanged(object sender) =>
			SingleSocket.Value = SelectedMultiSocket == null ? false : SelectedMultiSocket.Value;

		#endregion

		#region Public Methods

		/// <summary>
		/// Calculates coordinates for sockets
		/// </summary>
		public override void ComputeSocketCoords()
		{
			// TODO: Implement when UI is ready
		}

		/// <summary>
		/// Disposes this part
		/// </summary>
		public override void Dispose() { }

		#endregion
	}
}
	