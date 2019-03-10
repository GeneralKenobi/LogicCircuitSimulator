using CSharpEnhanced.Maths;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
    public class BaseInput : BasePart
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public BaseInput(string name) : base(name)
		{
			Width = 75;
			Height = 25;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Output of this part
		/// </summary>
		public OutputSocket Output { get; protected set; } = new OutputSocket();

		/// <summary>
		/// X coordinate of this part's top left cornet (origin is top left corner)
		/// </summary>
		public override double XCoord => CenterCoord.X - (Width / 3);

		/// <summary>
		/// Horizontal center point for part model rotation in UI
		/// </summary>
		public override double HorizontalRotationCenter => 25;

		/// <summary>
		/// Vertical center point for part model rotation in UI
		/// </summary>
		public override double VerticalRotationCenter => Height / 2;

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
						
		#region Abstract Method Implementation

		/// <summary>
		/// Computs coordinates of the center of each socket
		/// </summary>
		public override void ComputeSocketCoords()
		{
			Output.Position.Absolute = CenterCoord;
			Output.Position.Shift.Set(Width / 2, 0, RotationAngleClockWise, AngleUnit.Degrees);
		}

		#endregion
	}
}
