using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public class JKFlipFlop : DoubleInputLatchBaseStructure
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public JKFlipFlop() : base("JK flip flop", "J", "K", ClockSignalType.EdgeDriven)
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
			if(AdjustedClock)
			{
				SetQ((Input.Value && QComplement.Value) || (!SecondInput.Value && Q.Value));
			}
		}

		#endregion
	}
}
