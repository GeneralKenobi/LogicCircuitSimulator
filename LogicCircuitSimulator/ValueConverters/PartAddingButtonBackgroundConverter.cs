using System;
using Windows.UI.Xaml.Data;

namespace LogicCircuitSimulator
{
	/// <summary>
	/// Converter for part adding buttons backgrounds'. Returns {StaticResource GrayBrush}
	/// if string value == string parameter, LightGrayBrush otherwise
	/// </summary>
	public class PartAddingButtonBackgroundConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language) =>
			value is string v && parameter is string p && v == p ?
			App.Current.Resources["WhiteBrush20"] : App.Current.Resources["DarkBlue2Brush"];

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
