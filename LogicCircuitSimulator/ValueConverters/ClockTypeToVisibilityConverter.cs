using LogicCircuitSimulator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LogicCircuitSimulator
{
    public class ClockTypeToVisibilityConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is ClockSignalType type && type == ClockSignalType.EdgeDriven)
			{
				return Visibility.Visible;
			}
			else
			{
				return Visibility.Collapsed;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
