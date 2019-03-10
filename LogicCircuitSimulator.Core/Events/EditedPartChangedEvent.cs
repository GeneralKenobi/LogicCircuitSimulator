using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{

	/// <summary>
	/// Enum denoting type of change of the edited part
	/// </summary>
	public enum EditedPartChangeType
	{
		/// <summary>
		/// Old value was null, new is a part
		/// </summary>
		NullToPart = 0,

		/// <summary>
		/// Old value was a part, new is null
		/// </summary>
		PartToNull = 1,

		/// <summary>
		/// Old value was a part, new is a different part
		/// </summary>
		PartToPart = 2,

		/// <summary>
		/// Old value is the same as new (but it's not null)
		/// </summary>
		TheSame = 3,
	}


	public class EditedPartChangedEventArgs : EventArgs
	{
		#region Public Properties

		/// <summary>
		/// Type of change that occured
		/// </summary>
		public EditedPartChangeType ChangeType { get; private set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor with a parameter
		/// </summary>
		/// <param name="changeType"></param>
		public EditedPartChangedEventArgs(EditedPartChangeType changeType)
		{
			ChangeType = changeType;
		}

		#endregion
	}


	/// <summary>
	/// Event fired when edited part changes in the <see cref="PartEditSectionViewModel"/>
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	public delegate void EditedPartChangedEventHandler(object sender, EditedPartChangedEventArgs args);
}
