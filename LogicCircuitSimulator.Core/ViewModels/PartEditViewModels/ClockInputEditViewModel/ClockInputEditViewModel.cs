using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
    public class ClockInputEditViewModel : BasePartEditViewModel
    {
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="partToEdit"></param>
		public ClockInputEditViewModel(ClockInput partToEdit) : base(partToEdit)
		{

		}

		#endregion

		#region Protected Properties

		/// <summary>
		/// Casted getter to the edited part
		/// </summary>
		new protected ClockInput EditedPart => (ClockInput)base.EditedPart;

		#endregion

		#region Public Properties

		#region Modifiable Properties

		/// <summary>
		/// Period of the edited clock (in ms)
		/// </summary>
		public int Period
		{
			get => EditedPart.Period;
			set
			{
				// Minimum period is 50ms
				if(value < ClockInput.MinimumPeriod)
				{
					value = ClockInput.MinimumPeriod;
				}

				// Maximum period is 5000ms
				if(value > ClockInput.MaximumPeriod)
				{
					value = ClockInput.MaximumPeriod;
				}

				// Assign the value
				EditedPart.Period = value;

				// Notify the property changed (in case the value had to be adjusted)
				DelayedOnPropertyChanged(nameof(Period));
			}
		}

		#endregion

		#region Texts

		/// <summary>
		/// Header of this edit section
		/// </summary>
		public string Header => "Period [ms]";

		#endregion

		#endregion
	}
}