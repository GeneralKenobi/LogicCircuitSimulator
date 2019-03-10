using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Socket which acts as an output socket: its node's value can be set and upon
	/// deletion it will delete all references to itself. It's constructed with a Node
	/// that can't be later set (exception will be thrown if something tries to do that)
	/// </summary>
    public class OutputSocket : Socket
    {
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public OutputSocket()
		{
			// Create a new Node and store add it to the reference dictionary
			mNode = new Node();
			NodeReferenceDictionary.AddReference(mNode, this);
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Accessor to this Socket's value
		/// </summary>
		public new virtual bool Value
		{
			get => Node == null ? false : Node.Value;
			set
			{
				if (Node != null)
				{
					Node.Value = value;
					OnPropertyChanged("Value", "SocketState");
				}
			}
		}

		#endregion
	}
}
