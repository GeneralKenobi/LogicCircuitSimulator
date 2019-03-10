using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace LogicCircuitSimulator.Core
{
	/// <summary>
	/// Exponential socket collection, number of sockets is 2^size
	/// </summary>
	public class ExpSocketCollection<T> : BaseViewModel
		where T : Socket, new()
	{
		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public ExpSocketCollection()
		{
			Size = MinimumSize;
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Default size is 1
		/// </summary>
		private int mSize = 0;

		/// <summary>
		/// By default there are 2 sockets in the collection
		/// </summary>
		private readonly ObservableCollection<T> mSockets = new ObservableCollection<T>();

		#endregion

		#region Public Members

		/// <summary>
		/// Event fired when internal state changes in one of the sockets
		/// </summary>
		public InternalStateChangedEventHandler InternalStateChanged;

		#endregion

		#region Public Properties

		/// <summary>
		/// Getter to the sockets, copies the collection and returns a copy. Changing size is possible only
		/// using the <see cref="Size"/> property
		/// </summary>
		public ObservableCollection<T> Sockets => new ObservableCollection<T>(mSockets);

		/// <summary>
		/// Size of this collection, number of sockets is 2^size
		/// </summary>
		public int Size
		{
			get => mSize;
			set
			{
				// Check if the new value isn't equal to the current one
				if (value == mSize)
				{
					return;
				}

				// Check for the minimum value
				if (value < MinimumSize)
				{
					value = MinimumSize;
				}

				// If the new value is smaller than the current one
				if (value < mSize)
				{
					// Remove sockets until there's 2^value
					while (mSockets.Count > Math.Pow(2, value))
					{
						mSockets[mSockets.Count - 1].InternalStateChanged -= PropagateInternalStateChanged;
						mSockets.RemoveAt(mSockets.Count - 1);
					}
				}
				else
				{
					// Otherwise add sockets until there's 2^value
					while (mSockets.Count < Math.Pow(2, value))
					{
						var newSocket = new T();
						newSocket.InternalStateChanged += PropagateInternalStateChanged;
						mSockets.Add(newSocket);
						
					}
				}

				// Assign the size and notify about property changed
				mSize = value;
				OnPropertyChanged(nameof(Size), nameof(Sockets));
			}
		}

		#endregion

		#region Public Static Properties

		/// <summary>
		/// Minimum size of the collection
		/// </summary>
		int MinimumSize => 1;

		#endregion

		#region Private Members

		/// <summary>
		/// Propagates internal state changed from sockets
		/// </summary>
		/// <param name="sender"></param>
		private void PropagateInternalStateChanged(object sender) => InternalStateChanged?.Invoke(sender);

		#endregion

		#region Operators

		/// <summary>
		/// Getter to the i-th socket in the collection
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public T this[int i] => mSockets[i];

		/// <summary>
		/// Increments the size of the collection
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static ExpSocketCollection<T> operator++(ExpSocketCollection<T> c)
		{
			++c.Size;
			return c;
		}

		/// <summary>
		/// Decrements the size of the collection
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static ExpSocketCollection<T> operator --(ExpSocketCollection<T> c)
		{
			--c.Size;
			return c;
		}

		#endregion
	}
}
