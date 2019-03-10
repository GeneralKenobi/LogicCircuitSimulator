using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{

	/// <summary>
	/// A complemented OutputSocket 
	/// TODO: Introduce this socket to socket pairing in <see cref="MainViewModel"/> after a part that uses it is introduced
	/// </summary>
    public class ComplementedOutputSocket : Socket
    {
		#region Constructor

		/// <summary>
		/// Default constructor, has to be given an OutputSocket that it will complement which can't be null
		/// </summary>
		/// <param name="socketToComplement"></param>
		public ComplementedOutputSocket(OutputSocket socketToComplement)
		{
			if(socketToComplement==null)
			{
				throw new Exception("socketToComplement can't be null");
			}

			mNode = new ComplementedNode(socketToComplement.Node);
			mNode.InternalStateChanged += AssignedNodeInternalStateChanged;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method to call when internal state of complemented socket changes
		/// </summary>
		/// <param name="sender"></param>
		private void AssignedNodeInternalStateChanged(object sender) => InternalStateChanged?.Invoke(this);

		#endregion

		#region Public Properties

		/// <summary>
		/// Complemented value of this OutputSocket's Node
		/// </summary>
		public override bool Value => Node == null ? true : Node.Value;

		#endregion
	}
}