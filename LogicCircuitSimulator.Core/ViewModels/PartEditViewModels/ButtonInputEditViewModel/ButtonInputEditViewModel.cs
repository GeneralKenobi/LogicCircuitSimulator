using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public class ButtonInputEditViewModel : BasePartEditViewModel
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="partToEdit"></param>
		public ButtonInputEditViewModel(ButtonInput partToEdit) : base(partToEdit) {	}

		#endregion

		#region Protected Properties

		/// <summary>
		/// Casted getter to the edited part
		/// </summary>
		new protected ButtonInput EditedPart => (ButtonInput)base.EditedPart;

		#endregion

		#region Public Properties

		#region Modifiable Properties

		/// <summary>
		/// Pulse of the edited button (in ms)
		/// </summary>
		public int Pulse
		{
			get => EditedPart.Pulse;
			set
			{
				// Minimum period is 50ms
				if(value < ButtonInput.MinimumPulse)
				{
					value = ButtonInput.MinimumPulse;
				}

				// Maximum period is 5000ms
				if(value > ButtonInput.MaximumPulse)
				{
					value = ButtonInput.MaximumPulse;
				}

				// Assign the value
				EditedPart.Pulse = value;

				// Notify the property changed (in case the value had to be adjusted)
				DelayedOnPropertyChanged(nameof(Pulse));
			}
		}

		#endregion

		#region Texts

		/// <summary>
		/// Header for this edit section
		/// </summary>
		public string Header => "Pulse [ms]";

		#endregion

		#endregion
	}
}