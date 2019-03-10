using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Event for when an internal state of a <see cref="Node"/> changes
	/// </summary>
	/// <param name="sender"></param>
	public delegate void InternalStateChangedEventHandler(object sender);  
}