using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Logical AND gate (it is positive if and only if all inputs are positive)
	/// </summary>
    public class NANDGate : MultiInputGate
    {
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public NANDGate() : base("NAND Gate", MultiInputGateType.NAND) { }

		#endregion

		#region Private Methods

		/// <summary>
		/// Method called when an input chanes, computes the output
		/// </summary>
		/// <param name="sender"></param>
		protected override void InputChanged(object sender)
		{
			foreach (var socket in Inputs)
			{
				if (!socket.Value)
				{
					// If at least one inpit is off then NAND will give 1
					Output.Value = true;
					return;
				}
			}

			Output.Value = false;
		}

		#endregion
	}
}
