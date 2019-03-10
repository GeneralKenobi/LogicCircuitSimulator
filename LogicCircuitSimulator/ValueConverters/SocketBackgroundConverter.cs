using LogicCircuitSimulator.Core;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace LogicCircuitSimulator
{
	/// <summary>
	/// Converts <see cref="SocketState"/> to a proper brush
	/// </summary>
	public class SocketBackgroundConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is SocketState state)
			{
				switch(state)
				{
					case SocketState.StandardOff:
						{
							return App.Current.Resources["BlackBrush"];
						}

					case SocketState.StandardOn:
						{
							return App.Current.Resources["LightGreenBrush"];
						}

					case SocketState.Highlighted:
						{
							return App.Current.Resources["BlueBrush"];
						}

					case SocketState.NotConnected:
						{
							return App.Current.Resources["DarkRedBrush"];
						}

					default:
						{
							throw new Exception("Unrecognized enum value");
						}
				}
			}
			else
			{
				return App.Current.Resources["BlackBrush"];
			}
		}
			

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
