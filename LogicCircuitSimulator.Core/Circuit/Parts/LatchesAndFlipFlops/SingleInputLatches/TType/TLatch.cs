using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public class TLatch : SingleInputLatchBaseStructure
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public TLatch() : base("T Latch", "T", ClockSignalType.LevelDriven)
		{
			// Subscribe to input's internal state changed event
			Input.InternalStateChanged += InputChanged;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Sets the output if clock is in proper state
		/// </summary>
		/// <param name="sender"></param>
		protected void InputChanged(object sender)
		{
			if (AdjustedClock && Input.Value)
			{
				SetQ(!Q.Value);
			}
		}

		#endregion	
	}
}
