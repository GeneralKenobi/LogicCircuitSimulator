using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Socket which behaves as an input socket: its value can't be set,
	/// when the Node which it contains was deleted, it will be deleted here too.
	/// </summary>
    public class InputSocket : Socket
    {
		#region Public Properties

		public void SetNode(Node value)
		{
			// If we already had an input assigned here:
			if (mNode != null)
			{
				// Unsubscribe from the property changed event 
				mNode.InternalStateChanged -= NodeInternalStateChanged;
			}

			mNode = value;

			// Assign the value, subscribe to PropertyChanged event
			// and add the new Node to the NodeReferenceDictionary
			if (value != null)
			{
				mNode.InternalStateChanged += NodeInternalStateChanged;
				NodeReferenceDictionary.AddReference(mNode, this);
			}

			InternalStateChanged?.Invoke(this);
			OnPropertyChanged(nameof(Node));
		}

		/// <summary>
		/// When true, this Node can be paired with output (i.e. Node==null)
		/// </summary>
		public bool CanPairWithOutput => Node == null;

		public int ConnectedWire { get; set; }

		#endregion
	}
}
