using CSharpEnhanced.Maths;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Base class for all parts. Automatically gets an ID in the constructor
	/// </summary>
	public abstract class BasePart : Identifiable, INotifyPropertyChanged, IDisposable
	{
		#region Constructor

		/// <summary>
		/// Default constructors
		/// </summary>
		public BasePart(string displayName)
		{
			mDisplayName = displayName;
			CenterCoord.InternalStateChanged += (e) => OnPropertyChanged("XCoord");
			CenterCoord.InternalStateChanged += (e) => OnPropertyChanged("YCoord");
			CenterCoord.InternalStateChanged += (e) => ComputeSocketCoords();
			RotateLeftCommand = new RelayCommand(RotateLeft);
			RotateRightCommand = new RelayCommand(RotateRight);
		}

		#endregion

		#region Property Changed Event

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		#endregion

		#region Private Members

		/// <summary>
		/// Display name of this part
		/// </summary>
		private readonly string mDisplayName = "Default Name";

		/// <summary>
		/// Current angle of rotation
		/// </summary>
		private double mRotationAngle;

		#endregion

		#region Public Properties

		/// <summary>
		/// Display name of this part
		/// </summary>
		public string DisplayName => mDisplayName;

		/// <summary>
		/// X coordinate of this part's top left corner (origin is top left corner)
		/// </summary>
		[DependsOn("CenterCoord.X")]
		public virtual double XCoord => CenterCoord.X - (Width / 2);

		/// <summary>
		/// Y coordinate of this part's top left corner (origin is top left corner)
		/// </summary>
		public virtual double YCoord => CenterCoord.Y - (Height / 2);

		/// <summary>
		/// Width of this part
		/// </summary>
		public double Width { get; set; }

		/// <summary>
		/// Height of this part
		/// </summary>
		public double Height { get; set; }

		/// <summary>
		/// Horizontal center point for part model rotation in UI
		/// </summary>
		public virtual double HorizontalRotationCenter => Width / 2;

		/// <summary>
		/// Vertical center point for part model rotation in UI
		/// </summary>
		public virtual double VerticalRotationCenter => Height / 2;

		/// <summary>
		/// Absolute coord of the center of this part
		/// </summary>
		public Coord CenterCoord { get; private set; } = new Coord();

		/// <summary>
		/// Angle of part's rotation in degrees
		/// </summary>
		public double RotationAngle
		{
			get => mRotationAngle;
			set
			{
				mRotationAngle = Helpers.ReduceAngle(value, AngleUnit.Degrees);
				ComputeSocketCoords();
			}
		}

		/// <summary>
		/// Angle of the part's rotation counted clockwise (for UI in which the Y axis has positive values below X axis)
		/// </summary>
		public double RotationAngleClockWise => -RotationAngle;
		
		#endregion

		#region Abstract Methods

		/// <summary>
		/// Method which disposes of this part (makes sure all output nodes are
		/// removed from all clients)
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// Computs coordinates of the center of each socket
		/// </summary>
		public abstract void ComputeSocketCoords();

		#endregion		

		#region Commands

		/// <summary>
		/// Removes a part. Parameter should be the the part.
		/// </summary>
		public ICommand RemoveCommand { get; private set; } =
			IoC.GetPrime<MainViewModel>().RemoveCommand;

		/// <summary>
		/// Enables the editing of the part given by the parameter
		/// </summary>
		public ICommand EditPartCommand { get; set; } =
			IoC.GetPrime<MainViewModel>().EditPartCommand;

		/// <summary>
		/// Rotates the part 90 degrees right. Method: <see cref="RotateLeft"/>
		/// </summary>
		public ICommand RotateLeftCommand { get; set; }

		/// <summary>
		/// Rotates the part 90 degrees right. Method: <see cref="RotateRight"/>
		/// </summary>
		public ICommand RotateRightCommand { get; set; }

		#endregion

		#region Command Methods

		/// <summary>
		/// Rotates the part 90 degrees right. Command: <see cref="RotateLeftCommand"/>
		/// </summary>
		private void RotateLeft() => RotationAngle += 90;

		/// <summary>
		/// Rotates the part 90 degrees right. Command: <see cref="RotateRightCommand"/>
		/// </summary>
		private void RotateRight() => RotationAngle -= 90;

		#endregion

		#region Public Methods

		/// <summary>
		/// Removes this part from the list of parts and deletes it.	
		/// </summary>
		public void Remove() => RemoveCommand.Execute(this);

		#endregion
	}
}
