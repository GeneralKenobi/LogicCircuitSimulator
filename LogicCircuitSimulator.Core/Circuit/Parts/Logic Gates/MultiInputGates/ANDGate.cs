using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Logical AND gate (it is positive if and only if all inputs are positive)
	/// </summary>
    public class ANDGate : MultiInputGate
    {
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public ANDGate() : base("AND Gate", MultiInputGateType.AND) { }

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
					// If at least one input is off then AND will give 0
					Output.Value = false;
					return;
				}
			}

			Output.Value = true;
		}

		#endregion
	}
}
