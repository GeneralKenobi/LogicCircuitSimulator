using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Enum for types of clock signals
	/// </summary>
	public enum ClockSignalType
	{
		/// <summary>
		/// Clock sensitive to signal's high level (1/on state)
		/// </summary>
		LevelDriven = 0,

		/// <summary>
		/// Clock sensitive to signal's change between levels from 0 to 1
		/// </summary>
		EdgeDriven = 1,
	}



	/// <summary>
	/// Enum for different states in this app
	/// </summary>
    public enum AppState
	{
		/// <summary>
		/// The standard state
		/// </summary>
		Idle = 0,

		/// <summary>
		/// New part is added on click
		/// </summary>
		AddingParts = 1,

		/// <summary>
		/// Node pairing is going on (placing wire between 2 nodes)
		/// </summary>
		WireManipulation = 2,
	}

	/// <summary>
	/// State of a socket
	/// </summary>
	public enum SocketState
	{
		/// <summary>
		/// Standard state, socket is off
		/// </summary>
		StandardOff = 0,

		/// <summary>
		/// Standard state, socket is on
		/// </summary>
		StandardOn = 1,

		/// <summary>
		/// Socket is not connected (its node is null)
		/// </summary>
		NotConnected = 2,

		/// <summary>
		/// Socket is highlighted
		/// </summary>
		Highlighted = 3,
	}

	/// <summary>
	/// Type of a multi input gate
	/// </summary>
	public enum MultiInputGateType
	{
		/// <summary>
		/// OR gate - returns 1 if at least one input is 1
		/// </summary>
		OR = 0,

		/// <summary>
		/// NOR gate - returns 1 if no inputs are 1
		/// </summary>
		NOR = 1,

		/// <summary>
		/// AND gate - returns 1 if all inputs are 1
		/// </summary>
		AND = 2,

		/// <summary>
		/// NAND gate - returns 1 if at least one input is 0
		/// </summary>
		NAND = 3,

		/// <summary>
		/// XOR gate - returns 1 if an odd number of inputs is 1
		/// </summary>
		XOR = 4,

		/// <summary>
		/// XNOR gate - returns 1 if an even number of inputs is 1
		/// </summary>
		XNOR = 5,
	}
}
