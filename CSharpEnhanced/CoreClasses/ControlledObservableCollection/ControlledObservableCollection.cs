using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace CSharpEnhanced.CoreClasses
{
	/// <summary>
	/// Observable collection which will run appropriate action (passed in the constructor) when items are added to the collection or
	/// removed from the collection. Does not run the actions on <see cref="NotifyCollectionChangedAction.Move"/>. Runs the actions
	/// before <see cref="ObservableCollection{T}.CollectionChanged"/> event is fired.
	/// </summary>
	/// <typeparam name="T">Type of items in the collection</typeparam>
	public class ControlledObservableCollection<T> : ObservableCollection<T>
	{
		#region Constructor

		/// <summary>
		/// Default Constructor
		/// </summary>
		public ControlledObservableCollection(Action<T> newItemRoutine, Action<T> oldItemRoutine)
		{
			_NewItemRoutine = newItemRoutine;
			_OldItemRoutine = oldItemRoutine;
		}

		#endregion

		#region Private properties

		/// <summary>
		/// Control archive used to handle <see cref="NotifyCollectionChangedAction.Reset"/>
		/// </summary>
		private List<T> _ControlArchive { get; } = new List<T>();

		/// <summary>
		/// Action performed on each item added to the collection
		/// </summary>
		private Action<T> _NewItemRoutine { get; }

		/// <summary>
		/// Action performed on each item removed from the collection
		/// </summary>
		private Action<T> _OldItemRoutine { get; }

		#endregion

		#region Private methods

		/// <summary>
		/// Runs the <see cref="_OldItemRoutine"/> on new items and removes them from <see cref="_ControlArchive"/>
		/// </summary>
		/// <param name="oldItems"></param>
		private void ItemsDeleted(IList oldItems)
		{
			foreach (T item in oldItems)
			{
				_OldItemRoutine?.Invoke(item);
				_ControlArchive.Remove(item);
			}
		}

		/// <summary>
		/// Runs the <see cref="_NewItemRoutine"/> on new items and adds them to <see cref="_ControlArchive"/>
		/// </summary>
		/// <param name="newItems"></param>
		private void ItemsAdded(IList newItems)
		{
			foreach (T item in newItems)
			{
				_NewItemRoutine?.Invoke(item);
				_ControlArchive.Add(item);
			}
		}

		/// <summary>
		/// For extraordinary modifications of the collection, checks each item individually against <see cref="_ControlArchive"/>,
		/// determines new items and old items, then runs <see cref="ItemsAdded(IList)"/> and <see cref="ItemsDeleted(IList)"/> on them
		/// respectively
		/// </summary>
		private void CollectionReset()
		{
			// Find old items and handle them
			ItemsDeleted(new List<T>(_ControlArchive.Except(this)));

			// Find new items and handle them
			ItemsAdded(new List<T>(this.Except(_ControlArchive)));
		}

		/// <summary>
		/// Moves the items in the control archive
		/// </summary>
		/// <param name="oldIndex"></param>
		/// <param name="newIndex"></param>
		private void ItemsMoved(int oldIndex, int newIndex)
		{
			// Get the moved item
			var movedItem = _ControlArchive[oldIndex];

			// Remove it
			_ControlArchive.RemoveAt(oldIndex);

			// And add it at the new index
			_ControlArchive.Insert(newIndex, movedItem);
		}

		/// <summary>
		/// Checks the integrity of this collection (if the underlying collection is equal to <see cref="_ControlArchive"/>)
		/// Dedicated for debug
		/// </summary>
		[Conditional("DEBUG")]
		private void CheckIntegrity()
		{
			// Check the count
			if (this.Count == _ControlArchive.Count)
			{
				// For each element
				for (int i = 0; i < this.Count; ++i)
				{
					// If one pair isn't equal signal it
					if (!this[i].Equals(_ControlArchive[i]) && Debugger.IsAttached)
					{
						// Collection integrity compromised - pair of items wasn't equal
						throw new Exception("Collection integrity compromised");
					}
				}
			}
			else if (Debugger.IsAttached)
			{
				// Collection integrity compromised - _ControlArchive has a different number of items than underlying collection
				throw new Exception("Collection integrity compromised");
			}
		}

		#endregion

		#region Protected methods

		/// <summary>
		/// Runs the <see cref="_NewItemRoutine"/> and <see cref="_OldItemRoutine"/> on items added / deleted (respectively)
		/// </summary>
		/// <param name="e"></param>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			// Handle actions
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Remove:
					{
						ItemsDeleted(e.OldItems);
					}
					break;

				case NotifyCollectionChangedAction.Add:
					{
						ItemsAdded(e.NewItems);
					}
					break;

				case NotifyCollectionChangedAction.Replace:
					{
						ItemsDeleted(e.OldItems);
						ItemsAdded(e.NewItems);
					}
					break;

				case NotifyCollectionChangedAction.Reset:
					{
						CollectionReset();
					}
					break;

				case NotifyCollectionChangedAction.Move:
					{
						ItemsMoved(e.OldStartingIndex, e.NewStartingIndex);
					}
					break;
			}

			// If debug - check integrity
			CheckIntegrity();

			// Invoke the base method
			base.OnCollectionChanged(e);
		}

		#endregion
	}
}