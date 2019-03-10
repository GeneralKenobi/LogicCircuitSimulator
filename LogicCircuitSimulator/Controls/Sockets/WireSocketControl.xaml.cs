using CSharpEnhanced.Maths;
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
	public sealed partial class WireSocketControl : UserControl, INotifyPropertyChanged
	{
		#region Property Changed Event

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(params string[] propertyNames)
		{
			foreach(var item in propertyNames)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(item));
			}
		}

		#endregion

		#region Constructor

		public WireSocketControl()
		{
			this.InitializeComponent();			
		}		

		#endregion

		#region Tapped

		private void SocketTapped(object sender, TappedRoutedEventArgs e)
		{
			if (DataContext is WireSocket s)
			{
				s.SocketClickedCommand.Execute(
					new Tuple<WireSocket, bool, Position>(
						s, IsFirstSocket, Coord));

				e.Handled = true;
			}
		}

		#endregion

		#region Coord Dependency Property
		
		/// <summary>
		/// Position of this socket
		/// </summary>
		public Position Coord
		{
			get => (Position)GetValue(CoordProperty);
			set => SetValue(CoordProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="Coord"/>
		/// </summary>
		public static readonly DependencyProperty CoordProperty = DependencyProperty.Register(nameof(Coord),
			typeof(Position), typeof(WireSocketControl), new PropertyMetadata(null, new PropertyChangedCallback(OnCoordChanged)));

		#endregion

		#region Private Methods	

		/// <summary>
		/// Method fired when <see cref="CoordProperty"/> changes
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnCoordChanged(
			DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is WireSocketControl wire)
			{
				// If old value wasn't null
				if (e.OldValue is Position cOld)
				{
					// Unsubscribe from its Internal State Changed
					cOld.InternalStateChanged -= wire.CoordChanged;
				}

				// If new value isn't null
				if (e.NewValue is Position cNew)
				{
					cNew.InternalStateChanged += wire.CoordChanged;
				}

				wire.CoordChanged(null);
			}
		}

		/// <summary>
		/// Fires OnPropertyChanged("Coord"), used when subscribing to events
		/// </summary>
		/// <param name="sender"></param>
		private void CoordChanged(object sender) => OnPropertyChanged("Coord");

		#endregion

		#region IsFirstSocket Dependency Property

		/// <summary>
		/// If true it means that this control is the first (leading) socket of the wire
		/// </summary>
		public bool IsFirstSocket
		{
			get => (bool)GetValue(IsFirstSocketProperty);
			set => SetValue(IsFirstSocketProperty, value);
		}

		/// <summary>
		/// Backing store for <see cref="IsFirstSocket"/>
		/// </summary>
		public static readonly DependencyProperty IsFirstSocketProperty =
			DependencyProperty.Register(nameof(IsFirstSocket), typeof(bool),
			 typeof(WireSocketControl), new PropertyMetadata(false));

		#endregion
	}
}