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
	public sealed partial class LatchLabel : UserControl, INotifyPropertyChanged
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public LatchLabel()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Property Changed Event

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Label Dependence Property
		
		/// <summary>
		/// Presented text
		/// </summary>
		public string Label
		{
			get => (string)GetValue(LabelProperty);
			set => SetValue(LabelProperty, value);
		}

		// Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty LabelProperty =
			DependencyProperty.Register(nameof(Label), typeof(string), typeof(LatchLabel), new PropertyMetadata(string.Empty));

		#endregion

		#region ShowNegation Dependence Property

		/// <summary>
		/// When true, negation will be drawn above the text
		/// </summary>
		public bool ShowNegation
		{
			get => (bool)GetValue(ShowNegationProperty);
			set => SetValue(ShowNegationProperty, value);
		}

		// Using a DependencyProperty as the backing store for ShowNegation.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ShowNegationProperty =
			DependencyProperty.Register(nameof(ShowNegation), typeof(bool), typeof(LatchLabel),
				new PropertyMetadata(false));

		#endregion
	}
}
