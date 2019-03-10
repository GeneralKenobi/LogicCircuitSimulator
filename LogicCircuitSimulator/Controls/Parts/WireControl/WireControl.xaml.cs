using LogicCircuitSimulator.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LogicCircuitSimulator
{
	public sealed partial class WireControl : UserControl, INotifyPropertyChanged
	{
		#region Property Changed Event

		public event PropertyChangedEventHandler PropertyChanged;
		
		#endregion

		#region Constructor

		public WireControl()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Menu Opening/Closing

		/// <summary>
		/// When true, the border around the part is shown
		/// </summary>
		public bool ShowBorder { get; set; }

		/// <summary>
		/// SolidColorBrush to bind the BorderBrush to
		/// </summary>
		public SolidColorBrush MenuPresentBorderBrush => ShowBorder ?
			App.Current.Resources["RedBrush"] as SolidColorBrush :
			new SolidColorBrush(Colors.Transparent);

		/// <summary>
		/// Toggles the <see cref="ShowBorder"/> flag
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuFlyoutOpenClose(object sender, object e) =>
			ShowBorder = !ShowBorder;

		#endregion

		#region Tapped

		private void WireTapped(object sender, TappedRoutedEventArgs e)
		{
			if (DataContext is Wire wire)
			{
				e.Handled = true;

				var wireRelatedCoords = e.GetPosition(sender as UIElement);				
				
				wire.WireClickCommand.Execute(new Tuple<Wire, Position>(wire,
					new Position(wireRelatedCoords.X, wireRelatedCoords.Y)));
			}
		}

		#endregion
	}
}
