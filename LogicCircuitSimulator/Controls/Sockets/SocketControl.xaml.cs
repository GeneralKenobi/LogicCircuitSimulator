using LogicCircuitSimulator.Core;
using Windows.Devices.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LogicCircuitSimulator
{
	public sealed partial class SocketControl : UserControl
	{
		#region Constructor

		public SocketControl()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Tapped

		private void SocketTapped(object sender, TappedRoutedEventArgs e)
		{
			// Execute the command
			(DataContext as Socket)?.SocketClickedCommand.Execute(DataContext);
			e.Handled = true;
		}

		#endregion
	}
}
