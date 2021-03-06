﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	public class XORGate : MultiInputGate
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public XORGate() : base("XOR Gate", MultiInputGateType.XOR) { }

		#endregion

		#region Protected virtual methods

		/// <summary>
		/// Method called when an input chanes, computes the output
		/// </summary>
		/// <param name="sender"></param>
		protected override void InputChanged(object sender)
		{
			int numberOfOnInputs = 0;

			// Count the number of on inputs
			foreach(var socket in Inputs)
			{
				if(socket.Value)
				{
					++numberOfOnInputs;
				}
			}

			if (numberOfOnInputs % 2 == 1)
			{
				// If it's odd then XOR gives 1
				Output.Value = true;
			}
			else
			{
				// If it's even then XOR gives 0
				Output.Value = false;
			}
		}

		#endregion
	}
}
