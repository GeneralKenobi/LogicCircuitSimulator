using System;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
	public class BasePartEditViewModel : BaseViewModel
    {
		#region Protected Properties

		/// <summary>
		/// Part that is represented by this ViewModel
		/// </summary>
		protected BasePart EditedPart { get; private set; }

		#endregion

		#region Public Properties

		/// <summary>
		/// Getter to the display name of the edited part
		/// </summary>
		public string DisplayName => EditedPart.DisplayName;

		/// <summary>
		/// ID of the currently edited part
		/// </summary>
		public int EditedPartID => EditedPart == null ? -1 : EditedPart.ID;

		#endregion

		#region Constructor

		/// <summary>
		/// Default constructor which takes as an argument the part that
		/// is supposed to be edited
		/// </summary>
		/// <param name="part"></param>
		public BasePartEditViewModel(BasePart part)
		{
			EditedPart = part;
			RemoveCommand = new RelayCommand(Remove);
		}

		#endregion

		/// <summary>
		/// Getter to the rotate left command from the edited part
		/// </summary>
		public ICommand RotateLeftCommand => EditedPart.RotateLeftCommand;

		/// <summary>
		/// Getter to the rotate right command from the edited part
		/// </summary>
		public ICommand RotateRightCommand => EditedPart.RotateRightCommand;

		/// <summary>
		/// Removes the currently edited part from the list of parts and consequently deletes it
		/// </summary>
		public ICommand RemoveCommand { get; private set; }

		#region Command Methods

		/// <summary>
		/// Removes the edited part from the list of parts and consequently deletes it
		/// </summary>
		private void Remove() => EditedPart.Remove();

		#endregion
	}
}
