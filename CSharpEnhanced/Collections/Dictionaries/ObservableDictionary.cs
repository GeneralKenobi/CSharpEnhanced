using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace CSharpEnhanced.Collections
{
	/// <summary>
	/// Standard Dictionary except it notifies when its property is changed or when
	/// a key/value inside the property changes	
	/// </summary>
	/// <typeparam name="TKey">Type of the Key used in the Dictionary</typeparam>
	/// <typeparam name="TValue">Type of the value used in the Dictionary</typeparam>
	public class ObservableDictionary<TKey, TValue> :
	ICollection<KeyValuePair<TKey, TValue>>,
	IDictionary<TKey, TValue>,
	INotifyCollectionChanged,
	INotifyPropertyChanged
	{
		#region Private Members

		/// <summary>
		/// The backing collection for this dictionary
		/// </summary>
		private readonly ConcurrentDictionary<TKey, TValue> mCoreDictionary =
			new ConcurrentDictionary<TKey, TValue>();	

		#endregion

		#region Public Properties

		/// <summary>
		/// Property which decides whether an exception will be thrown if
		/// a duplicate <see cref="TKey"/> element is added.
		/// By default it is true.
		/// If it's set to false, the element won't be added and exception won't be thrown.
		/// </summary>
		public bool ThrowExceptionForDuplicates { get; set; } = true;

		/// <summary>
		/// Number of elements in the collection
		/// </summary>
		public int Count => mCoreDictionary.Count;

		public bool IsReadOnly => false;

		/// <summary>
		/// Returns a List of Keys
		/// </summary>
		public ICollection<TKey> Keys => mCoreDictionary.Keys;

		/// <summary>
		/// Returns a List of Values
		/// </summary>
		public ICollection<TValue> Values => mCoreDictionary.Values;		

		#endregion

		#region Events

		/// <summary>
		/// Event for when the collection changes
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>
		/// Event for when a property inside the collection changes
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Raising Events

		/// <summary>
		/// Raises proper events when a new entry is added
		/// </summary>
		/// <param name="newItem"></param>
		private void RaiseEventsItemAdded(DictionaryEntry newItem)
		{
			// TODO: Make it work for Add Action (this snipper addes new item, but the binding
			// only says DictionaryEntry
			//CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
			//	NotifyCollectionChangedAction.Add, newItem));

			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
				NotifyCollectionChangedAction.Reset));

			RaisePropertyChangedEvents();
		}

		/// <summary>
		/// Raises proper events when an entry is modified
		/// </summary>
		/// <param name="oldItem">The modified item before change</param>
		/// <param name="newItem">The modified item after change</param>
		private void RaiseEventsItemModified(object oldItem, object newItem)
		{
			// TODO: make it work with Replace action
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
				NotifyCollectionChangedAction.Reset));

			RaisePropertyChangedEvents("Item[]", "Values");
		}

		/// <summary>
		/// Raises proper events when an entry is deleted
		/// </summary>
		/// <param name="removedItem"></param>
		private void RaiseEventsItemRemoved(DictionaryEntry removedItem)
		{
			// TODO: Make it work for Remove action
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
				NotifyCollectionChangedAction.Reset));

			RaisePropertyChangedEvents();
		}

		/// <summary>
		/// Raises proper events when the collection is cleared
		/// </summary>
		private void RaiseEventsCollectionCleared()
		{
			CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
				NotifyCollectionChangedAction.Reset));

			RaisePropertyChangedEvents();
		}

		#endregion

		#region Raise PropertyChanged event

		/// <summary>
		/// Raises all PropertyChanged event for:
		/// "Item[]", "Count", "Keys", "Values"
		/// </summary>
		private void RaisePropertyChangedEvents() =>
			RaisePropertyChangedEvents("Item[]", "Count", "Keys", "Values");

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

		#region Private methods handling Addition/Removal/Modification of items

		/// <summary>
		/// Adds a key-value pair and raises proper events.
		/// If the key is a duplicate, throws an exception if this
		/// instance is configured to throw them, otherwise ignores
		/// </summary>
		/// <param name="pair">Key-Value pair to add to the dictionary</param>
		private void AddWithNotification(KeyValuePair<TKey, TValue> pair)
		{
			AddWithNotification(pair.Key, pair.Value);
		}

		/// <summary>
		/// Adds a key-value pair and raises proper events.
		/// If the key is a duplicate, throws an exception if this
		/// instance is configured to throw them, otherwise ignores
		/// </summary>
		/// <param name="key">Key to add</param>
		/// <param name="value">Value to assign to that key</param>
		private void AddWithNotification(TKey key, TValue value)
		{
			if (mCoreDictionary.TryAdd(key, value))
			{
				// Raise events
				RaiseEventsItemAdded(new DictionaryEntry(key, value));
			}
			else
			{
				// Otherwise, if we're throwing exceptions...
				if (ThrowExceptionForDuplicates)
				{
					// Throw an exception for the duplicate
					throw new Exception($"There already was an item with key: {key.ToString()}");
				}
			}
		}

		/// <summary>
		/// Removes an Key-Pair value from the dictionary (if it was there
		/// in the first place)
		/// </summary>
		/// <param name="key">Key to remove</param>
		/// <param name="value">Variable in which the old value will be saved</param>
		private bool RemoveWithNotification(TKey key, out TValue value)
		{
			if (mCoreDictionary.TryRemove(key, out value))
			{				
				RaiseEventsItemRemoved(new DictionaryEntry(key, value));
				return true;
			}
			else
			{
				value = default(TValue);
				return false;
			}
		}

		/// <summary>
		/// Updates a given element in the collection, then raises proper events
		/// </summary>
		/// <param name="key">Item with key to search</param>
		/// <param name="value">Value to assign</param>
		private void UpdateWithNotification(TKey key, TValue value)
		{
			// If the dictionary has such entry
			if (mCoreDictionary.ContainsKey(key))
			{
				// Save the old value
				TValue oldValue = mCoreDictionary[key];

				// Update the value
				mCoreDictionary[key] = value;

				// Raise events
				RaiseEventsItemModified(oldValue, value);
			}
		}

		#endregion

		#region this operator

		/// <summary>
		/// Allows for access to the items inside the collection
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public TValue this[TKey key]
		{
			get => mCoreDictionary[key];
			set => UpdateWithNotification(key, value);
		}

		#endregion

		#region Add new items

		/// <summary>
		/// Adds a new KeyValuePair to the collection
		/// </summary>
		/// <param name="item">KeyValuePair to add</param>
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			AddWithNotification(item);
		}

		/// <summary>
		/// Adds a new KeyValuePair to the collection
		/// </summary>
		/// <param name="key">Key of the pair</param>
		/// <param name="value">Value of the pair</param>
		public void Add(TKey key, TValue value)
		{
			AddWithNotification(key, value);
		}	

		#endregion

		#region Contains (something) methods

		/// <summary>
		/// Determines whether this collection contains a key from the given KeyValuePair.
		/// Returns true if it does, false otherwise
		/// </summary>
		/// <param name="item">KeyValuePair whose key to look for</param>
		/// <returns></returns>
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (mCoreDictionary.TryGetValue(item.Key, out TValue value))
			{
				return value.Equals(item.Value);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Determines whether this collection contains the given key.
		/// Returns true if it does, false otherwise
		/// </summary>
		/// <param name="key">Key to look for</param>
		/// <returns></returns>
		public bool ContainsKey(TKey key)
		{
			return mCoreDictionary.ContainsKey(key);
		}

		#endregion

		#region Removal of items from the collection

		/// <summary>
		/// Removes a KeyValuePair from the collection.
		/// Returns true on success, false otherwise.
		/// </summary>
		/// <param name="item">KeyValuePair to remove</param>
		/// <returns></returns>
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return RemoveWithNotification(item.Key, out TValue v);
		}

		/// <summary>
		/// Removes a KeyValuePair with a given key from the collection.
		/// Returns true on success, false otherwise.
		/// </summary>
		/// <param name="key">Key of the KeyValuePair to remove</param>
		/// <returns></returns>
		public bool Remove(TKey key)
		{
			return RemoveWithNotification(key, out TValue v);
		}

		#endregion

		#region IEnumerator

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return mCoreDictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return mCoreDictionary.GetEnumerator();
		}

		#endregion

		#region Other Methods

		/// <summary>
		/// Clears the collection
		/// </summary>
		public void Clear()
		{
			mCoreDictionary.Clear();
			RaiseEventsCollectionCleared();
		}

		/// <summary>
		/// Copies all items in the collection to the array
		/// </summary>
		/// <param name="array">Array to copy to</param>
		/// <param name="arrayIndex">Position to start the save operation from in the array</param>
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			foreach (var item in mCoreDictionary.Keys)
			{
				array[arrayIndex] = new KeyValuePair<TKey, TValue>(item, mCoreDictionary[item]);
				++arrayIndex;
			}
		}

		/// <summary>
		/// Attempts to get a value assigned to the given key.
		/// Returns true on success, false otherwise.
		/// </summary>
		/// <param name="key">Key to look for</param>
		/// <param name="value">Variable in which the value will be saved (if found).
		/// If it wasn't found, default(TValue) will be assigend</param>
		/// <returns>Returns true on success, false otherwise.</returns>
		public bool TryGetValue(TKey key, out TValue value)
		{
			return mCoreDictionary.TryGetValue(key, out value);
		}

		#endregion
	}
}