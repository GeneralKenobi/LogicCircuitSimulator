using CSharpEnhanced.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Collection which contains all part types, segragated, with proper edit viewmodels assigned to them.
	/// It's a viewmodel for menu which allows to add parts
	/// </summary>
    public class PartTypeCollection : BaseViewModel
    {
		#region Constructor

		/// <summary>
		/// Private constructor to restrict this class to only a single instance
		/// </summary>
		private PartTypeCollection() { }

		#endregion

		#region Singleton

		/// <summary>
		/// Single instance of this class for the whole application
		/// </summary>
		public static PartTypeCollection Singleton = new PartTypeCollection();

		#endregion

		#region Private Members

		#region Type Collections

		/// <summary>
		/// Collection of all types (values) with their display names (keys)
		/// </summary>
		private readonly ObservablePriorityDictionary<string, Type> mAllParts = new ObservablePriorityDictionary<string, Type>()
		{
			{"Toggle Input",typeof(ToggleInput), 0},
			{"Button",typeof(ButtonInput), 1},
			{"Clock",typeof(ClockInput), 2},
			{"Inverter",typeof(NOTGate), 3},
			{"OR Gate",typeof(ORGate), 4},
			{"NOR Gate",typeof(NORGate), 5},
			{"AND Gate",typeof(ANDGate), 6},
			{"NAND Gate",typeof(NANDGate), 7},
			{"XOR Gate",typeof(XORGate), 8},
			{"XNOR Gate",typeof(XNORGate), 9},
			{"Multiplexer", typeof(MUX), 10 },
			{"Demultiplexer", typeof(DMUX), 11 },
			{"D Latch",typeof(DLatch), 12},
			{"D Flip-flop",typeof(DFlipFlop), 13},
			{"T Latch",typeof(TLatch), 14},
			{"T Flip-flop",typeof(TFlipFlop), 15},
			{"JK Latch",typeof(JKLatch), 16},
			{"JK Flip-flop",typeof(JKFlipFlop), 17},
		};

		/// <summary>
		/// Collection of all input types (values) with their display names (keys)
		/// </summary>
		private readonly ObservablePriorityDictionary<string, Type> mInput = new ObservablePriorityDictionary<string, Type>()
		{
			{"Toggle Input",typeof(ToggleInput), 0},
			{"Button",typeof(ButtonInput), 1},
			{"Clock",typeof(ClockInput), 2},			
		};

		/// <summary>
		/// Collection of all simple logic types (values) with their display names (keys)
		/// </summary>
		private readonly ObservablePriorityDictionary<string, Type> mSimpleLogic = new ObservablePriorityDictionary<string, Type>()
		{			
			{"Inverter",typeof(NOTGate), 0},
			{"OR Gate",typeof(ORGate), 1},
			{"NOR Gate",typeof(NORGate), 2},
			{"AND Gate",typeof(ANDGate), 3},
			{"NAND Gate",typeof(NANDGate), 4},
			{"XOR Gate",typeof(XORGate), 5},
			{"XNOR Gate",typeof(XNORGate), 6},			
		};

		/// <summary>
		/// Collection of all complex logic types (values) with their display names (keys)
		/// </summary>
		private readonly ObservablePriorityDictionary<string, Type> mComplexLogic = new ObservablePriorityDictionary<string, Type>()
		{			
			{"Multiplexer", typeof(MUX), 0 },
			{"Demultiplexer", typeof(DMUX), 1 },
			{"D Latch",typeof(DLatch), 2},
			{"D Flip-flop",typeof(DFlipFlop), 3},
			{"T Latch",typeof(TLatch), 4},
			{"T Flip-flop",typeof(TFlipFlop), 5},
			{"JK Latch",typeof(JKLatch), 6},
			{"JK Flip-flop",typeof(JKFlipFlop), 7},
		};

		#endregion

		#region View model bindings

		/// <summary>
		/// When true, list containing input parts should be expanded
		/// </summary>
		private bool mExpandInputList = false;

		/// <summary>
		/// When true, list containing simple logic parts should be expanded
		/// </summary>
		private bool mExpandSimpleLogicList = false;

		/// <summary>
		/// When true, list containing complex logic parts should be expanded
		/// </summary>
		private bool mExpandComplexLogicList = false;

		#endregion

		#endregion

		#region Public Properties

		#region Type Collections

		/// <summary>
		/// Collection of all types (values) with their display names (keys)
		/// </summary>
		public ObservablePriorityDictionary<string, Type> AllParts => mAllParts;

		/// <summary>
		/// Collection of all input types (values) with their display names (keys)
		/// </summary>
		public ObservablePriorityDictionary<string, Type> Input => mInput;

		/// <summary>
		/// Collection of all simple logic types (values) with their display names (keys)
		/// </summary>
		public ObservablePriorityDictionary<string, Type> SimpleLogic => mSimpleLogic;

		/// <summary>
		/// Collection of all complex logic types (values) with their display names (keys)
		/// </summary>
		public ObservablePriorityDictionary<string, Type> ComplexLogic => mComplexLogic;

		#endregion

		#region Modifiable properties

		/// <summary>
		/// When true, list containing input parts should be expanded
		/// </summary>
		public bool ExpandInputList
		{
			get => mExpandInputList;
			set
			{
				if(value)
				{
					// If this list is expanding, collapse others
					mExpandSimpleLogicList = false;
					mExpandComplexLogicList = false;
					OnPropertyChanged(nameof(ExpandSimpleLogicList), nameof(ExpandComplexLogicList));
				}

				mExpandInputList = value;
			}
		}

		/// <summary>
		/// When true, list containing simple logic parts should be expanded
		/// </summary>
		public bool ExpandSimpleLogicList
		{
			get => mExpandSimpleLogicList;
			set
			{
				if (value)
				{
					// If this list is expanding, collapse others
					mExpandInputList = false;
					mExpandComplexLogicList = false;
					OnPropertyChanged(nameof(ExpandInputList), nameof(ExpandComplexLogicList));
				}

				mExpandSimpleLogicList = value;
			}
		}

		/// <summary>
		/// When true, list containing complex logic parts should be expanded
		/// </summary>
		public bool ExpandComplexLogicList
		{
			get => mExpandComplexLogicList;
			set
			{
				if (value)
				{
					// If this list is expanding, collapse others
					mExpandInputList = false;
					mExpandSimpleLogicList = false;
					OnPropertyChanged(nameof(ExpandInputList), nameof(ExpandSimpleLogicList));
				}

				mExpandComplexLogicList = value;
			}
		}

		#endregion

		#region Texts

		/// <summary>
		/// Header for the part adding section
		/// </summary>
		public string Header => "Add Parts";

		/// <summary>
		/// Header for the list with inputs
		/// </summary>
		public string InputListHeader => "Inputs";

		/// <summary>
		/// Header for the list with simple logics
		/// </summary>
		public string SimpleLogicListHeader => "Simple logic";

		/// <summary>
		/// Header for the list with complex logics
		/// </summary>
		public string ComplexLogicListHeader => "Complex logic";

		#endregion

		#endregion
	}
}
