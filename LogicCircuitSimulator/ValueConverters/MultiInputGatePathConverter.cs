using LogicCircuitSimulator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace LogicCircuitSimulator
{
	/// <summary>
	/// Converts a <see cref="ICollection"/> to a Data for a Path
	/// </summary>
	public class MutliInputGatePathConverter : IValueConverter
	{
		#region IValueConverter implementation

		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is Tuple<double, MultiInputGateType> castedValue)
			{
				string data = string.Empty;

				// Get the appropriate path
				switch(castedValue.Item2)
				{
					case MultiInputGateType.OR:
						{
							data = ORGatePath(castedValue.Item1);
						}
						break;

					case MultiInputGateType.NOR:
						{
							data = NORGatePath(castedValue.Item1);
						}
						break;

					case MultiInputGateType.AND:
						{
							data = ANDGatePath(castedValue.Item1);
						}
						break;

					case MultiInputGateType.NAND:
						{
							data = NANDGatePath(castedValue.Item1);
						}
						break;

					case MultiInputGateType.XOR:
						{
							data = XORGatePath(castedValue.Item1);
						}
						break;

					case MultiInputGateType.XNOR:
						{
							data = XNORGatePath(castedValue.Item1);
						}
						break;

					default:
						{
							return null;
						}
				}				

				var geometry = (Geometry)XamlReader.Load(
					"<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"
				+ data + "</Geometry>");

				return geometry;
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Private path drawing helpers

		/// <summary>
		/// Returns a path for a circle
		/// </summary>
		private string NegationCircle => "c0,0 0,5 5,5 c0,0 5,0 5,-5 c0,0 0,-5 -5,-5 c0,0 -5,0 -5,5";

		/// <summary>
		/// Creates a string path for an OR gate with the given height
		/// </summary>
		/// <param name="height"></param>
		/// <returns></returns>
		private string ORGatePath(double height)
		{
			// Starting point
			string data = "M25,0 ";

			// The back curve
			data += $"c0,0 17,{ height / 2} 0.5,{height}";

			// Bottom front curve
			data += $"c0,0 45,0 75,{-(height / 2 + 1)}";

			// Top front curve
			data += $"c0,0 -30,{-(height / 2)} -76,{-(height / 2 + 0.5)}";

			// The back curve again (to smooth edges out)
			data += $"c0,0 17,{height / 2} 1.5,{height}";

			return data;
		}


		/// <summary>
		/// Creates a string path for a NOR gate with the given height
		/// </summary>
		/// <param name="height"></param>
		/// <returns></returns>
		private string NORGatePath(double height)
		{
			// Starting point
			string data = "M25,1 ";

			// The back curve
			data += $"c0,0 17,{ height / 2} 0.5,{height}";

			// Bottom front curve
			data += $"c0,0 40,0 60,{-(height / 2 + 1)}";

			// Top front curve
			data += $"c0,0 -20,{-(height / 2)} -61,{-(height / 2 + 0.5)}";

			// The back curve again (to smooth edges out)
			data += $"c0,0 17,{height / 2} 1.5,{height}";

			data += $"M90,{height / 2}"+NegationCircle;

			return data;
		}
			

		/// <summary>
		/// Creates a string path for an AND gate with the given height
		/// </summary>
		/// <param name="height"></param>
		/// <returns></returns>
		private string ANDGatePath(double height)
		{
			// Starting point
			string data = "M25,0 ";

			// The back curve
			data += $"v{height}";

			// Front bottom curve
			data += $"h15 c0,0 60,0 60,{-(height / 2)}";

			// Front upper curve
			data += $"c0,0 0,{-height/2} -60,{-height / 2} h-15";

			// The back curve again (to smooth edges out)
			data += $"v{height}";

			return data;
		}


		/// <summary>
		/// Creates a string path for a NAND gate with the given height
		/// </summary>
		/// <param name="height"></param>
		/// <returns></returns>
		private string NANDGatePath(double height)
		{
			// Starting point
			string data = "M25,0 ";

			// The back curve
			data += $"v{height}";

			// Front bottom curve
			data += $"h10 c0,0 50,0 50,{-(height / 2)}";

			// Front upper curve
			data += $"c0,0 0,{-height / 2} -50,{-height / 2} h-10";

			// The back curve again (to smooth edges out)
			data += $"v{height}";

			data += $"M90,{height / 2}"+NegationCircle;

			return data;
		}


		/// <summary>
		/// Creates a string path for an XOR gate with the given height
		/// </summary>
		/// <param name="height"></param>
		/// <returns></returns>
		private string XORGatePath(double height)
		{
			// Starting point
			string data = "M25,0 ";

			// The back curve
			data += $"c0,0 17,{ height / 2} 0.5,{height}";

			// Bottom front curve
			data += $"c0,0 45,0 75,{-(height / 2 + 1)}";

			// Top front curve
			data += $"c0,0 -30,{-(height / 2)} -76,{-(height / 2 + 0.5)}";

			// The back curve again (to smooth edges out)
			data += $"c0,0 17,{height / 2} 1.5,{height}";

			data += $"M17,0 c0,0 17,{height / 2} 0,{(height)}";
			return data;
		}


		/// <summary>
		/// Creates a string path for an XNOR gate with the given height
		/// </summary>
		/// <param name="height"></param>
		/// <returns></returns>
		private string XNORGatePath(double height)
		{
			// Starting point
			string data = "M25,1 ";

			// The back curve
			data += $"c0,0 17,{ height / 2} 0.5,{height}";

			// Bottom front curve
			data += $"c0,0 40,0 60,{-(height / 2 + 1)}";

			// Top front curve
			data += $"c0,0 -20,{-(height / 2)} -61,{-(height / 2 + 0.5)}";

			// The back curve again (to smooth edges out)
			data += $"c0,0 17,{height / 2} 1.5,{height}";

			data += $"M90,{height / 2}" + NegationCircle;

			data += $"M17,0 c0,0 17,{height / 2} 0,{(height)}";
			return data;
		}

		#endregion

	}
}
