using LogicCircuitSimulator.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class SocketWithWireControl : UserControl, INotifyPropertyChanged
    {
		#region Constructor

		public SocketWithWireControl()
        {
            this.InitializeComponent();
        }

		#endregion

		#region Property Changed Event

		public event PropertyChangedEventHandler PropertyChanged;

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
				typeof(SocketWithWireControl), new PropertyMetadata(false));

		private static void AddNegationChanged(DependencyObject s, DependencyPropertyChangedEventArgs args)
		{
			if(s is SocketWithWireControl c)
			{
				c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs(nameof(AddNegation)));
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
			DependencyProperty.Register(nameof(Socket), typeof(Socket), typeof(SocketWithWireControl),
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
				typeof(SocketWithWireControl), new PropertyMetadata(UIPosition.Left, new PropertyChangedCallback(SocketPositionChanged)));

		private static void SocketPositionChanged(DependencyObject s, DependencyPropertyChangedEventArgs args)
		{
			if(s is SocketWithWireControl c)
			{
				c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("SocketPosition"));
				c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("WireHeight"));
				c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("WireWidth"));
				c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("WireBorderThickness"));
				c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("SocketRow"));
				c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("SocketColumn"));
				c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("NegatgionRow"));
				c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("NegationColumn"));
				c.PropertyChanged?.Invoke(c, new PropertyChangedEventArgs("WireMargin"));
			}
		}

		#endregion

		#region Public Properties

		public bool Horizontal => SocketPosition == UIPosition.Left || SocketPosition == UIPosition.Right;

		/// <summary>
		/// Height of the wire border, depends on the position
		/// </summary>
		public double WireHeight => Horizontal ? 5 : Double.NaN;

		/// <summary>
		/// Width of the wire border, depends on the position
		/// </summary>
		public double WireWidth => Horizontal ? Double.NaN : 5;

		/// <summary>
		/// Border thickness of the wire
		/// </summary>
		public Thickness WireBorderThickness
		{
			get
			{
				switch(SocketPosition)
				{
					case UIPosition.Left:
						{
							return new Thickness(1, 1, 0, 1);
						}

					case UIPosition.Top:
						{
							return new Thickness(1, 1, 1, 0);
						}

					case UIPosition.Right:
						{
							return new Thickness(0, 1, 1, 1);
						}

					case UIPosition.Bottom:
						{
							return new Thickness(1, 0, 1, 1);
						}

					default:
						{
							return new Thickness(1);
						}
				}
			}
		}

		/// <summary>
		/// Row for the socket 
		/// </summary>
		public int SocketRow
		{
			get
			{
				switch(SocketPosition)
				{
					case UIPosition.Left:
					case UIPosition.Right:
						{
							return 1;
						}

					case UIPosition.Bottom:
						{
							return 2;
						}

					case UIPosition.Top:
						{
							return 0;
						}

					default:
						{
							return 1;
						}
				}
			}
		}

		/// <summary>
		/// Row for the negation circle 
		/// </summary>
		public int NegationRow
		{
			get
			{
				switch (SocketPosition)
				{
					case UIPosition.Left:
					case UIPosition.Right:
						{
							return 1;
						}

					case UIPosition.Bottom:
						{
							return 0;
						}

					case UIPosition.Top:
						{
							return 2;
						}

					default:
						{
							return 1;
						}
				}
			}
		}

		/// <summary>
		/// Column for the socket 
		/// </summary>
		public int SocketColumn
		{
			get
			{
				switch (SocketPosition)
				{
					case UIPosition.Top:
					case UIPosition.Bottom:
						{
							return 1;
						}

					case UIPosition.Left:
						{
							return 0;
						}

					case UIPosition.Right:
						{
							return 2;
						}

					default:
						{
							return 1;
						}
				}
			}
		}

		/// <summary>
		/// Row for the negation circle 
		/// </summary>
		public int NegationColumn
		{
			get
			{
				switch (SocketPosition)
				{
					case UIPosition.Top:
					case UIPosition.Bottom:
						{
							return 1;
						}

					case UIPosition.Left:
						{
							return 2;
						}

					case UIPosition.Right:
						{
							return 0;
						}

					default:
						{
							return 1;
						}
				}
			}
		}

		public Thickness WireMargin => Horizontal ? new Thickness(-2, 0, -2, 0) : new Thickness(0, -2, 0, -2);

		#endregion
	}
}
