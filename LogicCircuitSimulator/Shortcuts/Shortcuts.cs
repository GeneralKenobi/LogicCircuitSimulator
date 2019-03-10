using LogicCircuitSimulator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace LogicCircuitSimulator
{
	/// <summary>
	/// Class handling events that may trigger a shortcut action
	/// </summary>
	public static class Shortcuts
	{
		/// <summary>
		/// If a key has a shortcut, invokes a command for that key.
		/// Currently handled keys:
		/// Escape - break current action and return to idle state
		/// </summary>
		/// <param name="key"></param>
		public static void HandleKey(VirtualKey key)
		{
			switch(key)
			{
				case VirtualKey.Escape:
					{
						IoC.GetPrime<MainViewModel>().ReturnToIdleCommand.Execute(null);
					}
					break;
			}
		}
	}
}
