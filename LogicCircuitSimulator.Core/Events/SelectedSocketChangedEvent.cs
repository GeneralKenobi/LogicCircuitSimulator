using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Arguments of <see cref="SelectedSocketChangedEventHandler"/>, contain old and new value
	/// </summary>
	public class SelectedSocketChangedEventArgs : EventArgs
	{
		#region Constructor

		/// <summary>
		/// Default constuctor that takes 2 arguments
		/// </summary>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		public SelectedSocketChangedEventArgs(Socket oldValue, Socket newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Unassigned value
		/// </summary>
		public Socket OldValue { get; private set; }

		/// <summary>
		/// New assigned socket
		/// </summary>
		public Socket NewValue { get; private set; }

		#endregion
	}

	/// <summary>
	/// Event for when a socket changes, designed with MUX/DMUX in mind
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	public delegate void SelectedSocketChangedEventHandler(object sender, SelectedSocketChangedEventArgs args);
}
