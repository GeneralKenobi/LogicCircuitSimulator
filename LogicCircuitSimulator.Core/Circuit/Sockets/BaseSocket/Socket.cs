using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Socket is responsible for containing and pairing Nodes
	/// </summary>
	public abstract class Socket : Identifiable, INotifyPropertyChanged
	{
		#region Constructor

		/// <summary>
		/// Default constructor, gets ID for the element
		/// </summary>
		public Socket()
		{			
			InternalStateChanged += (s) => OnPropertyChanged("Value", "SocketState");
			Position.InternalStateChanged += PositionChanged;
		}

		#endregion

		#region Property Changed Event

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(params string[] propertyNames)
		{
			foreach (var item in propertyNames)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(item));
			}
		}

		#endregion

		#region Internal State Changed Event

		/// <summary>
		/// Event fired when the internal state of this socket changes
		/// </summary>
		public InternalStateChangedEventHandler InternalStateChanged;

		#endregion

		#region Protected Members

		/// <summary>
		/// Node this Socket is connected with
		/// </summary>
		protected Node mNode;

		#endregion

		#region Public Properties

		/// <summary>
		/// Node this Socket is connected with
		/// </summary>
		public Node Node => mNode;

		/// <summary>
		/// Getter to the value of this Socket
		/// </summary>
		public virtual bool Value => Node == null ? false : Node.Value;

		/// <summary>
		/// When true, this Socket is highlighted
		/// </summary>		
		public bool Highlight { get; set; }

		/// <summary>
		/// Length (both width and height) of the socket
		/// </summary>
		public double Length { get; private set; } = 10;
		
		public Position Position { get; private set; } = new Position();

		/// <summary>
		/// The current state of the socket
		/// </summary>
		public SocketState SocketState
		{
			get
			{
				if (Highlight)
				{
					return SocketState.Highlighted;
				}
				else
					if (Node == null)
				{
					return SocketState.NotConnected;
				}
				else
					return Value ? SocketState.StandardOn : SocketState.StandardOff;
			}
		}

		#endregion

		#region Commands

		/// <summary>
		/// Command which handles clicks on <see cref="Socket"/>s. If the app is in the
		/// <see cref="AppState.Idle"/> state, it will begin Node Pairing. If the app is in the
		/// <see cref="AppState.WireManipulation"/>, it will try to pair the node that invoked that state
		/// and the node that tries to end that state (i.e. the node that has been clicked).
		/// Parameter should be the socket that has been clicked.
		/// </summary>
		public ICommand SocketClickedCommand { get; private set; } =
			IoC.GetPrime<MainViewModel>().SocketClickedCommand;

		#endregion		

		#region Protected Methods

		/// <summary>
		/// Method which propagates the <see cref="InternalStateChanged"/> event from
		/// node to this Socket
		/// </summary>
		protected void NodeInternalStateChanged(object sender)
		{			
			InternalStateChanged?.Invoke(this);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Method which fires OnPropertyChanged for X and Y properties whenever one of Coord determining
		/// this socket's position changes
		/// </summary>
		/// <param name="sender"></param>
		private void PositionChanged(object sender) => OnPropertyChanged("X", "Y");

		#endregion
	}
}