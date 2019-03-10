using LogicCircuitSimulator.Core;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LogicCircuitSimulator
{
	public sealed partial class MultiInputSocketControl : UserControl, INotifyPropertyChanged
	{
		#region Constructor

		public MultiInputSocketControl()
		{
			this.InitializeComponent();

			// When DataContext changes subscribe the the input collection's
			// CollectionChanged event and create the first presentation of controls
			this.DataContextChanged += (s, e) =>
			{
				InputCollection.CollectionChanged -= HandleElementsAddedDeleted;
				InputCollection.CollectionChanged += HandleElementsAddedDeleted;

				PresentedCollectionChanged();
			};
		}

		#endregion

		#region InputCollection Dependency Property

		/// <summary>
		/// Collection to present using this control
		/// </summary>
		public ObservableCollection<InputSocket> InputCollection
		{
			get => (ObservableCollection<InputSocket>)GetValue(InputCollectionProperty);
			set
			{
				// Unsubscribe from the old event
				if (InputCollection != null)
				{
					InputCollection.CollectionChanged -= HandleElementsAddedDeleted;
				}

				SetValue(InputCollectionProperty, value);

				// Subscribe to the new event
				value.CollectionChanged += HandleElementsAddedDeleted;
				PresentedCollectionChanged();
			}
		}

		/// <summary>
		/// Backing store for <see cref="InputCollection"/>
		/// </summary>
		public static readonly DependencyProperty InputCollectionProperty =
			DependencyProperty.Register(nameof(InputCollection),
			typeof(ObservableCollection<InputSocket>),
			typeof(MultiInputSocketControl), new PropertyMetadata(null));

		#endregion

		#region DifferentiateWireLengths dependence property

		/// <summary>
		/// If true, lengths of wires will be differentiated to fit a curved back (for example of an OR gate)
		/// </summary>
		public bool DifferentiateWireLengths
		{
			get => (bool)GetValue(DifferentiateWireLengthsProperty);
			set => SetValue(DifferentiateWireLengthsProperty, value);
		}

		/// <summary>
		/// If true, lengths of wires will be differentiated to fit a curved back (for example of an OR gate)
		/// </summary>
		public static readonly DependencyProperty DifferentiateWireLengthsProperty =
			DependencyProperty.Register("DifferentiateWireLengths", typeof(bool), typeof(MultiInputSocketControl), 
				new PropertyMetadata(false, new PropertyChangedCallback(DifferentiateWireLengthsChanged)));

		#endregion

		#region PropertyChanged Event

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Fires <see cref="PropertyChanged"/> event for the given properties
		/// </summary>
		/// <param name="propertyNames"></param>
		public void OnPropertyChanged(params string[] propertyNames)
		{
			foreach(var item in propertyNames)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(item));
			}
		}

		#endregion

		#region Private Methods

		#region Generating new parts

		/// <summary>
		/// Creates and returns a border which is used as a wire from an input socket
		/// </summary>
		/// <param name="index">Position of this wire (starting from 0, from top)</param>
		/// <returns></returns>
		private Border GenerateInputWireBorder(int index)
		{
			// Create a new border
			Border border = new Border()
			{
				Height = 5,
				BorderThickness = new Thickness(1),
				BorderBrush = App.Current.Resources["BlackBrush"] as SolidColorBrush,
				HorizontalAlignment = HorizontalAlignment.Left,
				Tag = index,
			};

			
			if (DifferentiateWireLengths)
			{
				// Create a binding for its width
				Binding widthBinding = new Binding()
				{
					Path = new PropertyPath("Inputs.Count"),
					Converter = new ORGateInputWireLengthConverter(),
					ConverterParameter = index,
				};

				border.SetBinding(Border.WidthProperty, widthBinding);
			}
			else
			{
				// Othwerwise set a constant value
				border.Width = 15;
			}

			Binding marginBinding = new Binding()
			{
				Path = new PropertyPath("InputCollection.Count"),
				Source = this,
				Converter = new MultiInputWireMarginConverter(),
				ConverterParameter = index,
			};

			// Background binding
			Binding backgroundBinding = new Binding()
			{
				Path = new PropertyPath($"Inputs[{index}].Value"),
				Converter = new FalsePositiveBrushConverter(),				
			};

			// Set the bindings
			border.SetBinding(Border.MarginProperty, marginBinding);
			border.SetBinding(Border.BackgroundProperty, backgroundBinding);

			return border;
		}

		/// <summary>
		/// Creates and returns a SocketControl
		/// </summary>
		/// <param name="index">Position of this SocketControl 
		/// (starting from 0, from top)</param>
		/// <returns></returns>
		private SocketControl GenerateNewSocketControl(int index)
		{
			SocketControl socket = new SocketControl();			

			Binding marginBinding = new Binding()
			{
				Path = new PropertyPath("InputCollection.Count"),
				Source = this,
				Converter = new MultiInputSocketMarginConverter(),
				ConverterParameter = index,
			};

			// DataContextBinding to InputSocket in place index in the collection
			Binding dataContextBinding = new Binding()
			{
				Path = new PropertyPath($"InputCollection[{index}]"),
				Source = this,
			};

			// Set the bindings
			socket.SetBinding(SocketControl.DataContextProperty, dataContextBinding);
			socket.SetBinding(SocketControl.MarginProperty, marginBinding);

			return socket;
		}

		#endregion

		#region Actions taken when the presented collection changes

		/// <summary>
		/// Removes old items and generates new for the new collection
		/// </summary>
		/// <param name="newCollection"></param>
		private void PresentedCollectionChanged()
		{
			// Clear the old items
			SocketStackPanel.Children.Clear();
			WireStackPanel.Children.Clear();			

			// Make new items
			for (int i = 0; i < InputCollection.Count; ++i)
			{
				// Generate a new wire
				WireStackPanel.Children.Add(GenerateInputWireBorder(i));

				SocketStackPanel.Children.Add(GenerateNewSocketControl(i));
			}
		}

		/// <summary>
		/// Handles the collection changed actions
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HandleElementsAddedDeleted(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch(e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					{
						foreach(var item in e.NewItems)
						{
							// Generate a new wire+
							WireStackPanel.Children.Add(
								GenerateInputWireBorder(InputCollection.Count - 1));

							// Generate a new socket
							SocketStackPanel.Children.Add(
								GenerateNewSocketControl(InputCollection.Count - 1));							
						}
					}
					break;

				case NotifyCollectionChangedAction.Remove:
					{
						foreach (var item in e.OldItems)
						{
							// Remove the wire
							WireStackPanel.Children.RemoveAt(
								WireStackPanel.Children.Count - 1);

							// And the socket
							SocketStackPanel.Children.RemoveAt(
								SocketStackPanel.Children.Count - 1);
						}

						// Force garbage collection because if it's not done immediately the converters will work for no reason
						GC.Collect();
					}
					break;

				case NotifyCollectionChangedAction.Reset:
					{
						PresentedCollectionChanged();
					}
					break;
			}
		}

		#endregion

		#region Type of wires changed

		/// <summary>
		/// Method which recalculates wire lengths in the sender
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public static void DifferentiateWireLengthsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			if (sender is MultiInputSocketControl d)
			{
				foreach (var item in d.WireStackPanel.Children)
				{
					if (item is Border b)
					{
						if (d.DifferentiateWireLengths)
						{
							// Create a binding for its width
							Binding widthBinding = new Binding()
							{
								Path = new PropertyPath("Inputs.Count"),
								Converter = new ORGateInputWireLengthConverter(),
								ConverterParameter = b.Tag,
							};

							b.SetBinding(Border.WidthProperty, widthBinding);
						}
						else
						{
							// Othwerwise set a constant value
							b.Width = 15;
						}
					}
				}
			}
		}

		#endregion

		#endregion

	}
}