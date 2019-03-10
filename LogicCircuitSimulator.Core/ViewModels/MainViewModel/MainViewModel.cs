using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using CSharpEnhanced.Collections;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// ViewModel for the main window in this app
	/// </summary>
	public class MainViewModel : BaseViewModel
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public MainViewModel()
		{
			MainPanelClickedCommand = new RelayParametrizedCommand(MainPanelClicked);
			SocketClickedCommand = new RelayParametrizedCommand(SocketClicked);
			WireSocketClickedCommand = new RelayParametrizedCommand(WireSocketClicked);
			WireClickedCommand = new RelayParametrizedCommand(WireClicked);
			RemoveCommand = new RelayParametrizedCommand(Remove);
			ReturnToIdleCommand = new RelayCommand(ReturnToIdle);
			EditPartCommand = new RelayParametrizedCommand(EditPart);
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Wire that is currently being placed (when node pairing is active)
		/// </summary>
		private Wire mPlacedWire;

		/// <summary>
		/// Backing store for <see cref="EditMenuViewModel"/>
		/// </summary>
		private readonly PartEditSectionViewModel mEditMenuViewModel = new PartEditSectionViewModel();

		/// <summary>
		/// Index of the part selected to be added (from <see cref="PartsToAdd"/>)
		/// </summary>
		private string mSelectedPartToAdd = null;

		#endregion

		#region Public Properties

		/// <summary>
		/// Collection with all part types
		/// </summary>
		public PartTypeCollection PartTypeCollection => PartTypeCollection.Singleton;

		/// <summary>
		/// Current state of the app
		/// </summary>
		public AppState AppState { get; set; }

		/// <summary>
		/// The currently presented part edit view model
		/// </summary>
		public PartEditSectionViewModel EditMenuViewModel => mEditMenuViewModel;

		/// <summary>
		/// Selected part to be added (from <see cref="PartsToAdd"/>)
		/// </summary>
		public string SelectedPartToAdd
		{
			get => mSelectedPartToAdd;
			set
			{
				mSelectedPartToAdd = value;

				if(mSelectedPartToAdd == null)
				{
					AppState = AppState.Idle;
				}
				else
				{
					AppState = AppState.AddingParts;
				}			
			}
		}

		/// <summary>
		/// When true, AddParts buttons should be enabled
		/// </summary>
		public bool EnableAddPartsButtons => AppState != AppState.WireManipulation;
		
		#region Parts

			/// <summary>
			/// Collection with all placed parts
			/// </summary>
		public ObservableCollection<BasePart> Parts { get; set; } =
			new ObservableCollection<BasePart>();

		#endregion		

		#endregion

		#region Commands

		/// <summary>
		/// Handles clicks performed on the main panel. Does different things depending on
		/// what is the current state of the app. The parameter should be
		/// a Coord with X and Y coordinates of the click.
		/// Idle: nothing.
		/// AddingParts: Adds a new part in the desired location.
		/// NodePairing: Draws the new cable up to the desired point.
		/// </summary>
		public ICommand MainPanelClickedCommand { get; set; }

		/// <summary>
		/// Removes a part. Parameter should be the the part.
		/// </summary>
		public ICommand RemoveCommand { get; set; }

		/// <summary>
		/// Command which handles clicks on a <see cref="Socket"/>s. If the app is in the
		/// <see cref="AppState.Idle"/> state, it will begin Node Pairing. If the app is in the
		/// <see cref="AppState.WireManipulation"/>, it will try to pair the node that invoked that state
		/// and the node that tries to end that state (i.e. the node that has been clicked).
		/// Parameter should be the socket that has been clicked.
		/// </summary>
		public ICommand SocketClickedCommand { get; set; }

		/// <summary>
		/// Command which handles clicks on a <see cref="Wire"/>s. If the app is in the
		/// <see cref="AppState.Idle"/> state, it will begin Node Pairing. If the app is in the
		/// <see cref="AppState.WireManipulation"/>, it will try to pair the node that invoked
		/// that state and the node that tries to end that state
		/// (i.e. the node that has been clicked).
		/// Parameter should be a tuple, where 1st item is the wire that has been clicked
		/// the second should be the Coord with X and Y coordinates of the click.
		/// </summary>
		public ICommand WireClickedCommand { get; set; }

		/// <summary>
		/// Command which handles clicks on a <see cref="WireSocket"/>s. If the app is in the
		/// <see cref="AppState.Idle"/> state, it will enable the user to extend the wire.
		/// If the app is in the <see cref="AppState.WireManipulation"/> state,
		/// it will try to pair the node that invoked that state and the node that tries
		/// to end that state (i.e. the WireSocket that has been clicked).
		/// Parameter should be the a Tuple(WireSocket, bool, Coord) where
		/// WireSocket is the socket that was clicked, bool should be true if the WireSocket is
		/// the one at the beginning of the wire and Coord should be the XY coordinates
		/// of the WireSocket.
		/// </summary>
		public ICommand WireSocketClickedCommand { get; set; }

		/// <summary>
		/// Finishes whatever action was currently done and returns to the idle state
		/// </summary>
		public ICommand ReturnToIdleCommand { get; set; }

		/// <summary>
		/// Enables the editing of the part given by the parameter
		/// </summary>
		public ICommand EditPartCommand { get; set; }

		#endregion

		#region Private Methods
		
		#region Command Methods

		#region Click Handling

		/// <summary>
		/// Method for <see cref="MainPanelClickedCommand"/>
		/// </summary>
		private void MainPanelClicked(object parameter)
		{
			switch(AppState)
			{				
				
				case AppState.AddingParts:
					{
						AddPart(parameter);
					}
					break;

				case AppState.WireManipulation:
					{
						ClickDuringWireManipulation(parameter);
					}
					break;
			}
		}

		/// <summary>
		/// Method for <see cref="SocketClickedCommand"/>
		/// </summary>
		private void SocketClicked(object parameter)
		{
			switch (AppState)
			{
				case AppState.Idle:
					{
						BeginNodePairing(parameter);
					}
					break;			

				case AppState.WireManipulation:
					{
						if (parameter is Socket s && s.Highlight)
						{
							EndNodePairing(parameter);
						}
					}
					break;
			}
		}

		/// <summary>
		/// Method for <see cref="WireClickedCommand"/>
		/// </summary>
		/// <param name="parameter"></param>
		private void WireClicked(object parameter)
		{
			if(parameter is Tuple<Wire, Position> tuple)
			{
				switch(AppState)
				{
					case AppState.Idle:
						{
							BeginWireManipulationFromWire(tuple.Item1, tuple.Item2);
						}
						break;					

					case AppState.WireManipulation:
						{
							if(mPlacedWire.State.CanPairWithOutput ||
								tuple.Item1.State.CanPairWithOutput)
							{
								EndWireManipulationOnWire(tuple.Item1, tuple.Item2);
							}
						}
						break;
				}
			}
		}

		/// <summary>
		/// Method for <see cref="WireSocketClickedCommand"/>
		/// </summary>
		/// <param name="parameter"></param>
		private void WireSocketClicked(object parameter)
		{
			if(parameter is Tuple<WireSocket, bool, Position> tuple)
			{
				switch(AppState)
				{
					case AppState.Idle:
						{
							BeginWireManipulationFromWireSocket(tuple.Item1, tuple.Item2);
						}
						break;

					case AppState.WireManipulation:
						{
							if (IDManager.TryGetIDOwner(tuple.Item1.WireID, out Wire wire))
							{
								EndWireManipulationOnWire(wire, tuple.Item3);
							}
						}
						break;
				}
			}
		}

		#endregion

		#region Adding/Removing Parts

		/// <summary>
		/// Method for <see cref="AddPartCommand"/>
		/// </summary>
		private void AddPart(object parameter)
		{
			// If we're currently adding parts and the parameter is a tuple of 2 doubles
			if(AppState == AppState.AddingParts && parameter is Position castedParam)
			{
				// Create the new part
				object newPart = Activator.CreateInstance(PartTypeCollection.Singleton.AllParts[SelectedPartToAdd]);

				// And check if it derives from BasePart
				// (make sure it's a part, not a Node for example)
				if(newPart is BasePart baseNewPart)
				{
					// Get both coordinates
					baseNewPart.CenterCoord.X = 25 * Math.Round(castedParam.X / 25);
					baseNewPart.CenterCoord.Y = 25 * Math.Round(castedParam.Y / 25);

					// Comput coordinates for sockets
					baseNewPart.ComputeSocketCoords();
					
					Parts.Add(baseNewPart);
				}
			}
		}		

		/// <summary>
		/// Command for <see cref="RemoveCommand"/>
		/// </summary>
		/// <param name="parameter"></param>
		private void Remove(object parameter)
		{
			if (parameter is BasePart part)
			{
				// If the deleted part is currently edited
				if(EditMenuViewModel.CurrentEditMenuViewModel?.EditedPartID == part.ID)
				{
					// Remove the edit view model to stop editing
					EditMenuViewModel.CurrentEditMenuViewModel = null;
				}

				part.Dispose();
				Parts.Remove(part);
			}
		}

		#endregion

		#region Return to idle

		/// <summary>
		/// Method for <see cref="ReturnToIdleCommand"/>
		/// </summary>
		private void ReturnToIdle()
		{
			// Cleanup after the specific state
			switch(AppState)
			{
				case AppState.WireManipulation:
					{
						CleanUpAfterWireManipulation();
					}
					break;
			}

			AppState = AppState.Idle;
		}

		#endregion

		#region Part Edit

		/// <summary>
		/// Method for <see cref="EditPartCommand"/>
		/// </summary>
		/// <param name="parameter"></param>
		private void EditPart(object parameter)
		{
			if (parameter is BasePart part)
			{
				Dictionary<Type, int> typeDictionary = new Dictionary<Type, int>()
				{
					{typeof(ClockInput), 0 },
					{typeof(ORGate), 1 },
					{typeof(NORGate), 1 },
					{typeof(ANDGate), 1 },
					{typeof(NANDGate), 1 },
					{typeof(XORGate), 1 },
					{typeof(XNORGate), 1 },
					{typeof(ButtonInput), 2 },
					{typeof(DLatch), 3},
					{typeof(DFlipFlop), 3},
					{typeof(TLatch), 3},
					{typeof(TFlipFlop), 3},
					{typeof(JKLatch), 3},
					{typeof(JKFlipFlop), 3},
				};

				if (typeDictionary.ContainsKey(part.GetType()))
				{
					switch (typeDictionary[part.GetType()])
					{
						case 0:
							{
								EditMenuViewModel.CurrentEditMenuViewModel = new ClockInputEditViewModel((ClockInput)part);
							}
							break;

						case 1:
							{
								EditMenuViewModel.CurrentEditMenuViewModel = new MultiInputGateEditViewModel((MultiInputGate)part);
							}
							break;

						case 2:
							{
								EditMenuViewModel.CurrentEditMenuViewModel = new ButtonInputEditViewModel((ButtonInput)part);
							}
							break;

						case 3:
							{
								EditMenuViewModel.CurrentEditMenuViewModel = new LatchEditViewModel((LatchBaseStructure)part);
							}
							break;
					}
				}
				else
				{
					EditMenuViewModel.CurrentEditMenuViewModel = new BasePartEditViewModel(part);
				}
			}
		}

		#endregion

		#endregion

		#region Wire Manipulation

		#region Helper Methods

		/// <summary>
		/// Adds a point to the currently placed wire (if it's not null). Checks whether
		/// the point should be added at the beginning or the end.
		/// Rounds the points so that they're a multiple of 25.
		/// </summary>
		/// <param name="position"></param>
		private void AddPointToPlacedWire(Position position)
		{
			mPlacedWire?.Coords.Insert(
					mPlacedWire.ExtendAtBeginning ? 0 : mPlacedWire.Coords.Count, position);

			mPlacedWire.OnPropertyChanged("Coords");
		}

		/// <summary>
		/// Adds a point to the currently placed wire (if it's not null). Checks whether
		/// the point should be added at the beginning or the end.
		/// Rounds the points so that they're a multiple of 25.
		/// </summary>
		/// <param name="coord"></param>
		private void AddPointToPlacedWire(double xCoord, double yCoord) =>
			AddPointToPlacedWire(new Position(xCoord, yCoord));

		/// <summary>
		/// Cleans up after wire manipulation:
		/// Sets the <see cref="AppState"/> to <see cref="AppState.Idle"/>,
		/// Removes all highlights on sockets,
		/// Resets ExtendAtBeginning flag in the <see cref="mPlacedWire"/>,
		/// Sets <see cref="mPlacedWire"/> to null.
		/// </summary>
		private void CleanUpAfterWireManipulation()
		{
			// Exit the Node Pairing state
			AppState = AppState.Idle;

			// Remove the highlight from all input sockets
			IDManager.ForAll<Socket>(x => x.Highlight = false);

			// Reset the flag that causes the wire to extend at the beginning
			mPlacedWire.ExtendAtBeginning = false;
						
			// And remove the reference from the mPlacedWire variable
			mPlacedWire = null;
		}

		#endregion

		#region General

		/// <summary>
		/// Decides which type of node pairing will happen and calls the appropriate method
		/// </summary>
		/// <param name="parameter"></param>
		private void BeginNodePairing(object parameter)
		{
			Dictionary<Type, int> typeDictionary = new Dictionary<Type, int>()
			{
				{typeof(InputSocket), 0},
				{typeof(OutputSocket), 1},
				{typeof(ComplementedOutputSocket), 1},
				{typeof(WireSocket), 2},
			};

			if (typeDictionary.ContainsKey(parameter.GetType()))
			{
				switch (typeDictionary[parameter.GetType()])
				{
					// InputSocket & WireSocket
					case 0:
					case 2:
						{
							if (parameter is InputSocket s && s.CanPairWithOutput)
							{
								BeginWireManipulationFromInput(s);
							}
						}
						break;

					// Output Socket
					case 1:
						{
							BeginWireManipulationFromOutput((Socket)parameter);
						}
						break;
				
				}
			}
		}

		/// <summary>
		/// Decides which node pairing method to call
		/// </summary>
		/// <param name="parameter"></param>
		private void EndNodePairing(object parameter)
		{
			Dictionary<Type, int> typeDictionary = new Dictionary<Type, int>()
			{
				{typeof(InputSocket), 0},
				{typeof(OutputSocket), 1},
				{typeof(WireSocket), 2},
			};

			if (typeDictionary.ContainsKey(parameter.GetType()))
			{
				switch (typeDictionary[parameter.GetType()])
				{
					// InputSocket & WireSocket
					case 0:
					case 2:
						{
							if (parameter is InputSocket s && s.Highlight)
							{
								EndWireManipulationFromOutput(s);
							}
						}
						break;

					// Output Socket
					case 1:
						{
							EndWireManipulationFromInput((Socket)parameter);
						}
						break;					
				}
			}
		}

		#endregion

		#region Wire manipulation from output socket

		/// <summary>
		/// Begins pairing nodes from an <see cref="Socket"/>
		/// </summary>
		/// <param name="socket"></param>
		private void BeginWireManipulationFromOutput(Socket socket)
		{
			// Set the app state
			AppState = AppState.WireManipulation;

			// Highlight all input sockets
			IDManager.ForAll<InputSocket>(x => x.Highlight = true, x => x.CanPairWithOutput);

			// Create a new wire which will be placed
			mPlacedWire = new Wire();

			// Add the wire to the list of parts
			Parts.Add(mPlacedWire);

			// Pair the nodes with the wire
			mPlacedWire.State.SetNode(socket.Node);

			// Add the last point to the wire
			AddPointToPlacedWire(socket.Position);
		}

		/// <summary>
		/// Ends node pairing that was started from an output socket.
		/// </summary>
		/// <param name="socket">Socket to pair, it has to be a socket that can
		/// be paired</param>
		private void EndWireManipulationFromOutput(InputSocket socket)
		{			
			// Add the last point to the wire
			AddPointToPlacedWire(socket.Position);

			// Pair the nodes with the wire
			socket.SetNode(mPlacedWire.State.Node);

			// Add the socket to the connected inputs
			mPlacedWire.AddNewInputConnection(socket.ID);

			CleanUpAfterWireManipulation();
		}

		#endregion

		#region Wire manipulation from input socket

		/// <summary>
		/// Begins pairing nodes from an uconnected <see cref="InputSocket"/>
		/// </summary>
		/// <param name="socket"></param>
		private void BeginWireManipulationFromInput(InputSocket socket)
		{
			// Start the pairing action
			AppState = AppState.WireManipulation;

			// Highlight all output sockets
			IDManager.ForAll<OutputSocket>(x => x.Highlight = true);

			// And all input sockets that aren't connected to the output yet
			IDManager.ForAll<InputSocket>(x => x.Highlight = true, x => x.CanPairWithOutput);

			// Create a new wire which will be placed
			mPlacedWire = new Wire();

			// Add the wire to the list of parts
			Parts.Add(mPlacedWire);

			// Pair the nodes with the wire
			mPlacedWire.State.SetNode(socket.Node);

			// Add the socket to the list of connections
			mPlacedWire.AddNewInputConnection(socket.ID);

			// Add the last point to the wire
			AddPointToPlacedWire(socket.Position);
		}

		/// <summary>
		/// Ends node pairing that started from an <see cref="InputSocket"/>
		/// </summary>
		/// <param name="socket"></param>
		private void EndWireManipulationFromInput(Socket socket)
		{
			// Add the last point to the wire
			AddPointToPlacedWire(socket.Position);

			// Pair the nodes with the wire
			mPlacedWire.State.SetNode(socket.Node);

			CleanUpAfterWireManipulation();
		}

		#endregion

		#region Wire manipulation from wire

		/// <summary>
		/// Begins pairing nodes from the given wire
		/// </summary>
		/// <param name="wire"></param>
		/// <param name="XCoord"></param>
		/// <param name="YCoord"></param>
		private void BeginWireManipulationFromWire(Wire wire, Position coord)
		{
			// Start the pairing action
			AppState = AppState.WireManipulation;

			// If the wire isn't connected
			if(wire.State.CanPairWithOutput)
			{
				// Highlight all sockets (we can pair with more unconnected
				// inputs or with an output) except for already connected inputs
				IDManager.ForAll<OutputSocket>(x => x.Highlight = true);
				IDManager.ForAll<InputSocket>(x => x.Highlight = true, x => x.CanPairWithOutput);
			}
			else
			{
				// Otherwise highlight only the unconnected input sockets
				IDManager.ForAll<InputSocket>(x => x.Highlight = true, x => x.CanPairWithOutput);
			}

			// Create a new wire which will be placed
			mPlacedWire = new Wire();

			// Add the wire to the list of parts
			Parts.Add(mPlacedWire);

			// Pair the nodes with the wire
			mPlacedWire.State.SetNode(wire.State.Node);

			// Add the socket to the list of connections
			wire.AddNewInputConnection(mPlacedWire.State.ID);

			// Add the first point of the wire as the center of the socket
			AddPointToPlacedWire(coord.X, coord.Y);

			wire.AddIntermediateCoord(new Position(coord.X, coord.Y));
		}

		/// <summary>
		/// Ends node pairing that started from an <see cref="InputSocket"/>
		/// </summary>
		/// <param name="socket"></param>
		private void EndWireManipulationOnWire(Wire wire, Position coord)
		{
			if(!mPlacedWire.State.CanPairWithOutput && !wire.State.CanPairWithOutput)
			{
				return;
			}

			// Add the last point of the wire as the center of the socket
			AddPointToPlacedWire(coord.X, coord.Y);

			wire.AddIntermediateCoord(new Position(coord.X, coord.Y));

			// Pair the output node to the null slot for a node
			if (mPlacedWire.State.CanPairWithOutput)
			{
				mPlacedWire.State.SetNode(wire.State.Node);
			}
			else
			{
				wire.State.SetNode(mPlacedWire.State.Node);
			}

			// Add wires to each other's connections
			wire.AddNewInputConnection(mPlacedWire.State.ID);

			CleanUpAfterWireManipulation();
		}

		#endregion

		#region Wire manipualtion from wire socket

		/// <summary>
		/// Begins pairing nodes from an uconnected <see cref="InputSocket"/>
		/// </summary>
		/// <param name="socket"></param>
		private void BeginWireManipulationFromWireSocket(WireSocket socket, bool extendAtBeginning)
		{
			// Get the socket's wire
			if (IDManager.TryGetIDOwner(socket.WireID, out Wire wire))				
			{
				// Start the pairing action
				AppState = AppState.WireManipulation;

				// Assign the wir to the mPlacedWire
				mPlacedWire = wire;

				mPlacedWire.ExtendAtBeginning = extendAtBeginning;

				// Highlight all output sockets
				IDManager.ForAll<OutputSocket>(x => x.Highlight = true);

				// And input sockets that can be paired with output
				IDManager.ForAll<InputSocket>(x => x.Highlight = true, x => x.CanPairWithOutput);				
			}
		}

		#endregion

		#region Click during wire manipulation

		/// <summary>
		/// Adds a new coord to the currently placed wire
		/// </summary>
		/// <param name="parameter"></param>
		private void ClickDuringWireManipulation(object parameter)
		{
			// Add the desired coordinate to the wire (first round it)
			if (parameter is Position c)
			{
				AddPointToPlacedWire(c);
			}
		}

		#endregion

		#endregion

		#endregion
	}
}