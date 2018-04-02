using CSharpEnhanced.Collections;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpEnhanced.Synchronization
{
	/// <summary>
	/// FIFO semaphore, limits the amount of callers that can continue their execution.
	/// The first waiter is guaranteed to be released first. Implementation based on an internal <see cref="SemaphoreSlim"/>.
	/// Allows for timeouts in return for increased memory consumption and lower speed
	/// </summary>
	public class SemaphoreSlimFIFOTimeout : IDisposable
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="initialCount">Initial number of callers to allow through</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="initialCount"/> is less than 0</exception>
		public SemaphoreSlimFIFOTimeout(int initialCount)
		{
			_InternalSemaphore = new SemaphoreSlim(initialCount);
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="initialCount">Initial number of callers to allow through</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="initialCount"/> is less than 0 or
		/// <paramref name="maxCount"/> is less than 0 or
		/// <paramref name="initialCount"/> is greater than <paramref name="maxCount"/></exception>
		public SemaphoreSlimFIFOTimeout(int initialCount, int maxCount)
		{
			_InternalSemaphore = new SemaphoreSlim(initialCount, maxCount);
		}

		#endregion

		#region Private Memebers

		/// <summary>
		/// Internal semaphore used to block callers
		/// </summary>
		private readonly SemaphoreSlim _InternalSemaphore;

		/// <summary>
		/// Waiting calls and their status (true = timed out)
		/// </summary>
		private readonly ConcurrentQueue<Pair<TaskCompletionSource<bool>, bool>> _Waiters =
			new ConcurrentQueue<Pair<TaskCompletionSource<bool>, bool>>();

		#endregion

		#region Public Properties

		/// <summary>
		/// Number of callers than can enter the semaphore
		/// </summary>
		public int CurrentCount => _InternalSemaphore.CurrentCount;

		/// <summary>
		/// Returns a <see cref="WaitHandle"/> from the internal <see cref="SemaphoreSlim"/> that can be used to wait on the
		/// <see cref="SemaphoreSlimFIFOTimeout"/>
		/// </summary>
		public WaitHandle AvailableWaitHandle => _InternalSemaphore.AvailableWaitHandle;

		/// <summary>
		/// The number of callers waiting to get through the semaphore. Includes waiters that timed out but didn't reach
		///  the front of the queue yet (however they won't be let through when calling <see cref="Release"/>)
		/// </summary>
		public int Waiting => _Waiters.Count;

		#endregion

		#region Public Methods

		#region Accessors

		/// <summary>
		/// Returns the number of active callers waiting to get through the semaphore (excluding timed out callers)
		/// </summary>
		/// <returns></returns>
		public int ActiveWaiters()
		{
			int activeWaiters = 0;
			
			// Foreach waiter that isn't marked as timed out
			foreach(var item in _Waiters)
			{
				if (!item.Second)
				{
					++activeWaiters;
				}
			}

			return activeWaiters;
		}

		/// <summary>
		/// Returns the number of timed out callers that weren't yet dequeued
		/// </summary>
		/// <returns></returns>
		public int TimedoutWaiters()
		{
			int timedOutWaiters = 0;

			// For each waiter that is marked as timed out
			foreach (var item in _Waiters)
			{
				if (item.Second)
				{
					++timedOutWaiters;
				}
			}

			return timedOutWaiters;
		}

		#endregion

		#region Releasing

		/// <summary>
		/// Lets through one caller
		/// </summary>
		/// <exception cref="ObjectDisposedException"></exception>
		/// <exception cref="SemaphoreFullException">When semaphore reaches the maximum size</exception>
		/// <returns>The previous count of <see cref="SemaphoreSlimFIFOTimeout"/></returns>
		public int Release() => _InternalSemaphore.Release();

		/// <summary>
		/// Lets through the sepcified number of callers
		/// </summary>
		/// <param name="releaseCount">Number of callers to let through</param>
		/// <exception cref="ObjectDisposedException"></exception>
		/// <exception cref="SemaphoreFullException">When semaphore reaches the maximum size</exception>
		/// <returns>The previous count of <see cref="SemaphoreSlimFIFOTimeout"/></returns>
		public int Release(int releaseCount) => _InternalSemaphore.Release(releaseCount);

		#endregion

		#region Waiting Synchronously

		/// <summary>
		/// Blocks until the caller can enter the semaphore
		/// </summary>
		/// <exception cref="ObjectDisposedException"></exception>
		public void Wait() => WaitAsync().Wait();

		/// <summary>
		/// Blocks until the caller can enter the <see cref="SemaphoreSlimFIFOTimeout"/>
		/// while observing the <see cref="CancellationToken"/>
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was canceled</exception>
		/// <exception cref="ObjectDisposedException"/>
		public void Wait(CancellationToken cancellationToken) => WaitAsync(cancellationToken).Wait();

		/// <summary>
		/// Blocks until the caller can enter the <see cref="SemaphoreSlimFIFOTimeout"/> or a timeout occurs.
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait before timing out.
		/// <see cref="Timeout.Infinite"/> (-1) to wait indefinitely, or 0 to test the state and return immediately</param>
		/// <returns>True if the caller successfully entered the <see cref="SemaphoreSlimFIFOTimeout"/>, false otherwise</returns>
		/// <exception cref="ArgumentOutOfRangeException"/>
		/// <exception cref="ObjectDisposedException"/>
		public bool Wait(int millisecondsTimeout) => WaitAsync(millisecondsTimeout).Result;

		/// <summary>
		/// Blocks until the caller can enter the <see cref="SemaphoreSlimFIFOTimeout"/> or a timeout occurs while
		/// observing the <see cref="CancellationToken"/>
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait before timing out.
		/// <see cref="Timeout.Infinite"/> (-1) to wait indefinitely, or 0 to test the state and return immediately</param>
		/// <param name="cancellationToken"></param>
		/// <returns>True if the caller successfully entered the <see cref="SemaphoreSlimFIFOTimeout"/>, false otherwise</returns>
		/// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was canceled</exception>
		/// <exception cref="ArgumentOutOfRangeException"/>
		/// <exception cref="ObjectDisposedException"/>
		public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken) => 
			WaitAsync(millisecondsTimeout, cancellationToken).Result;

		/// <summary>
		/// Blocks until the caller can enter the <see cref="SemaphoreSlimFIFOTimeout"/> or a timeout occurs.
		/// </summary>
		/// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds to wait before timing out.
		/// a <see cref="TimeSpan"/> that represents (-1) to wait indefinitely, or a <see cref="TimeSpan"/> that represents 0 to
		/// test the state and return immediately</param>
		/// <returns>True if the caller successfully entered the <see cref="SemaphoreSlimFIFOTimeout"/>, false otherwise</returns>
		/// <exception cref="ArgumentOutOfRangeException"/>
		/// <exception cref="ObjectDisposedException"/>
		public bool Wait(TimeSpan timeout) => WaitAsync(timeout).Result;

		/// <summary>
		/// Blocks until the caller can enter the <see cref="SemaphoreSlimFIFOTimeout"/> or a timeout occurs while
		/// observing the <see cref="CancellationToken"/>
		/// </summary>
		/// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds to wait before timing out.
		/// a <see cref="TimeSpan"/> that represents (-1) to wait indefinitely, or a <see cref="TimeSpan"/> that represents 0 to
		/// test the state and return immediately</param>
		/// <param name="cancellationToken"></param>
		/// <returns>True if the caller successfully entered the <see cref="SemaphoreSlimFIFOTimeout"/>, false otherwise</returns>
		/// <exception cref="OperationCanceledException"><paramref name="cancellationToken"/> was canceled</exception>
		/// <exception cref="ArgumentOutOfRangeException"/>
		/// <exception cref="ObjectDisposedException"/>
		public bool Wait(TimeSpan timeout, CancellationToken cancellationToken) => WaitAsync(timeout, cancellationToken).Result;

		#endregion

		#region Waiting Asynchronously

		/// <summary>
		/// Asynchronously waits to enter the <see cref="SemaphoreSlimFIFOTimeout"/>
		/// </summary>
		/// <returns>A task that will complete when the <see cref="SemaphoreSlimFIFOTimeout"/> has been entered</returns>
		/// <exception cref="ObjectDisposedException"/>
		public Task WaitAsync()
		{
			// Create a Task to enqueue
			var completed = new TaskCompletionSource<bool>();

			_Waiters.Enqueue(new Pair<TaskCompletionSource<bool>, bool>(completed, false));

			// Wait asynchronously
			_InternalSemaphore.WaitAsync().ContinueWith(x =>
			{
				// Keep dequeuing items until there are no more items or the dequeued item didn't time out
				while (_Waiters.TryDequeue(out var dequeuedItem))
				{
					// If the dequeued item didn't time out
					if (!dequeuedItem.Second)
					{
						// Set the result and break
						dequeuedItem.First.SetResult(true);
						break;
					}
				}
			});

			return completed.Task;
		}

		/// <summary>
		/// Asynchronously waits to enter the <see cref="SemaphoreSlimFIFOTimeout"/> while observing the <see cref="CancellationToken"/>
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns>A task that will complete when the <see cref="SemaphoreSlimFIFOTimeout"/> has been entered</returns>
		/// <exception cref="OperationCanceledException"/>
		/// <exception cref="ObjectDisposedException"/>
		public Task WaitAsync(CancellationToken cancellationToken)
		{
			// Create a Task to enqueue
			var completed = new TaskCompletionSource<bool>();

			_Waiters.Enqueue(new Pair<TaskCompletionSource<bool>, bool>(completed, false));

			// Wait asynchronously
			_InternalSemaphore.WaitAsync(cancellationToken).ContinueWith(x =>
			{
				if(x.IsCanceled)
				{
					throw new OperationCanceledException();
				}
				// Keep dequeuing items until there are no more items or the dequeued item didn't time out
				while (_Waiters.TryDequeue(out var dequeuedItem))
				{
					// If the dequeued item didn't time out
					if (!dequeuedItem.Second)
					{
						// Set the result and break
						dequeuedItem.First.SetResult(true);
						break;
					}
				}
			});

			return completed.Task;
		}

		/// <summary>
		/// Asynchronously waits to enter the <see cref="SemaphoreSlimFIFOTimeout"/> or until a timeout occurs
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait before timing out.
		/// <see cref="Timeout.Infinite"/> (-1) to wait indefinitely, or 0 to test the state and return immediately</param>
		/// <returns>A task that will complete with result of true when the <see cref="SemaphoreSlimFIFOTimeout"/> has been entered
		/// or with result of false otherwise</returns>
		/// <exception cref="ArgumentOutOfRangeException"/>
		/// <exception cref="ObjectDisposedException"/>
		public Task<bool> WaitAsync(int millisecondsTimeout)
		{
			// Create a Task to enqueue
			var completed = new Pair<TaskCompletionSource<bool>, bool>(new TaskCompletionSource<bool>(), false);
			
			_Waiters.Enqueue(completed);

			// Wait asynchronously
			_InternalSemaphore.WaitAsync(millisecondsTimeout).ContinueWith(x =>
			{
				if(x.Result)
				{
					_Waiters.TryDequeue(out var dequeuedItem);
				}
				else
				{
					completed.Second = true;
				}
				completed.First.SetResult(x.Result);
			});

			return completed.First.Task;
		}

		/// <summary>
		/// Asynchronously waits to enter the <see cref="SemaphoreSlimFIFOTimeout"/> or until a timeout occurs 
		/// while observing the <see cref="CancellationToken"/>
		/// </summary>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait before timing out.
		/// <see cref="Timeout.Infinite"/> (-1) to wait indefinitely, or 0 to test the state and return immediately</param>
		/// <param name="cancellationToken"></param>
		/// <returns>A task that will complete with result of true when the <see cref="SemaphoreSlimFIFOTimeout"/> has been entered
		/// or with result of false otherwise</returns>
		/// <exception cref="ArgumentOutOfRangeException"/>
		/// <exception cref="OperationCanceledException"/>
		/// <exception cref="ObjectDisposedException"/>
		public Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			
			// Create a Task to enqueue
			var completed = new Pair<TaskCompletionSource<bool>, bool>(new TaskCompletionSource<bool>(), false);

			_Waiters.Enqueue(completed);

			// Wait asynchronously
			_InternalSemaphore.WaitAsync(millisecondsTimeout, cancellationToken).ContinueWith(x =>
			{
				if (x.Result)
				{
					_Waiters.TryDequeue(out var dequeuedItem);
				}
				else
				{
					completed = new Pair<TaskCompletionSource<bool>, bool>(completed.First, true);
				}
				completed.First.SetResult(x.Result);
			});

			return completed.First.Task;
		}

		/// <summary>
		/// Asynchronously waits to enter the <see cref="SemaphoreSlimFIFOTimeout"/> or until a timeout occurs
		/// </summary>
		/// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds to wait before timing out.
		/// a <see cref="TimeSpan"/> that represents (-1) to wait indefinitely, or a <see cref="TimeSpan"/> that represents 0 to
		/// test the state and return immediately</param>
		/// <returns>A task that will complete with result of true when the <see cref="SemaphoreSlimFIFOTimeout"/> has been entered
		/// or with result of false otherwise</returns>
		/// <exception cref="ArgumentOutOfRangeException"/>
		/// <exception cref="ObjectDisposedException"/>
		public Task<bool> WaitAsync(TimeSpan timeout)
		{
			if(timeout.Milliseconds > Int32.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(timeout));
			}
			else
			{
				return WaitAsync(timeout.Milliseconds);
			}
		}

		/// <summary>
		/// Asynchronously waits to enter the <see cref="SemaphoreSlimFIFOTimeout"/> or until a timeout occurs 
		/// while observing the <see cref="CancellationToken"/>
		/// </summary>
		/// <param name="timeout">A <see cref="TimeSpan"/> that represents the number of milliseconds to wait before timing out.
		/// a <see cref="TimeSpan"/> that represents (-1) to wait indefinitely, or a <see cref="TimeSpan"/> that represents 0 to
		/// test the state and return immediately</param>
		/// <param name="cancellationToken"></param>
		/// <returns>A task that will complete with result of true when the <see cref="SemaphoreSlimFIFOTimeout"/> has been entered
		/// or with result of false otherwise</returns>
		/// <exception cref="ArgumentOutOfRangeException"/>
		/// <exception cref="OperationCanceledException"/>
		/// <exception cref="ObjectDisposedException"/>
		public Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
		{
			if (timeout.Milliseconds > Int32.MaxValue)
			{
				throw new ArgumentOutOfRangeException(nameof(timeout));
			}
			else
			{
				return WaitAsync(timeout.Milliseconds, cancellationToken);
			}
		}

		#endregion

		#region IDisposable

		/// <summary>
		/// Releases all resources
		/// </summary>
		public void Dispose()
		{
			_InternalSemaphore.Dispose();
		}

		#endregion

		#endregion
	}
}
