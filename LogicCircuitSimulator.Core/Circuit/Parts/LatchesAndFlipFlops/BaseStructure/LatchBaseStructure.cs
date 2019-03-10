using CSharpEnhanced.Maths;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public abstract class LatchBaseStructure : BasePart
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="name"></param>
		public LatchBaseStructure(string name, ClockSignalType clockType) : base(name)
		{
			mQComplement = new ComplementedOutputSocket(mQ);

			mClockType = clockType;

			// Subscribe to asynchronous input changes
			Set.InternalStateChanged += AsynchronousInputChanged;
			Reset.InternalStateChanged += AsynchronousInputChanged;

			Width = 160;
			Height = 210;
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Type of clock in this latch
		/// </summary>
		private readonly ClockSignalType mClockType;

		/// <summary>
		/// Clock input of this latch
		/// </summary>
		private readonly InputSocket mClock = new InputSocket();

		/// <summary>
		/// Asynchronous set input of this latch
		/// </summary>
		private readonly InputSocket mSet = new InputSocket();

		/// <summary>
		/// Asynchronous reset input of this latch
		/// </summary>
		private readonly InputSocket mReset = new InputSocket();

		/// <summary>
		/// Q - output of this latch
		/// </summary>
		private readonly OutputSocket mQ = new OutputSocket();

		/// <summary>
		/// Complemented Q - negated output of this latch
		/// </summary>
		private readonly ComplementedOutputSocket mQComplement;

		/// <summary>
		/// When true <see cref="Set"/> is negated
		/// </summary>
		private bool mNegateSet = false;

		/// <summary>
		/// When true <see cref="Reset"/> is negated
		/// </summary>
		private bool mNegateReset = false;

		/// <summary>
		/// When both <see cref="Set"/> and <see cref="Reset"/> are positive, output will be set to 1 if this bool is true
		/// </summary>
		private bool mDominantSet = true;

		#endregion

		#region Protected Properties

		/// <summary>
		/// Value of set socket adjusted to <see cref="NegateSet"/> setting
		/// </summary>
		protected bool AdjustedSet => Set.Value ^ NegateSet;

		/// <summary>
		/// Value of reset socket adjusted to <see cref="NegateReset"/> setting
		/// </summary>
		protected bool AdjustedReset => Reset.Value ^ NegateReset;

		/// <summary>
		/// Value of clock adjusted to <see cref="NegateClock"/> setting
		/// </summary>
		protected bool AdjustedClock => Clock.Value ^ NegateClock;

		#endregion

		#region Public Properties

		#region Sockets

		/// <summary>
		/// Clock input of this latch
		/// </summary>
		public InputSocket Clock => mClock;

		/// <summary>
		/// Asynchronous set input of this latch
		/// </summary>
		public InputSocket Set => mSet;

		/// <summary>
		/// Asynchronous reset input of this latch
		/// </summary>
		public InputSocket Reset => mReset;

		/// <summary>
		/// Q - output of this latch
		/// </summary>
		public OutputSocket Q => mQ;

		/// <summary>
		/// Complemented Q - negated output of this latch
		/// </summary>
		public ComplementedOutputSocket QComplement => mQComplement;

		#endregion

		#region Labels

		/// <summary>
		/// Label to put near clock <see cref="Clock"/>
		/// </summary>
		public string ClockLabel => "Cl";

		/// <summary>
		/// Label to put near <see cref="Q"/> and <see cref="QComplement"/>
		/// </summary>
		public string OutputLabel => "Q";

		/// <summary>
		/// Label to put near <see cref="Set"/>
		/// </summary>
		public string SetLabel => "S";

		/// <summary>
		/// Label to put near <see cref="Reset"/>
		/// </summary>
		public string ResetLabel => "R";

		#endregion

		#region Settings

		/// <summary>
		/// When true <see cref="Clock"/> is negated
		/// </summary>
		public bool NegateClock { get; set; } = false;

		/// <summary>
		/// When true <see cref="Set"/> is negated
		/// </summary>
		public bool NegateSet
		{
			get => mNegateSet;
			set
			{
				if (mNegateSet != value)
				{
					mNegateSet = value;
					OnPropertyChanged(nameof(NegateSet));

					// Recalculate the output value based on the new setting
					SetQ(Q.Value);
				}
			}
		}

		/// <summary>
		/// When true <see cref="Reset"/> is negated
		/// </summary>
		public bool NegateReset
		{
			get => mNegateReset;
			set
			{
				if (mNegateReset != value)
				{
					mNegateReset = value;
					OnPropertyChanged(nameof(NegateReset));

					// Recalculate the output value based on the new setting
					SetQ(Q.Value);
				}
			}
		}

		/// <summary>
		/// When both <see cref="Set"/> and <see cref="Reset"/> are positive, output will be set to 1 if this bool is true
		/// </summary>
		public bool DominantSet
		{
			get => mDominantSet;
			set
			{
				if (mDominantSet != value)
				{
					mDominantSet = value;
					OnPropertyChanged(nameof(DominantSet));

					// Recalculate the output value based on the new setting
					SetQ(Q.Value);
				}
			}
		}

		/// <summary>
		/// Type of clock in this latch
		/// </summary>
		public ClockSignalType ClockType => mClockType;

		#endregion

		#endregion

		#region Private Methods

		/// <summary>
		/// Method to call when asynchronous input changes
		/// </summary>
		/// <param name="sender"></param>
		private void AsynchronousInputChanged(object sender) => SetQ(Q.Value);

		#endregion

		#region Protected Methods		

		/// <summary>
		/// Sets the Q value and adjusts it for asynchronous inputs: <see cref="Set"/> and <see cref="Reset"/>
		/// </summary>
		/// <param name="value"></param>
		protected void SetQ(bool value) =>
			mQ.Value = value && !AdjustedReset || DominantSet && AdjustedSet || AdjustedSet && !AdjustedReset;

		#endregion

		#region Public Methods

		/// <summary>
		/// Computes coords for base sockets in a flip flop
		/// </summary>
		public override void ComputeSocketCoords()
		{
			Set.Position.Absolute = CenterCoord;
			Set.Position.Shift.Set(0, -100, RotationAngleClockWise, AngleUnit.Degrees);

			Reset.Position.Absolute = CenterCoord;
			Reset.Position.Shift.Set(0, 100, RotationAngleClockWise, AngleUnit.Degrees);

			Clock.Position.Absolute = CenterCoord;
			Clock.Position.Shift.Set(-75, 0, RotationAngleClockWise, AngleUnit.Degrees);

			Q.Position.Absolute = CenterCoord;
			Q.Position.Shift.Set(75, -50, RotationAngleClockWise, AngleUnit.Degrees);

			QComplement.Position.Absolute = CenterCoord;
			QComplement.Position.Shift.Set(75, 50, RotationAngleClockWise, AngleUnit.Degrees);
		}

		#endregion
	}

}
