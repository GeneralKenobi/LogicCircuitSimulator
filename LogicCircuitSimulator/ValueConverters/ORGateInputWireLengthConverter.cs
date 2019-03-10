using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace LogicCircuitSimulator
{
	/// <summary>
	/// Converter for wires from input sockets which returns the length for
	/// a proper wire (the closer the wire is to the center the longer it has
	/// to be due to the curvature). Value should be the number of elements, parameter
	/// should be the position of the wire (from top)
	/// </summary>
	public class ORGateInputWireLengthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is int numberOfElements && parameter is int position)
			{
				// Value for a single input part
				if (numberOfElements == 1)
				{
					return 22;
				}

				// Consider first and last position as second from end
				if(position == 0)
				{
					++position;
				}

				// Map positions in the second half to the first half (the value is designed to work for the first half,
				// second half is symmetrical
				if(position > (numberOfElements / 2))
				{
					position = numberOfElements - position;
				}
				
				double result = 17 + 5 * position / (numberOfElements / 2);

				// Return either the result or 0 if the result is negative for some reason
				return result >= 0 ? result : 0;
			}

			return 20;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
