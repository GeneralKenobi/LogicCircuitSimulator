using LogicCircuitSimulator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace LogicCircuitSimulator
{
	/// <summary>
	/// Converts a <see cref="ICollection"/> to a Data for a Path
	/// </summary>
	public class CoordsToWireBorderPathDataConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is ICollection<Position> collection && collection.Count > 0)
			{
				var enumerator = collection.GetEnumerator();

				// Advance to the first element
				enumerator.MoveNext();

				// Extremal points of the wire
				double xMin = enumerator.Current.X;
				double xMax = enumerator.Current.X;
				double yMin = enumerator.Current.Y;
				double yMax = enumerator.Current.Y;

				while (enumerator.MoveNext())
				{
					if (enumerator.Current.X < xMin)
						xMin = enumerator.Current.X;

					if (enumerator.Current.X > xMax)
						xMax = enumerator.Current.X;

					if (enumerator.Current.Y < yMin)
						yMin = enumerator.Current.Y;

					if (enumerator.Current.Y > yMax)
						yMax = enumerator.Current.Y;
				}

				if (xMin >= 10)
					xMin -= 10;

				if (yMin >= 10)
					yMin -= 10;

				xMax += 10;
				yMax += 10;
				string data = "M" + xMin.ToString() + "," + yMax.ToString() +
					"H" + xMax.ToString() + "V" + yMin.ToString() +
					"H" + xMin.ToString() + "V" + yMax.ToString();			

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
