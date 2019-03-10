using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace LogicCircuitSimulator
{
	/// <summary>
	/// Converts true to static resuource LightGreenBrush, otherwise
	/// to static resource BlackBrush
	/// </summary>
	public class FalsePositiveBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language) =>
			value is bool b && b ?
			App.Current.Resources["LightGreenBrush"] : App.Current.Resources["BlackBrush"];
		

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
