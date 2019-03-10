using CSharpEnhanced.Maths;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Toggle input (switches between positive and negative using the provided command)
	/// </summary>
	public class ToggleInput : BaseInput
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public ToggleInput() : base("Toggle Input")
		{			
			ChangeStateCommand = new RelayCommand(ChangeState);
		}

		#endregion	

		#region Commands

		/// <summary>
		/// Command which changes the state of this input part
		/// </summary>
		public ICommand ChangeStateCommand { get; set; }

		#endregion

		#region Command Methods

		/// <summary>
		/// Method for <see cref="ChangeStateCommand"/>
		/// </summary>
		private void ChangeState() => Output.Value = !Output.Value;

		#endregion
	}
}