using CSharpEnhanced.Maths;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Clock input (switches between positive and negative after requested amount
	/// of time)
	/// </summary>
    public class ClockInput : BaseInput
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public ClockInput() : base("Clock")
		{		
			SwitchStates();
		}

		#endregion

		#region Private Members

		/// <summary>
		/// As long as this variable is true, this part will keep on swithing states
		/// using <see cref="SwitchStates"/> async task
		/// </summary>
		private bool mKeepSwitching = true;

		/// <summary>
		/// Number of miliseconds between a change in states (default is 1 second)
		/// </summary>
		private int mPeriod = 1000;

		#endregion

		#region Public Properties		

		/// <summary>
		/// Number of miliseconds between a change in states (default is 1 second)
		/// </summary>
		public int Period
		{
			get => mPeriod;
			set
			{
				if(value >= MinimumPeriod && value <= MaximumPeriod)
				{
					mPeriod = value;
				}
				else if(Debugger.IsAttached)
				{
					// If it's a debuggin session and the new period doesn't fall in the allows bracket, break
					Debugger.Break();
				}
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Switches the state (output) of this clock every <see cref="Period"/>.
		/// Works as long as <see cref="mKeepSwitching"/> is true
		/// </summary>
		/// <returns></returns>
		private async Task SwitchStates()
		{
			while(mKeepSwitching)
			{
				await Task.Delay(Period);
				Output.Value = !Output.Value;
			}
		}

		#endregion				

		#region IDisposable

		/// <summary>
		/// Method which disposes of this part (makes sure all output nodes are
		/// removed from all clients)
		/// </summary>
		public override void Dispose()
		{
			mKeepSwitching = false;
			base.Dispose();
		}

		#endregion

		#region Public Static Properties

		/// <summary>
		/// Minimum period which can be assigned to a clock
		/// </summary>
		public static int MinimumPeriod => 50;

		/// <summary>
		/// Maximum period which can be assigned to a clock
		/// </summary>
		public static int MaximumPeriod => 5000;

		#endregion
	}
}
