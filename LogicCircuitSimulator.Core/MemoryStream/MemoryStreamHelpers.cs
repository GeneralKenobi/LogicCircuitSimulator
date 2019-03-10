using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Class containing helper methods for <see cref="MemoryStream"/>s
	/// </summary>
	public static class MemoryStreamHelpers
	{
		/// <summary>
		/// Serializes the given object and writes it into the returned <see cref="MemoryStream"/>
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static MemoryStream Serialize(object obj)
		{
			var stream = new MemoryStream();

			var writer = new StreamWriter(stream);

			writer.Write(JsonConvert.SerializeObject(obj));

			writer.Flush();
			stream.Position = 0;

			return stream;
		}

		/// <summary>
		/// Deserializes object of type T from the given <see cref="MemoryStream"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static T Deserialize<T>(MemoryStream stream) => (T)
			JsonSerializer.Create().Deserialize(new StreamReader(stream), typeof(T));

	}
}
