using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Input which is turned on for a brief moment upon being pressed
	/// </summary>
    public class ButtonInput : BaseInput
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public ButtonInput() : base("Button")
		{
			PressButtonCommand = new RelayCommand(PressButton);
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Setting this flag will prevent this object from starting more pulses
		/// </summary>
		private bool mDisposeStarted = false;

		/// <summary>
		/// Duration of the pulse upon pressing the button
		/// </summary>
		private int mPulse = 50;

		/// <summary>
		/// Dictionary in which tasks mark their pulses
		/// </summary>
		private readonly ConcurrentDictionary<DateTime, int> mPulses = new ConcurrentDictionary<DateTime, int>();		

		#endregion

		#region Public Properties

		/// <summary>
		/// Duration of the pulse upon pressing the button
		/// </summary>
		public int Pulse
		{
			get => mPulse;
			set
			{
				if (value >= MinimumPulse && value <= MaximumPulse)
				{
					mPulse = value;
				}
				else if (Debugger.IsAttached)
				{
					// If it's a debuggin session and the new pulse doesn't fall in the allows bracket, break
					Debugger.Break();
				}
			}
		}

		#endregion

		#region Commands

		/// <summary>
		/// Command which triggers the button press
		/// </summary>
		public ICommand PressButtonCommand { get; private set; }

		#endregion

		#region Private Methods

		/// <summary>
		/// Method which triggers the button press
		/// </summary>
		private void PressButton() => PulseOn();

		/// <summary>
		/// Runs a pulse (turns the output on, then off)
		/// </summary>
		/// <returns></returns>
		private async Task PulseOn()
		{
			// If the disposing of this object started don't start another pulse
			if (mDisposeStarted)
			{
				return;
			}

			// Get the current time as the unique identifier for this pulse
			var time = DateTime.Now;

			// Try to add it to the mPulses dictionary 
			if (mPulses.TryAdd(time, 0))
			{
				// Set the output
				Output.Value = true;

				// Await the pulse duration
				await Task.Delay(Pulse);

				// Remove the time identifier
				mPulses.TryRemove(time, out int val);

				lock (mPulses)
				{
					// If it was a last identifier
					if (mPulses.IsEmpty)
					{
						// Reset the output
						Output.Value = false;
					}
				}
			}
		}

		#endregion

		#region Public Static Properties

		/// <summary>
		/// Minimum pulse duration for a button
		/// </summary>
		public static int MinimumPulse => 20;

		/// <summary>
		/// Maximum pulse duration for a button
		/// </summary>
		public static int MaximumPulse => 1000;

		#endregion

		#region IDisposable

		/// <summary>
		/// Waits for all pulses to end
		/// </summary>
		public async override void Dispose()
		{
			mDisposeStarted = true;

			while(!mPulses.IsEmpty)
			{
				await Task.Delay(mPulse);
			}

			base.Dispose();
		}

		#endregion
	}
}
