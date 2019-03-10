using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	public class DLatch : SingleInputLatchBaseStructure
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public DLatch() : base("D Latch", "D", ClockSignalType.LevelDriven)
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
			if(AdjustedClock)
			{
				SetQ(Input.Value);
			}
		}

		#endregion		
	}
}
