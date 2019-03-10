using LogicCircuitSimulator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace LogicCircuitSimulator
{
	/// <summary>
	/// Converts a <see cref="ICollection"/> to a Data for a Path
	/// </summary>
	public class CoordsToPathDataConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is ObservableCollection<Position> collection && collection.Count > 0)
			{
				string data = string.Empty;

				var enumerator = collection.GetEnumerator();

				// Advance to the first element
				enumerator.MoveNext();

				// Mark the starting point
				data += "M" + enumerator.Current.X.ToString() + "," +
					 enumerator.Current.Y.ToString();

				// Then mark each subsequent point
				while (enumerator.MoveNext())
				{
					data += " V" + enumerator.Current.Y.ToString() + " H" +
					 enumerator.Current.X.ToString();
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
	}
}
