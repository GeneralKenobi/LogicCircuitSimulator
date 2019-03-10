using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public class JKLatch : DoubleInputLatchBaseStructure
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public JKLatch() : base("JK Latch", "J", "K", ClockSignalType.LevelDriven)
		{
			// Subscribe to internal state changed of both inputs
			Input.InternalStateChanged += InputChanged;
			SecondInput.InternalStateChanged += InputChanged;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method to call when input changes
		/// </summary>
		/// <param name="sender"></param>
		private void InputChanged(object sender)
		{
			if(AdjustedClock)
			{
				// Characteristic equation of JK latch
				SetQ((Input.Value && QComplement.Value) || (!SecondInput.Value && Q.Value));
			}
		}

		#endregion
	}
}
