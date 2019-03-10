using LogicCircuitSimulator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace LogicCircuitSimulator
{
	public class PartEditViewModelToControlConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if(value is BasePartEditViewModel viewModel)
			{
				Dictionary<Type, int> typeDictionary = new Dictionary<Type, int>()
				{
					{typeof(MultiInputGateEditViewModel), 0},
					{typeof(ClockInputEditViewModel), 1},
					{typeof(ButtonInputEditViewModel), 2},
					{typeof(LatchEditViewModel), 3},
				};

				UserControl editControl = null;

				if (typeDictionary.ContainsKey(viewModel.GetType()))
				{
					switch (typeDictionary[viewModel.GetType()])
					{
						case 0:
							{
								editControl = new MultiInputGateEdit();
							}
							break;

						case 1:
							{
								editControl = new ClockInputEdit();
							}
							break;

						case 2:
							{
								editControl = new ButtonInputEdit();
							}
							break;

						case 3:
							{
								editControl = new LatchEdit();
							}
							break;
					}
				}

				if (editControl != null)
				{
					editControl.DataContext = viewModel;
				}

				return editControl;
			}


			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
