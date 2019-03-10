using System;
using System.Collections.Generic;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Class which manages IDs for parts and nodes in this app.
	/// IDs range from 0 to <see cref="int.MaxValue"/>
	/// </summary>
	public static class IDManager
    {
		#region Private Members

		/// <summary>
		/// Set containing all currently used ids
		/// </summary>
		private static readonly Dictionary<int, Identifiable> mUsedID =
			new Dictionary<int, Identifiable>();

		/// <summary>
		/// Prediction on the next ID to use. For example: we have used 0,1,2,3,4,5
		/// and we have just removed 3. mNextID == 3. After using it, mNextID will be
		/// equal to 4, so it will have to adjust, however when it is back to norm it will
		/// work properly again (after next requst for id (which will be 6), mNextID == 7)
		/// </summary>
		private static int mNextID = 0;

		#endregion

		#region Public Methods

		#region Add/Remove/Get IDs		

		/// <summary>
		/// Finds ID to assign to the object, adds the object to the internal container with that ID.
		/// If the object already has an ID, returns that ID
		/// </summary>
		/// <returns></returns>
		public static int AssignID(this Identifiable obj)
		{
			// If the object already has an ID, don't do anything
			if (obj.ID >= 0)
			{
				return obj.ID;
			}

			// Find the next free ID
			while (mUsedID.ContainsKey(mNextID))
			{
				++mNextID;
			}		

			// Add the object to the list of references
			mUsedID.Add(mNextID, obj);

			// And increase the expected next ID
			return mNextID++;
		}

		/// <summary>
		/// Frees the ID and removes the reference to it from the manager
		/// </summary>
		/// <param name="obj"></param>
		public static void FreeID(this Identifiable obj) =>
			mUsedID.Remove(obj.ID);		

		/// <summary>
		/// Tries to get owner of the given ID. Returns true on success and assigns the
		/// found object to the result. Otherwise returns false and assigns null to result.
		/// </summary>
		/// <param name="ID">ID to look for</param>
		/// <param name="owner">variable to save the result in</param>
		/// <returns></returns>
		public static bool TryGetIDOwner(int ID, out Identifiable owner) =>
			mUsedID.TryGetValue(ID, out owner);

		/// <summary>
		/// Tries to get owner of the given ID. Returns true on success and assigns the
		/// found object to the result. Otherwise returns false and assigns null to result.
		/// </summary>
		/// <param name="ID">ID to look for</param>
		/// <param name="owner">variable to save the result in</param>
		/// <returns></returns>
		public static bool TryGetIDOwner<T>(int ID, out T owner)
			where T : Identifiable
		{
			if(mUsedID.TryGetValue(ID, out Identifiable idOwner) && idOwner is T t)
			{
				owner = t;
				return true;
			}
			else
			{
				owner = default(T);
				return false;
			}
		}

		#endregion

		#region ForAll

		/// <summary>
		/// Performs the given action on all elements in the collection
		/// </summary>
		/// <param name="action">Action to perform</param>
		public static void ForAll(Action<Identifiable> action)
		{
			foreach (var item in mUsedID.Values)
			{				
				action.Invoke(item);				
			}
		}
		
		/// <summary>
		/// Performs the given action on all elements of the given type in the collection
		/// </summary>
		/// <param name="action">Action to perform</param>
		public static void ForAll<T>(Action<T> action)
			where T : Identifiable
		{
			foreach (var item in mUsedID.Values)
			{
				if (item is T castedItem)
				{
					action.Invoke(castedItem);
				}
			}
		}

		/// <summary>
		/// Performs the given action on all elements in the collection that pass the
		/// given predicate.
		/// </summary>
		/// <param name="action">Action to perform</param>
		/// <param name="predicate">Predicate which is used to determine on which
		/// elements to perform the action</param>
		public static void ForAll(Action<Identifiable> action,
			Predicate<Identifiable> predicate)
		{
			foreach (var item in mUsedID.Values)
			{
				if (predicate(item))
				{
					action.Invoke(item);
				}
			}
		}

		/// <summary>
		/// Performs the given action on all elements of the given type in the collection
		/// that pass the given predicate.
		/// </summary>
		/// <param name="action">Action to perform</param>
		/// <param name="predicate">Predicate which is used to determine on which
		/// elements to perform the action</param>
		public static void ForAll<T>(Action<T> action, Predicate<T> predicate)
			where T : Identifiable
		{
			foreach (var item in mUsedID.Values)
			{
				if (item is T castedItem && predicate(castedItem))
				{
					action.Invoke(castedItem);
				}
			}
		}

		#endregion

		#endregion
	}
}
