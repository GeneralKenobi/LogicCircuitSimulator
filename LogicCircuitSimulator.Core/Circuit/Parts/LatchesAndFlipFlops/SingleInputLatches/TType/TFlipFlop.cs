using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public class TFlipFlop : SingleInputLatchBaseStructure
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public TFlipFlop() : base("T flip flop", "T", ClockSignalType.EdgeDriven)
		{
			// Subscribe to clock's internal state changed
			Clock.InternalStateChanged += ClockChanged;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method to call when clock changes
		/// </summary>
		/// <param name="sender"></param>
		private void ClockChanged(object sender)
		{
			if (AdjustedClock && Input.Value)
			{
				// If it's a proper clock cycle and the input is on, toggle the output
				SetQ(!Q.Value);
			}
		}

		#endregion	
	}
}
