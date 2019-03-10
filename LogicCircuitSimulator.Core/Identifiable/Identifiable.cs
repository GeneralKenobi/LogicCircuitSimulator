using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public class Identifiable
    {
		#region Public Properties

		/// <summary>
		/// ID assigned to this object
		/// </summary>
		public int ID { get; private set; } = -1;

		#endregion

		#region Constructor/Destructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Identifiable()
		{
			ID = IDManager.AssignID(this);
		}

		/// <summary>
		/// Default destructor
		/// </summary>
		~Identifiable()
		{
			IDManager.FreeID(this);
		}

		#endregion
	}
}
