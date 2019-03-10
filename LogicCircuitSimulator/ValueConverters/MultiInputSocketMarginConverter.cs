using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LogicCircuitSimulator
{
	public class MultiInputSocketMarginConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is int count && parameter is int index && index * 2 == count)
			{
				return new Thickness(0, 25, 0, 15);
			}
			return new Thickness(0, 0, 0, 15);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
