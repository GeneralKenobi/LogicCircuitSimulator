using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
    public class MultiInputGateEditViewModel : BasePartEditViewModel
    {
		#region Constructor

		/// <summary>
		/// Default constructor, requires a <see cref="MultiInputGate"/> which
		/// it will represent.
		/// </summary>
		/// <param name="gate"></param>
		public MultiInputGateEditViewModel(MultiInputGate gate) : base(gate)
		{
			// Set the commands
			AddInputCommand = new RelayCommand(AddInput);
			RemoveInputCommand = new RelayCommand(RemoveInput);
		}

		#endregion

		#region Commands

		/// <summary>
		/// Command which adds a new input if it's possible (resulting number of inputs isn't greater than <see cref=""/>
		/// </summary>
		public ICommand AddInputCommand { get; private set; }

		/// <summary>
		/// Command which removes an input if it's possible (resulting number of inputs is at least 1)
		/// </summary>
		public ICommand RemoveInputCommand { get; private set; }

		#endregion
		
		#region Protected Properties

		/// <summary>
		/// Maximum number of inputs in a MultiInputGate
		/// </summary>
		protected static int MaximumNumberOfInputs => 21;

		/// <summary>
		/// Casted getter to the edited part
		/// </summary>
		new protected MultiInputGate EditedPart => (MultiInputGate)base.EditedPart;

		#endregion

		#region Public Properties

		#region Modifiable Properties

		/// <summary>
		/// Property which allows to change the number of inputs the edited gate has.
		/// </summary>
		public int NumberOfInputs
		{
			get => EditedPart.Inputs.Count;
			set
			{
				// If the new value is smaller than 1, make it 1
				if (value < 1)
				{
					value = 1;
				}

				// Maximum number of inptus for a part
				if(value > MaximumNumberOfInputs)
				{
					value = MaximumNumberOfInputs;				
				}

				int oldSize = EditedPart.Inputs.Count;

				// If the new value is bigger than the current number of inputs
				if (value > EditedPart.Inputs.Count)
				{					
					// Add new inputs to match the requested value
					for (int i = 0; i < (value - oldSize); ++i)
					{						
						EditedPart.Inputs.Add(new InputSocket());
					}
				}
				else
				{
					// Else remove the inputs to match the requested value
					for (int i = 0; i < (oldSize - value); ++i)
					{
						EditedPart.Inputs.RemoveAt(EditedPart.Inputs.Count - 1);						
					}
				}

				// Notify that the property changed (it's often that the value will be changed using the Add/Remove buttons
				// or it will be adjusted because it was nonpositive or too large)
				DelayedOnPropertyChanged(nameof(NumberOfInputs));
			}
		}

		/// <summary>
		/// Property to bind to which determines when to enable Add input button
		/// </summary>
		public bool EnableAddButton => NumberOfInputs < 21;

		/// Property to bind to which determines when to enable Remove input button
		public bool EnableRemoveButton => NumberOfInputs > 1;

		#endregion

		#region Texts

		/// <summary>
		/// Header for this part's section
		/// </summary>
		public string Header => "Number of Inputs";

		/// <summary>
		/// Text for a button which adds inputs
		/// </summary>
		public string AddInputButtonText => "Add";

		/// <summary>
		/// Text for a button which removes inputs
		/// </summary>
		public string RemoveInputButtonText => "Remove";

		#endregion

		#endregion

		#region Private Methods

		/// <summary>
		/// Helper for <see cref="NumberOfInputs"/> setter, after delay of 10ms fires property changed for NumberOfInputs
		/// If it's fired from the setter, it won't work that's why it's fired directly outside.
		/// It's necessary if the value was changed using a button or if it was adjusted because it was nonpositive / too large
		/// </summary>
		/// <returns></returns>
		private async Task DelayNumberOfInputsPropertyChanged()
		{
			await Task.Delay(10);
			OnPropertyChanged(nameof(NumberOfInputs));
		}

		#endregion

		#region Command Methods

		/// <summary>
		/// Method for <see cref="AddInputCommand"/>, if possible (resulting number of inputs is smaller than 22) adds a new input
		/// </summary>
		private void AddInput() => NumberOfInputs = NumberOfInputs + 1;

		/// <summary>
		/// Method for <see cref="RemoveInputCommand"/>, if possible (resulting number of inputs is greater than 0) removes the last input
		/// </summary>
		private void RemoveInput() => NumberOfInputs = NumberOfInputs - 1;

		#endregion
	}
}