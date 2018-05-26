using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CSharpEnhanced.Collections
{
	/// <summary>
	/// <see cref="SortedSet{T}"/> with PropertyChanged and 
	/// CollectionChanged events
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ObservableSortedSet<T> : ICollection<T>,
		ISet<T>,
		INotifyPropertyChanged,
		INotifyCollectionChanged
	{
		#region Private Members

		/// <summary>
		/// Private set used to store the values in this collection
		/// </summary>
		private readonly SortedSet<T> mCoreSet = new SortedSet<T>();

		#endregion

		#region Public Properties

		/// <summary>
		/// Number of elements in the collection
		/// </summary>
		public int Count => mCoreSet.Count;

		/// <summary>
		/// Is read only
		/// </summary>
		public bool IsReadOnly => false;

		#endregion

		#region Events

		/// <summary>
		/// Event fired whenever a property is changed inside this collection
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Event fired whenever the collection is modified
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		#endregion

		#region Raise Events

		/// <summary>
		/// Raises <see cref="CollectionChanged"/> event
		/// </summary>
		private void RaiseCollectionChangedEvent()
		{
			CollectionChanged?.Invoke(this,
				new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Reset));
		}

		/// <summary>
		/// Raises all PropertyChanged event for:
		/// "Item[]", "Count"
		/// </summary>
		private void RaisePropertyChangedEvents() =>
			RaisePropertyChangedEvents("Item[]", "Count");

		/// <summary>
		/// Raises PropertyChanged event for each given property
		/// </summary>
		/// <param name="properties">Properties to raise PropertyChanged event for</param>
		private void RaisePropertyChangedEvents(params string[] properties)
		{
			foreach (var item in properties)
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(item));
			}
		}

		#endregion

		#region Addition of elements

		/// <summary>
		/// Adds a new element to the collection
		/// </summary>
		/// <param name="item">Item to add</param>
		/// <returns></returns>
		public bool Add(T item)
		{
			if (mCoreSet.Add(item))
			{
				RaiseCollectionChangedEvent();
				RaisePropertyChangedEvents();
				return true;
			}

			return false;
		}

		/// <summary>
		/// Adds a new item to the collection
		/// </summary>
		/// <param name="item"></param>
		void ICollection<T>.Add(T item) => Add(item);

		#endregion

		#region Removal of items

		/// <summary>
		/// Tries to remova an item from the set.
		/// </summary>
		/// <param name="item">Item to remove</param>
		/// <returns></returns>
		public bool Remove(T item)
		{
			if (mCoreSet.Remove(item))
			{
				RaiseCollectionChangedEvent();
				RaisePropertyChangedEvents();
				return true;
			}

			return false;
		}

		/// <summary>
		/// Clears the collection
		/// </summary>
		public void Clear()
		{
			mCoreSet.Clear();
			RaisePropertyChangedEvents();
			RaiseCollectionChangedEvent();
		}

		/// <summary>
		/// Removes all elements that are in a specified collection from the
		/// current <see cref="ObservableSortedSet{T}"/>
		/// </summary>
		/// <param name="other"></param>
		public void ExceptWith(IEnumerable<T> other) =>
			mCoreSet.ExceptWith(other);
		
		#endregion

		#region Contains

		/// <summary>
		/// Returns true if the collection contains the given item
		/// </summary>
		/// <param name="item">Item to look for</param>
		/// <returns></returns>
		public bool Contains(T item) =>
			mCoreSet.Contains(item);

		#endregion

		#region Copying

		/// <summary>
		/// Copies elements from the collection to the given array
		/// </summary>
		/// <param name="array">Array to copy to</param>
		/// <param name="arrayIndex">Index to start the copying at (in the array)</param>
		public void CopyTo(T[] array, int arrayIndex) =>
			mCoreSet.CopyTo(array, arrayIndex);

		#endregion

		#region Enumerator

		/// <summary>
		/// Returns an enumerator that iterates through the
		/// <see cref="ObservableSortedSet{T}"/>
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator() => mCoreSet.GetEnumerator();

		/// <summary>
		/// Returns an enumerator that iterates through the
		/// <see cref="ObservableSortedSet{T}"/>
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator() => mCoreSet.GetEnumerator();

		#endregion

		#region Intersect

		/// <summary>
		/// Modifies the current <see cref="ObservableSortedSet{T}"/> object so that it
		/// contains only elements that are also in a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current
		/// <see cref="ObservableSortedSet{T}"/> object.</param>
		public void IntersectWith(IEnumerable<T> other) =>
			mCoreSet.IntersectWith(other);

		#endregion

		#region Set relations

		/// <summary>
		/// Determines whether a <see cref="ObservableSortedSet{T}"/> object is a proper
		/// subset of the specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current
		/// <see cref="ObservableSortedSet{T}"/> object.</param>
		/// <returns></returns>
		public bool IsProperSubsetOf(IEnumerable<T> other) =>
			mCoreSet.IsProperSubsetOf(other);

		/// <summary>
		/// Determines whether a <see cref="ObservableSortedSet{T}"/> object is a proper
		/// superset of the specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current
		/// <see cref="ObservableSortedSet{T}"/>object.</param>
		/// <returns></returns>
		public bool IsProperSupersetOf(IEnumerable<T> other) =>
			mCoreSet.IsProperSupersetOf(other);

		/// <summary>
		/// Determines whether a <see cref="ObservableSortedSet{T}"/> object is a subset
		/// of the specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current
		/// <see cref="ObservableSortedSet{T}"/> object.</param>
		/// <returns></returns>
		public bool IsSubsetOf(IEnumerable<T> other) =>
			mCoreSet.IsSubsetOf(other);

		/// <summary>
		/// Determines whether a <see cref="ObservableSortedSet{T}"/> object is a superset
		/// of the specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current
		/// <see cref="ObservableSortedSet{T}"/> object.</param>
		/// <returns></returns>
		public bool IsSupersetOf(IEnumerable<T> other) =>
			mCoreSet.IsSupersetOf(other);

		/// <summary>
		/// Determines whether the current <see cref="ObservableSortedSet{T}"/> object
		/// and a specified collection share common elements.
		/// </summary>
		/// <param name="other">The collection to compare to the current
		/// <see cref="ObservableSortedSet{T}"/> object.</param>
		/// <returns></returns>
		public bool Overlaps(IEnumerable<T> other)
		{
			return mCoreSet.Overlaps(other);
		}

		/// <summary>
		/// Determines whether the current <see cref="ObservableSortedSet{T}"/>
		/// and the specified collection contain the same elements.
		/// </summary>
		/// <param name="other">The collection to compare to the current
		/// <see cref="ObservableSortedSet{T}"/> object.</param>
		/// <returns></returns>
		public bool SetEquals(IEnumerable<T> other) =>
			mCoreSet.SetEquals(other);
		
		/// <summary>
		/// Modifies the current <see cref="ObservableSortedSet{T}"/> object so that it
		/// contains only elements that are present either in the current object or in the
		/// specified collection, but not both.
		/// </summary>
		/// <param name="other">The collection to compare to the current
		/// <see cref="ObservableSortedSet{T}"/> object.</param>
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			mCoreSet.SymmetricExceptWith(other);
			RaiseCollectionChangedEvent();
			RaisePropertyChangedEvents();
		}

		/// <summary>
		/// Modifies the current <see cref="ObservableSortedSet{T}"/> object so that it
		/// contains all elements that are present in either the current object or the specified
		/// collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current
		/// <see cref="ObservableSortedSet{T}"/> object.</param>
		public void UnionWith(IEnumerable<T> other)
		{
			mCoreSet.UnionWith(other);
			RaiseCollectionChangedEvent();
			RaisePropertyChangedEvents();
		}

		#endregion
	}
}