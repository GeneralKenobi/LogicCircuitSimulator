using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace LogicCircuitSimulator
{
	/// <summary>
	/// Converts any FocusState except of Unfocussed to DarkRedBrush, the rest
	/// to transparent brush
	/// </summary>
	public class FocusStateToBorderBrushConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language) =>
			(value is FocusState state && state != FocusState.Unfocused) ?
			App.Current.Resources["DarkRedBrush"] : new SolidColorBrush(Colors.Transparent);
		

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
