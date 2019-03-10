using CSharpEnhanced.Maths;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Logical NOT gate (flips the signal)
	/// </summary>
    public class NOTGate : BasePart
    {
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public NOTGate() : base("NOT Gate")
		{
			Width = 50;
			Height = 25;
			Input.InternalStateChanged += InputChanged;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Input of this NOT gate
		/// </summary>
		public InputSocket Input { get; private set; } = new InputSocket();
	
		/// <summary>
		/// Output of this NOT gate
		/// </summary>
		public OutputSocket Output { get; private set; } = new OutputSocket();

		#endregion

		#region Private Methods

		/// <summary>
		/// Updates the output (and internal state) whenever InternalState of
		/// its input socket is changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void InputChanged(object sender)
		{
			Output.Value = !Input.Value;
		}

		#endregion

		#region IDisposable

		/// <summary>
		/// Method which disposes of this part (makes sure all output nodes are
		/// removed from all clients)
		/// </summary>
		public override void Dispose()
		{
			NodeReferenceDictionary.RemoveAllReferences(Output.Node.ID);
		}

		#endregion

		#region Implementation of Abstract Methods

		/// <summary>
		/// Computs coordinates of the center of each socket
		/// </summary>
		public override void ComputeSocketCoords()
		{
			Input.Position.Absolute = CenterCoord;
			Input.Position.Shift.Set(-Width / 2, 0, RotationAngleClockWise, AngleUnit.Degrees);

			Output.Position.Absolute = CenterCoord;
			Output.Position.Shift.Set(Width / 2, 0, RotationAngleClockWise, AngleUnit.Degrees);
		}

		#endregion
	}
}