using CSharpEnhanced.Maths;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Wire (simple input-output connection, no logic operations)
	/// </summary>
	public class Wire : BasePart
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public Wire() : base("Wire")
		{
			State.WireID = ID;

			// Refreshes the wire's color whenever the internal state is changed
			State.InternalStateChanged += (s) =>
			{
				OnPropertyChanged("State.Value");
			};

			// Refreshes the wire's color whenever the more cable is placed
			Coords.CollectionChanged += (s, e) =>
			{
				OnPropertyChanged("State.Value");				
				OnPropertyChanged("FirstSocketCoord");				
				OnPropertyChanged("SecondSocketCoord");
				OnPropertyChanged(nameof(FirstSocketCoord));
				OnPropertyChanged(nameof(SecondSocketCoord));
				if(e.Action== NotifyCollectionChangedAction.Add)
				{
					foreach(var item in e.NewItems)
					{
						if(item is Position c)
						{
							c.InternalStateChanged += (cd) => OnPropertyChanged("Coords");
						}
					}
				}
			};

			Coords.CollectionChanged += CalculateXYCoords;

			// If the State Node changes, update all connected inputs
			State.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == "Node")
				{
					mConnectedInputs.ForEach((x) =>
					{
						if(IDManager.TryGetIDOwner(x, out InputSocket socket))
						{
							socket.SetNode(State.Node);
						}
					});
				}
			};
		}

		#endregion

		#region Private Members

		/// <summary>
		/// List with IDs of all input sockets that are connected to this wire
		/// </summary>
		private readonly List<int> mConnectedInputs = new List<int>();		

		#endregion

		#region Public Properties

		/// <summary>
		/// Coordinate of the socket at the beginning of the wire
		/// </summary>
		public Position FirstSocketCoord => Coords.Count > 0 ? Coords[0] : new Position();

		/// <summary>
		/// Coordinate of the socket at the end of the wire
		/// </summary>
		public Position SecondSocketCoord =>
			Coords.Count > 0 ? Coords[Coords.Count - 1] : new Position();

		/// <summary>
		/// True if the second socket should be visible
		/// </summary>
		bool SecondSocketVisible => Coords.Count > 1;

		/// <summary>
		/// When true, new points to the wire will be added at the beginning
		/// </summary>
		public bool ExtendAtBeginning { get; set; }

		/// <summary>
		/// X coordinate of this part's top left corner (origin is top left corner)
		/// </summary>
		new public double XCoord { get; set; }

		/// <summary>
		/// Y coordinate of this part's top left corner (origin is top left corner)
		/// </summary>
		new public double YCoord { get; set; }		

		/// <summary>
		/// State of the wire
		/// </summary>
		public WireSocket State { get; set; } = new WireSocket();

		/// <summary>
		/// Coordinates this wire goes through
		/// </summary>
		public ObservableCollection<Position> Coords { get; set; } =
			new ObservableCollection<Position>();		

		#endregion

		#region Private Methods

		/// <summary>
		/// Calculates the new XY Coordinates as well as the width and height of this wire
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CalculateXYCoords(object sender, NotifyCollectionChangedEventArgs e)
		{
			var enumerator = Coords.GetEnumerator();

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

			XCoord = xMin;
			YCoord = yMin;
			Width = xMax - xMin;
			Height = yMax - yMin;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds a new input to this wire's inputs
		/// </summary>
		/// <param name="inputID"></param>
		public void AddNewInputConnection(int inputID)
		{
			if(IDManager.TryGetIDOwner(inputID, out InputSocket owner))
			{
				mConnectedInputs.Add(inputID);			
			}
		}

		/// <summary>
		/// Adds an itermediate point to the wire (if the point doesn't lie on the wire, exception will be thrown)
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void AddintermediateCoord(double x, double y) => AddIntermediateCoord(new Position(x, y));

		/// <summary>
		/// Adds an itermediate point to the wire (if the point doesn't lie on the wire, exception will be thrown)
		/// </summary>
		/// <param name="pos"></param>
		public void AddIntermediateCoord(Position pos)
		{
			// If there are not enough coords to add intermediate points, throw an exception
			if (Coords.Count < 2)
			{
				throw new Exception("Can't add an intermediate point to a wire with less than 2 coords");
			}		

			// List with coordinates that are split so that between each pair there is a straight vertical or horizontal line
			var coordsSplit = new List<Position>() { Coords[0] };

			for (int i = 1; i < Coords.Count; ++i)
			{
				coordsSplit.Add(new Position(Coords[i - 1].X, Coords[i].Y));
				coordsSplit.Add(new Position(Coords[i].X, Coords[i].Y));
			}

			// If the point that we is already on the wire just return
			for(int i=0; i<coordsSplit.Count; ++i)
			{
				if(coordsSplit[i].X == pos.X && coordsSplit[i].Y == pos.Y)
				{
					return;
				}
			}

			for (int i = 0; i < coordsSplit.Count - 1; ++i)
			{
				// If the point lies on the line between two subsequent points
				if((coordsSplit[i].X==pos.X && coordsSplit[i+1].X==pos.X && (pos.Y-coordsSplit[i].Y)*(pos.Y-coordsSplit[i+1].Y) < 0) ||
					(coordsSplit[i].Y == pos.Y && coordsSplit[i + 1].Y == pos.Y && (pos.X - coordsSplit[i].X) * (pos.X - coordsSplit[i + 1].X) < 0))
				{
					Coords.Insert((int)Math.Floor((double)i / 2) + 1, pos);
					return;
				}
			}

			throw new Exception("The given position doesn't lie on the wire");
		}

		#endregion

		#region IDisposable

		/// <summary>
		/// Method which disposes of this part (makes sure all output nodes are
		/// removed from all clients)
		/// </summary>
		public override void Dispose()
		{
			if (State != null && State.Node != null)
			{
				mConnectedInputs.ForEach((connectionID) =>
					NodeReferenceDictionary.RemoveReference(State.Node.ID, connectionID));
			}
		}

		#endregion

		#region Implementation of Abstract Methods

		/// <summary>
		/// Does nothing in this part (it doesn't have any nodes)
		/// </summary>
		public override void ComputeSocketCoords() { }

		#endregion

		#region Commands

		/// <summary>
		/// Command which handles clicks on a <see cref="Wire"/>s. If the app is in the
		/// <see cref="AppState.Idle"/> state, it will begin Node Pairing. If the app is in the
		/// <see cref="AppState.WireManipulation"/>, it will try to pair the node that invoked
		/// that state and the node that tries to end that state
		/// (i.e. the node that has been clicked).
		/// Parameter should be a tuple, where 1st item is the wire that has been clicked
		/// the second should be the X coord of click, the third should by the Y coord
		/// of the click.
		/// </summary>
		public ICommand WireClickCommand { get; set; } =
			IoC.GetPrime<MainViewModel>().WireClickedCommand;

		#endregion
	}
}