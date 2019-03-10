using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Edit view model for all latches, allows to edit 
	/// </summary>
	public class LatchEditViewModel : BasePartEditViewModel
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="part"></param>
		public LatchEditViewModel(LatchBaseStructure part) : base(part) { }

		#endregion

		#region Protected Properties

		/// <summary>
		/// Casted edited part
		/// </summary>
		protected new LatchBaseStructure EditedPart => (LatchBaseStructure)base.EditedPart;

		#endregion

		#region Public Properties

		#region Modifiable Properties

		/// <summary>
		/// Property to bind to which controls the negate clock setting in the latch
		/// </summary>
		public bool NegateClock
		{
			get => EditedPart.NegateClock;
			set => EditedPart.NegateClock = value;
		}

		/// <summary>
		/// Property to bind to which controls the negate set setting in the latch
		/// </summary>
		public bool NegateSet
		{
			get => EditedPart.NegateSet;
			set => EditedPart.NegateSet= value;
		}

		/// <summary>
		/// Property to bind to which controls the negate reset setting in the latch
		/// </summary>
		public bool NegateReset
		{
			get => EditedPart.NegateReset;
			set => EditedPart.NegateReset = value;
		}

		/// <summary>
		/// Property to bind to which controls the dominant set setting in the latch
		/// </summary>
		public bool DominantSet
		{
			get => EditedPart.DominantSet;
			set => EditedPart.DominantSet = value;
		}

		#endregion

		#region Texts

		/// <summary>
		/// Header for the latch edit section
		/// </summary>
		public string Header => "Latch Settings";

		/// <summary>
		/// Text for button assigned to <see cref="NegateClock"/>
		/// </summary>
		public string NegateClockText => "Negate Clock";

		/// <summary>
		/// Text for button assigned to <see cref="NegateSet"/>
		/// </summary>
		public string NegateSetText => "Negate Set";

		/// <summary>
		/// Text for button assigned to <see cref="NegateReset"/>
		/// </summary>
		public string NegateResetText => "Negate Reset";

		/// <summary>
		/// Text for button assigned to <see cref="DominantSet"/>
		/// </summary>
		public string DominantSetText => "Dominant Set";

		#endregion

		#endregion
	}
}
