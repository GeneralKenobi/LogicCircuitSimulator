using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace LogicCircuitSimulator
{

	/// <summary>
	/// Converts true to StaticResource GreenBrush and false to StaticResource DarkRedBrush
	/// </summary>
	public class StateToGreenRedValueConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is bool b)
			{
				return b ? App.Current.Resources["LightGreenBrush"] : App.Current.Resources["DarkRedBrush"];
			}
			else
			{
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
