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
    public sealed class SocketWithWireTC : Control
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public SocketWithWireTC()
        {
            this.DefaultStyleKey = typeof(SocketWithWireTC);
        }

		#endregion

		#region Add Negation Dependency Property

		/// <summary>
		/// If true, negation will be added at the end of the wire
		/// </summary>
		public bool AddNegation
		{
			get => (bool)GetValue(AddNegationProperty);
			set => SetValue(AddNegationProperty, value);
		}

		// Using a DependencyProperty as the backing store for AddNegation.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty AddNegationProperty =
			DependencyProperty.Register(nameof(AddNegation), typeof(bool),
				typeof(SocketWithWireTC), new PropertyMetadata(false));

		private static void AddNegationChanged(DependencyObject s, DependencyPropertyChangedEventArgs args)
		{
			if (s is SocketWithWireControl c)
			{
				//c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs(nameof(AddNegation)));
			}
		}

		#endregion

		#region Socket Dependency Property

		/// <summary>
		/// Socket to present
		/// </summary>
		public Socket Socket
		{
			get => (Socket)GetValue(SocketProperty);
			set => SetValue(SocketProperty, value);
		}

		// Using a DependencyProperty as the backing store for Socket.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SocketProperty =
			DependencyProperty.Register(nameof(Socket), typeof(Socket), typeof(SocketWithWireTC),
				new PropertyMetadata(null));

		#endregion

		#region Socket Position Dependency Property

		/// <summary>
		/// Position (face direction) of the socket
		/// </summary>
		public UIPosition SocketPosition
		{
			get => (UIPosition)GetValue(SocketPositionProperty);
			set => SetValue(SocketPositionProperty, value);
		}

		// Using a DependencyProperty as the backing store for SocketPosition.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SocketPositionProperty =
			DependencyProperty.Register(nameof(SocketPosition), typeof(UIPosition),
				typeof(SocketWithWireTC), new PropertyMetadata(UIPosition.Left, new PropertyChangedCallback(SocketPositionChanged)));

		private static void SocketPositionChanged(DependencyObject s, DependencyPropertyChangedEventArgs args)
		{
			if (s is SocketWithWireControl c)
			{
				//c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("SocketPosition"));
				//c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("WireHeight"));
				//c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("WireWidth"));
				//c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("WireBorderThickness"));
				//c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("SocketRow"));
				//c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("SocketColumn"));
				//c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("NegatgionRow"));
				//c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("NegationColumn"));
				//c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("WireMargin"));
			}
		}

		#endregion
	}
}
