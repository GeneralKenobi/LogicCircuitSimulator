using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Socket which propagates state of a different Socket
	/// </summary>
    public class PropagatingOutputSocket : OutputSocket
    {
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public PropagatingOutputSocket() { }

		/// <summary>
		/// Constructor with a parameter
		/// </summary>
		/// <param name="propagateFrom"></param>
		public PropagatingOutputSocket(Socket propagateFrom)
		{
			PropagateFrom = propagateFrom;
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Socket to propagate the state from
		/// </summary>
		private Socket mPropagateFrom = null;

		#endregion

		#region Public Properties

		/// <summary>
		/// Value of this output socket
		/// </summary>
		public override bool Value
		{
			get => base.Value;
			set
			{
				if(Debugger.IsAttached)
				{
					// Break to indicate a propagating output socket is being set which is impossible
					Debugger.Break();
				}
			}
		}

		/// <summary>
		/// Sets the socket to propagate from
		/// </summary>
		public Socket PropagateFrom
		{
			set
			{
				// If the old value isn't null, unsubscribe from its internal state changed
				if(mPropagateFrom!=null)
				{
					mPropagateFrom.InternalStateChanged -= PropagateFromInternalStateChanged;
				}

				mPropagateFrom = value;

				// If the new value isn't null, subscribe to its internal state changed
				if (mPropagateFrom != null)
				{
					mPropagateFrom.InternalStateChanged += PropagateFromInternalStateChanged;
				}

				PropagateFromInternalStateChanged(null);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method to call when internal state of <see cref="mPropagateFrom"/> changes
		/// </summary>
		/// <param name="sender"></param>
		private void PropagateFromInternalStateChanged(object sender) =>
			mNode.Value = (mPropagateFrom == null ? false : mPropagateFrom.Value);

		#endregion
	}
}
