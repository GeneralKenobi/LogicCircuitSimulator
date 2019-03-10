using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LogicCircuitSimulator
{
	/// <summary>
	/// Converts true to <see cref="Visiblity.Visible"/> and false to <see cref="Visibility.Collapsed"/>
	/// If a parameter is passed (any parameter), inverses the conversion.
	/// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(parameter==null)
			{
				return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
			}
			else
			{
				return ((bool)value) ? Visibility.Collapsed : Visibility.Visible;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
