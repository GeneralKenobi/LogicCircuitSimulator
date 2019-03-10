using LogicCircuitSimulator.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace LogicCircuitSimulator
{
	/// <summary>
	/// Class containing helpers for controls
	/// </summary>
    public static class ControlsHelpers
    {
		/// <summary>
		/// Attempts to identify the part and create a proper (and configured)
		/// UserControl for it (element). Returns true on success
		/// </summary>
		/// <param name="part">Part to try to identify</param>
		/// <param name="element">Element which will be the result,
		/// in case of failure it's null</param>
		/// <returns></returns>
		public static bool TryGetUIControl(object part, out FrameworkElement element)
		{
			Dictionary<Type, int> typeDictionary = new Dictionary<Type, int>()
			{
				{typeof(ToggleInput), 1},
				{typeof(ClockInput), 2},
				{typeof(ANDGate), 3},
				{typeof(ORGate), 3},
				{typeof(NORGate), 3},
				{typeof(NANDGate), 3},
				{typeof(XORGate), 3},
				{typeof(XNORGate), 3},
				{typeof(NOTGate), 4},
				{typeof(ButtonInput), 5},
				{typeof(DLatch), 6},
				{typeof(DFlipFlop), 6},
				{typeof(TLatch), 6},
				{typeof(TFlipFlop), 6},
				{typeof(JKLatch), 6},
				{typeof(JKFlipFlop), 6},
				{typeof(MUX), 7},
				{typeof(DMUX), 7},
			};

			switch(typeDictionary[part.GetType()])
			{
				// Wire
				case 0:
					{
						element = new WireControl();
					}
					break;

				// ToggleInput
				case 1:
					{
						element = new ToggleInputControl();
					}
					break;
					
				// ClockInput
				case 2:
					{
						element = new ClockInputControl();
					}
					break;

				case 4:
					{
						element = new NOTGateControl();
					}
					break;

				case 3:			
					{
						element = new MultiInputGateControl();
					}
					break;

				case 5:
					{
						element = new ButtonInputControl();
					}
					break;

				case 6:
					{
						element = new LatchControl();
					}
					break;

				case 7:
					{
						element = new MUXControl();
					}
					break;

				default:
					{
						// Unknown type found
						if(Debugger.IsAttached)
						{
							Debugger.Break();
						}

						element = null;
						return false;
					}
			}

			element.DataContext = part;
			element.Tag = ((BasePart)part).ID;
			return true;
		}
    }
}
