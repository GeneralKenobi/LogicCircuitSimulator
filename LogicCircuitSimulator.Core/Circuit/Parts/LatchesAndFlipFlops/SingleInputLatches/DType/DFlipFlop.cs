using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	public class DFlipFlop : SingleInputLatchBaseStructure
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public DFlipFlop() : base("D flip flop", "D", ClockSignalType.EdgeDriven)
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
				SetQ(Input.Value);
			}
		}

		#endregion		
	}
}
