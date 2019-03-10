using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
    public class PartEditSectionViewModel : BaseViewModel
    {
		#region Constructor

		/// <summary>
		/// Default constructor, assigns commands
		/// </summary>
		public PartEditSectionViewModel()
		{
			StopEditingCommand = new RelayCommand(StopEditing);
		}

		#endregion

		#region Events

		/// <summary>
		/// Event fired whenever edited part changes
		/// </summary>
		public EditedPartChangedEventHandler EditedPartChangedEvent;

		#endregion

		#region Private Members

		/// <summary>
		/// View model for the currently presented part (or null if nothing is presented)
		/// </summary>
		BasePartEditViewModel mCurrentEditMenuViewModel;

		#endregion

		#region Public Properties

		#region Modifiable

		/// <summary>
		/// View model for the currently presented part (or null if nothing is presented)
		/// </summary>
		public BasePartEditViewModel CurrentEditMenuViewModel
		{
			get => mCurrentEditMenuViewModel;
			set
			{
				// If the value is identical, return
				if(mCurrentEditMenuViewModel?.EditedPartID == value?.EditedPartID)
				{
					if(value!=null)
					{
						EditedPartChangedEvent?.Invoke(this, new EditedPartChangedEventArgs(EditedPartChangeType.TheSame));
					}

					return;
				}

				EditedPartChangeType changeType;

				// Determine the change type
				if(mCurrentEditMenuViewModel == null)
				{
					// mCurrentEditViewModel and value can't be null at the same time due to the check above
					changeType = EditedPartChangeType.NullToPart;
				}
				else if(value == null)
				{
					changeType = EditedPartChangeType.PartToNull;
				}
				else
				{
					changeType = EditedPartChangeType.PartToPart;
				}

				// Assign the value
				mCurrentEditMenuViewModel = value;

				// Fire the event
				EditedPartChangedEvent?.Invoke(this, new EditedPartChangedEventArgs(changeType));
			}
		}

		/// <summary>
		/// Getter to the header of the edit menu (generic header if <see cref="CurrentEditMenuViewModel"/> is null or
		/// current part's display name)
		/// </summary>
		public string HeaderName => CurrentEditMenuViewModel == null ? "Part edit menu" : CurrentEditMenuViewModel.DisplayName;

		/// <summary>
		/// Getter to the rotate left command from the edited part
		/// </summary>
		public ICommand RotateLeftCommand => CurrentEditMenuViewModel?.RotateLeftCommand;

		/// <summary>
		/// Getter to the rotate right command from the edited part
		/// </summary>
		public ICommand RotateRightCommand => CurrentEditMenuViewModel?.RotateRightCommand;

		/// <summary>
		/// Removes the currently edited part from the list of parts and consequently deletes it
		/// </summary>
		public ICommand RemoveCommand => CurrentEditMenuViewModel?.RemoveCommand;

		/// <summary>
		/// Command which stops editing the current part (removes its edit view model)
		/// </summary>
		public ICommand StopEditingCommand { get; private set; }

		/// <summary>
		/// When true, generic buttons (those that are present for every part, for example rotation buttons) should be enabled
		/// </summary>
		public bool EnableGenericButtons => CurrentEditMenuViewModel != null;

		#endregion

		#region Texts

		/// <summary>
		/// Header for rotation section of the menu
		/// </summary>
		public string RotationSectionHeader => "Rotation";

		#endregion

		#endregion

		#region Private Methods

		/// <summary>
		/// Stops editing the current part
		/// </summary>
		private void StopEditing() => CurrentEditMenuViewModel = null;

		#endregion
	}
}
