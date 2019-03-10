using CSharpEnhanced.Maths;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// A single coordinate in a 2d plane
	/// </summary>
    public class Coord
    {
		#region Constructors

		/// <summary>
		/// Default Constructor
		/// </summary>
		public Coord() { }

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Coord(double x, double y)
		{
			X = x;
			Y = y;
		}		

		#endregion

		#region Internal State Changed Event

		/// <summary>
		/// Event fired when the internal state of this socket changes
		/// </summary>
		public InternalStateChangedEventHandler InternalStateChanged;

		/// <summary>
		/// Invokes the <see cref="InternalStateChanged"/> event
		/// </summary>
		public void InvokeInternalStateChanged() => InternalStateChanged?.Invoke(this);

		/// <summary>
		/// Invokes the <see cref="InternalStateChanged"/> event
		/// </summary>
		private void DependsOnInternalStateChanged(object sender) =>
			InvokeInternalStateChanged();

		#endregion

		#region Private Members

		/// <summary>
		/// Current location of this coord expressed as a complex number (real part is the position on X axis
		/// and imaginary part is the position on the Y axis)
		/// </summary>
		private cdouble mValue = new cdouble();

		/// <summary>
		/// The coord is rounded to a multiple of this number
		/// </summary>
		private int mRoundTo = 25;

		#endregion

		#region Public Properties

		/// <summary>
		/// Position on the X (horizontal) axis
		/// </summary>
		public double X
		{
			get => mValue.Re;
			set
			{
				if (mValue.Re != value)
				{
					mValue.Re = value;
					Round();
					InvokeInternalStateChanged();
				}
			}
		}

		/// <summary>
		/// Position on the Y (vertical) axis
		/// </summary>
		public double Y
		{
			get => mValue.Im;
			set
			{
				if (mValue.Im != value)
				{
					mValue.Im = value;
					Round();
					InvokeInternalStateChanged();
				}
			}
		}

		/// <summary>
		/// The coord is rounded to a multiple of this number. If zero is assigned no rounding will be applied
		/// </summary>
		public int RoundTo
		{
			get => mRoundTo;
			set
			{
				if(value < 0)
				{
					value = 1;
				}

				mRoundTo = value;
			}
		}

		/// <summary>
		/// Returns true if this Coord is rounded
		/// </summary>
		public bool IsRounded => RoundTo != 0;

		#endregion

		#region Public Methods

		/// <summary>
		/// Sets both coordinates at once, fires <see cref="InternalStateChanged"/> event only once
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Set(double x, double y, double angle = 0, AngleUnit unit = AngleUnit.Radians)
		{
			// Assign the values
			mValue.Re = x;
			mValue.Im = y;

			if (angle != 0)
			{
				Rotate(angle, unit);
			}

			// Round the values
			Round();

			// Invoker internal state changed event
			InvokeInternalStateChanged();
		}
		
		/// <summary>
		/// Rotates this coord around point 0,0 by the given angle
		/// </summary>
		/// <param name="angle"></param>
		public void Rotate(double angle, AngleUnit unit = AngleUnit.Radians)
		{
			// Create a rotation multiplier - phase is the rotation angle and modulus is 1
			cdouble rotationMultiplier = new cdouble()
			{
				Mod = 1,
			};

			switch(unit)
			{
				case AngleUnit.Radians:
					{
						rotationMultiplier.Phase = angle;
					}
					break;

				case AngleUnit.Degrees:
					{
						rotationMultiplier.Phase = Math.PI*angle/180;
					}
					break;

				case AngleUnit.Turns:
					{
						rotationMultiplier.Phase = 2*Math.PI*angle;
					}
					break;
			}

			// Rotate the coord by multiplying it with the rotation multiplier
			mValue *= rotationMultiplier;

			Round();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Rounds this coordinate to values that are
		/// multiples of <paramref name="number"/>.
		/// Ex: number = 25, X,Y = (62, 19), round => X,Y = (50,25)
		/// </summary>
		/// <param name="number">A positive integer</param>
		public void Round()
		{
			if (RoundTo == 0)
			{
				return;
			}

			mValue.Re = RoundTo * Math.Round(X / RoundTo);
			mValue.Im = RoundTo * Math.Round(Y / RoundTo);
		}

		#endregion
	}
}