using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Container which stores all nodes and all objects that hold reference to those nodes
	/// </summary>
    public static class NodeReferenceDictionary
    {
		#region Private Members

		/// <summary>
		/// Internal dictionary which holds all references made to a <see cref="Node"/>
		/// </summary>
		private static readonly ConcurrentDictionary<int, List<int>> mNodeReferences =
			new ConcurrentDictionary<int, List<int>>();		

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds a part which references the node to the collection
		/// </summary>
		/// <param name="node">Node that is referenced</param>
		/// <param name="socket">Socket that it is referenced by</param>
		public static void AddReference(Node node, Socket socket)
		{
			if(mNodeReferences.ContainsKey(node.ID))
			{
				mNodeReferences[node.ID].Add(socket.ID);
			}
			else
			{				
				mNodeReferences.TryAdd(node.ID, new List<int>() { socket.ID });
			}
		}

		/// <summary>
		/// Removes a single entry from the collection. Returns true if the entry was found
		/// and deleted.
		/// </summary>
		/// <param name="nodeID">ID of the node we're looking for</param>
		/// <param name="socketID">ID of the socket which no longer references it</param>
		/// <returns></returns>
		public static bool RemoveReference(int nodeID, int socketID)
		{
			// If the entry is in the dictionary
			if(mNodeReferences.ContainsKey(nodeID))
			{
				// Try to get the owner we're looking for and check
				// if it's an InputSocket
				if(IDManager.TryGetIDOwner(socketID, out Identifiable owner) &&
					owner is InputSocket socket)				
				{
					// If so, remove it's reference to the node
					socket.SetNode(null);

					// And remove that entry from the dictionary
					mNodeReferences[nodeID].Remove(socketID);

					// If it was the last entry for that node, remove it's entry
					// in the dictionary
					if(mNodeReferences[nodeID].Count==0)
					{
						mNodeReferences.TryRemove(nodeID, out List<int> removedList);
					}

					return true;
				}				
			}

			return false;
		}

		/// <summary>
		/// Removes references to this node in all parts that reference it
		/// and removes the entry for that node
		/// </summary>
		/// <param name="nodeID">ID of the node whose references are to be deleted</param>
		public static void RemoveAllReferences(int nodeID)
		{
			if(mNodeReferences.TryGetValue(nodeID, out List<int> list))
			{				
				for(int i=0; i<list.Count; ++i)
				{
					if(IDManager.TryGetIDOwner(list[i], out Identifiable result) &&
						result is InputSocket socket)
					{
						socket.SetNode(null);
					}
				};

				mNodeReferences.TryRemove(nodeID, out List<int> removedList);
			}
		}

		/// <summary>
		/// Performs the given action on all nodes
		/// </summary>
		/// <param name="action">Action to perform</param>
		public static void ForEach(Action<Node> action)
		{
			foreach(var item in mNodeReferences.Keys)
			{
				if(IDManager.TryGetIDOwner(item, out Node owner))
				{
					action.Invoke(owner);
				}
			}			
		}

		/// <summary>
		/// Performs the given action on all nodes that result in true for the given predicate
		/// </summary>
		/// <param name="action">Action to perform</param>
		/// <param name="predicate">Predicate to test on nodes</param>
		public static void ForEach(Action<Node> action, Predicate<Node> predicate)
		{
			foreach (var item in mNodeReferences.Keys)
			{
				if (IDManager.TryGetIDOwner(item, out Identifiable owner) &&
					owner is Node node && predicate(node))
				{
					action.Invoke(node);
				}
			}
		}

		#endregion
	}
}
