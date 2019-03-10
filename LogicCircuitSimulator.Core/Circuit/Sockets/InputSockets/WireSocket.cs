using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Wire socket is a modified InputSocket
	/// </summary>
    public class WireSocket : InputSocket
    {
		new public ICommand SocketClickedCommand { get; set; } =
			IoC.GetPrime<MainViewModel>().WireSocketClickedCommand;

		public int WireID { get; set; } = -1;
    }
}
