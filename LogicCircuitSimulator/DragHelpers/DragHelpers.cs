using LogicCircuitSimulator.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace LogicCircuitSimulator
{

	/// <summary>
	/// Static class with helper mathods for drag events
	/// </summary>
	public static class DragHelpers
	{
		/// <summary>
		/// Sets given object as the data with the given ID in the given DataPackage
		/// </summary>
		/// <param name="dataPackage">DataPackage to set the data in</param>
		/// <param name="ID">ID to assign the data to</param>
		/// <param name="obj">Object to assign</param>
		public static void SetData(DataPackage dataPackage, string ID, object obj) =>
			dataPackage.SetData(ID, MemoryStreamHelpers.Serialize(obj).AsRandomAccessStream());

		/// <summary>
		/// Gets the Data of the type T assigned to the package with the given ID.
		/// Doesn't catch exceptions resulting in wrong ID/type etc.
		/// </summary>
		/// <typeparam name="T">Type of the object to obtain</typeparam>
		/// <param name="dataPackage">Data package to obtain it from</param>
		/// <param name="ID">ID of the object</param>
		/// <returns></returns>
		public async static Task<T> GetData<T>(DataPackageView dataPackage, string ID) =>
				MemoryStreamHelpers.Deserialize<T>((MemoryStream)
					(await dataPackage.GetDataAsync(ID) as IRandomAccessStream).AsStream());
	}
}
