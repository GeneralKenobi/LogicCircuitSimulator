using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	public class ORGate : MultiInputGate
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public ORGate() : base("OR Gate", MultiInputGateType.OR) { }

		#endregion

		#region Protected virtual methods

		/// <summary>
		/// Method called when an input chanes, computes the output
		/// </summary>
		/// <param name="sender"></param>
		protected override void InputChanged(object sender)
		{
			foreach(var socket in Inputs)
			{
				if(socket.Value)
				{
					// If at least 1 input is on then OR gives 1
					Output.Value = true;
					return;
				}
			}

			Output.Value = false;
		}

		#endregion
	}
}
