using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	public class NORGate : MultiInputGate
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public NORGate() : base("NOR Gate", MultiInputGateType.NOR) { }

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
					// If at least one output is on, NOR will give 0
					Output.Value = false;
					return;
				}
			}

			Output.Value = true;
		}

		#endregion
	}
}
