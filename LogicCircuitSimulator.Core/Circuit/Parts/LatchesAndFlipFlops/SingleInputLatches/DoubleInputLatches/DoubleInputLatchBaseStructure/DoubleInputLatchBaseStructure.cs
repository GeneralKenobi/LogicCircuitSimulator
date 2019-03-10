using CSharpEnhanced.Maths;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public abstract class DoubleInputLatchBaseStructure : SingleInputLatchBaseStructure
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public DoubleInputLatchBaseStructure(string name, string firstInputLabel, string secondInputLabel, ClockSignalType clockType)
			: base(name, firstInputLabel, clockType)
		{
			mSecondInputLabel = secondInputLabel;
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Second input socket of this SingleInputLatch
		/// </summary>
		private readonly InputSocket mSecondInput = new InputSocket();	

		/// <summary>
		/// Label to put near <see cref="mSecondInput"/>
		/// </summary>
		private readonly string mSecondInputLabel;

		#endregion

		#region Public Properties	

		/// <summary>
		/// Second input socket of this SingleInputLatch
		/// </summary>
		public InputSocket SecondInput => mSecondInput;

		/// <summary>
		/// Label to put near <see cref="SecondInput"/>
		/// </summary>
		public string SecondInputLabel=> mSecondInputLabel;

		/// <summary>
		/// Carries information on whether this is still a single input latch
		/// </summary>
		public override bool IsSingleInput => false;

		#endregion

		#region Public Methods

		/// <summary>
		/// Computes the coords of input sockets and calls base implementation for other sockets
		/// </summary>
		public override void ComputeSocketCoords()
		{
			base.ComputeSocketCoords();

			SecondInput.Position.Absolute = CenterCoord;
			SecondInput.Position.Shift.Set(-75, 50, RotationAngleClockWise, AngleUnit.Degrees);
		}

		public override void Dispose()
		{
			
		}

		#endregion
	}
}
