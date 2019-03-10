using CSharpEnhanced.Maths;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	public abstract class MultiInputGate : BasePart
	{
		#region Constructor

		/// <summary>
		/// DefaultConstructor
		/// </summary>
		public MultiInputGate(string displayName, MultiInputGateType type) : base(displayName)
		{
			mGateType = type;
			Width = 125;
			Height = 75;
			Inputs.CollectionChanged += NumberOfInputsChanged;
			NumberOfInputsChanged(null, null);
		}

		#endregion

		#region Number of inputs changed

		/// <summary>
		/// Manages internal state event subscriptions and calculates the height
		/// of this gate
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NumberOfInputsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			foreach (var item in Inputs)
			{
				item.InternalStateChanged -= InputChanged;
				item.InternalStateChanged += InputChanged;
			}

			// The height is is 50+25*(number of inputs rounded down to even value)
			if (Inputs.Count % 2 == 1)
			{
				Height = 50 + 25 * Inputs.Count - 25;
			}
			else
			{
				Height = 50 + 25 * Inputs.Count;
			}

			OnPropertyChanged(nameof(OutputXCoord));
			OnPropertyChanged(nameof(PartBodyBinding));
			ComputeSocketCoords();			
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Method to call when one of inputs changes
		/// </summary>
		/// <param name="sender"></param>
		protected abstract void InputChanged(object sender);

		#endregion

		#region Private Members

		/// <summary>
		/// Type of this gate, should be assigned in the constructor
		/// </summary>
		private readonly MultiInputGateType mGateType;

		#endregion

		#region Public Properties

		/// <summary>
		/// Type of this gate
		/// </summary>
		public MultiInputGateType GateType => mGateType;

		/// <summary>
		/// Values which determine the shape of this part in the UI
		/// </summary>
		public Tuple<double, MultiInputGateType> PartBodyBinding => new Tuple<double, MultiInputGateType>(Height, GateType);

		/// <summary>
		/// When true, lengths of input wires should be adjusted to fit the curved back of the part
		/// </summary>
		public bool DifferentiateInputWireLengths => GateType == MultiInputGateType.OR || GateType == MultiInputGateType.NOR ||
			GateType == MultiInputGateType.XOR || GateType == MultiInputGateType.XNOR;

		/// <summary>
		/// Height for the output wire and socket
		/// </summary>
		public double OutputXCoord => Height / 2;		

		/// <summary>
		/// Collection with all InputSockets this gate has. It's created with
		/// 2 inputs by default.
		/// </summary>
		public ObservableCollection<InputSocket> Inputs { get; private set; } =
			new ObservableCollection<InputSocket>()
			{
				new InputSocket(),
				new InputSocket(),
			};

		/// <summary>
		/// Output of this AND gate
		/// </summary>
		public OutputSocket Output { get; private set; } = new OutputSocket();

		#endregion
		
		#region IDisposable

		public override void Dispose()
		{
			
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Computes the coords of each input socket and the output socket
		/// </summary>
		public override void ComputeSocketCoords()
		{
			Output.Position.Absolute = CenterCoord;
			Output.Position.Shift.Set(50, 0, RotationAngleClockWise, AngleUnit.Degrees);

			for(int i=0; i<Inputs.Count; ++i)
			{
				Inputs[i].Position.Absolute = CenterCoord;
				Inputs[i].Position.Shift.Set(-50,
					25 * (i - Inputs.Count/2 + (Inputs.Count % 2 == 0 && (i >= (Inputs.Count / 2)) ? 1 : 0)),
					RotationAngleClockWise, AngleUnit.Degrees);
			}
		}

		#endregion		
	}
}