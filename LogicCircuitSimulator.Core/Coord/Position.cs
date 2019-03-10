using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public class Position
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Position()
		{
			mAbsolute.InternalStateChanged += PropagateInternalStateChanged;
			mShift.InternalStateChanged += PropagateInternalStateChanged;
		}

		/// <summary>
		/// Constructor with absolute position parameters
		/// </summary>
		/// <param name="x">Absolute horizontal position</param>
		/// <param name="y">Absolute vertical position</param>
		public Position(double x, double y) : this()
		{
			Absolute.X = x;
			Absolute.Y = y;
		}

		/// <summary>
		/// Constructor with absolute position and shift parameters
		/// </summary>
		/// <param name="x">Absolute horizontal position</param>
		/// <param name="y">Absolute vertical position</param>
		/// <param name="xShift">Horizontal shift</param>
		/// <param name="yShift">Vertical shift</param>
		public Position(double x, double y, double xShift, double yShift) : this(x,y)
		{
			Shift.X = xShift;
			Shift.Y = yShift;
		}

		#endregion

		#region InternalStateChanged Event

		/// <summary>
		/// Event fired when the internal state of this Position changes
		/// </summary>
		public InternalStateChangedEventHandler InternalStateChanged;

		/// <summary>
		/// Invokes the <see cref="InternalStateChanged"/> event
		/// </summary>
		public void InvokeInternalStateChanged() => InternalStateChanged?.Invoke(this);

		#endregion

		#region Private Members

		/// <summary>
		/// The absolute coord of this position
		/// </summary>
		private Coord mAbsolute = new Coord();

		/// <summary>
		/// Shift applied to this position
		/// </summary>
		private Coord mShift = new Coord();

		#endregion

		#region Public Properties

		/// <summary>
		/// The absolute coord of this position
		/// </summary>
		public Coord Absolute
		{
			get => mAbsolute;
			set
			{
				// Check if the assigned value is different than the current one
				if(mAbsolute==value)
				{
					// If so, return
					return;
				}

				// If the old value isn't null
				if(mAbsolute!=null)
				{
					// Unsubscribe from its internal state changed
					mAbsolute.InternalStateChanged -= PropagateInternalStateChanged;
				}

				mAbsolute = value;

				// If the new value isn't null
				if(mAbsolute!=null)
				{
					// Subscribe to its internal state changed
					mAbsolute.InternalStateChanged += PropagateInternalStateChanged;
				}

				InvokeInternalStateChanged();
			}
		}

		/// <summary>
		/// The shift applied to this position
		/// </summary>
		public Coord Shift
		{
			get => mShift;
			set
			{
				// Check if the assigned value is different than the current one
				if(mShift==value)
				{
					// If so, return
					return;
				}

				// If the old value isn't null
				if (mShift != null)
				{
					// Unsubscribe from its internal state changed
					mShift.InternalStateChanged -= PropagateInternalStateChanged;
				}

				mShift = value;

				// If the new value isn't null
				if (mShift != null)
				{
					// Subscribe to its internal state changed
					mShift.InternalStateChanged += PropagateInternalStateChanged;
				}

				InvokeInternalStateChanged();
			}
		}
		
		/// <summary>
		/// Final X coordinate of this position (with applied shift)
		/// </summary>
		public double X => (Absolute == null ? 0 : Absolute.X) + (Shift == null ? 0 : Shift.X);

		/// <summary>
		/// Final Y coordinate of this position (with applied shift)
		/// </summary>
		public double Y => (Absolute == null ? 0 : Absolute.Y) + (Shift == null ? 0 : Shift.Y);

		#endregion

		#region Private Methods

		/// <summary>
		/// Fires <see cref="InternalStateChanged"/> event whenever absolute position or shift changes
		/// </summary>
		/// <param name="sender"></param>
		private void PropagateInternalStateChanged(object sender) => InvokeInternalStateChanged();

		#endregion
	}
}
