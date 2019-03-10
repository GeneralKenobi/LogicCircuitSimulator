using LogicCircuitSimulator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace LogicCircuitSimulator
{
    public sealed class SocketTC : Control
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public SocketTC()
        {
            this.DefaultStyleKey = typeof(SocketTC);
        }

		#endregion

		#region Socket Dependency Property

		/// <summary>
		/// Socket to present using this control
		/// </summary>
		public Socket Socket
		{
			get => (Socket)GetValue(SocketProperty);
			set => SetValue(SocketProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Socket"/>
		/// </summary>
		public static readonly DependencyProperty SocketProperty =
			DependencyProperty.Register(nameof(Socket), typeof(Socket), typeof(SocketTC), new PropertyMetadata(null));

		#endregion

		#region Tapped handler

		/// <summary>
		/// When the control is tapped, execute the socket tapped command
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void SocketTCTapped(object sender, TappedRoutedEventArgs e)
		{
			Socket?.SocketClickedCommand.Execute(Socket);
			e.Handled = true;
		}

		#endregion
	}
}