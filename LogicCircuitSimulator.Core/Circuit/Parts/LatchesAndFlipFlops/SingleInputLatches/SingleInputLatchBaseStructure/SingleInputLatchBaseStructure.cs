using CSharpEnhanced.Maths;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	public abstract class SingleInputLatchBaseStructure : LatchBaseStructure		
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public SingleInputLatchBaseStructure(string name, string inputLabel, ClockSignalType clockType) : base(name, clockType)
		{
			mInputLabel = inputLabel;
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Input socket of this SingleInputLatch
		/// </summary>
		private readonly InputSocket mInput = new InputSocket();

		/// <summary>
		/// Label to put near <see cref="Input"/>
		/// </summary>
		private readonly string mInputLabel;

		#endregion

		#region Public Properties

		/// <summary>
		/// Input socket of this SingleInputLatch
		/// </summary>
		public InputSocket Input => mInput;

		/// <summary>
		/// Label to put near <see cref="Input"/>
		/// </summary>
		public string InputLabel => mInputLabel;

		/// <summary>
		/// Carries information on whether this is still a single input latch
		/// </summary>
		public virtual bool IsSingleInput => true;

		#endregion

		#region Public Methods

		/// <summary>
		/// Computes the input socket coord and calls base implementation for other sockets
		/// </summary>
		public override void ComputeSocketCoords()
		{
			base.ComputeSocketCoords();

			Input.Position.Absolute = CenterCoord;
			Input.Position.Shift.Set(-75, -50, RotationAngleClockWise, AngleUnit.Degrees);
		}

		public override void Dispose()
		{
			
		}

		#endregion

	}
}
