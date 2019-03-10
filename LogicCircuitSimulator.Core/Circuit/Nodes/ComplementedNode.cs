using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public class ComplementedNode : Node
    {
		#region Constructor

		/// <summary>
		/// Default constructor with one parameter
		/// </summary>
		/// <param name="dependentOn"></param>
		public ComplementedNode(Node dependentOn)
		{
			if(dependentOn==null)
			{
				throw new Exception("Dependent on node can't be null");
			}

			mDependentOn = dependentOn;
			mDependentOn.InternalStateChanged += (s) => InternalStateChanged?.Invoke(this);
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Node this node is dependent on
		/// </summary>
		private readonly Node mDependentOn;

		#endregion

		#region Public Properties

		/// <summary>
		/// Value of this node
		/// </summary>
		public override bool Value
		{
			get => !mDependentOn.Value;
			set
			{
				if(Debugger.IsAttached)
				{
					// Indicate that value of this node shouldn't be set
					Debugger.Break();
				}
			}
		}

		#endregion
	}
}
