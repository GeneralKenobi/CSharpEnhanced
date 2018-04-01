using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
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
		private readonly ConcurrentQueue<Tuple<TaskCompletionSource<bool>, bool>> _Waiters =
			new ConcurrentQueue<Tuple<TaskCompletionSource<bool>, bool>>();

		#endregion

		#region Public Properties

		/// <summary>
		/// Number of callers than can enter the semaphore
		/// </summary>
		public int CurrentCount => _InternalSemaphore.CurrentCount;

		#endregion

		#region Public Methods

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

		public void Wait(CancellationToken cancellationToken) => WaitAsync(cancellationToken).Wait();

		public bool Wait(int millisecondsTimeout) => WaitAsync(millisecondsTimeout).Result;
		public bool Wait(int millisecondsTimeout, CancellationToken cancellationToken) => 
			WaitAsync(millisecondsTimeout, cancellationToken).Result;

		public bool Wait(TimeSpan timeout) => WaitAsync(timeout).Result;

		public bool Wait(TimeSpan timeout, CancellationToken cancellationToken) => WaitAsync(timeout, cancellationToken).Result;
		#endregion
		/// <summary>
		/// Asynchronously waits to enter the <see cref="SemaphoreSlimFIFOTimeout"/>
		/// </summary>
		/// <returns>A task that will complete when the <see cref="SemaphoreSlimFIFOTimeout"/> has been entered</returns>
		/// <exception cref="ObjectDisposedException"></exception>
		public Task WaitAsync()
		{
			// Create a Task to enqueue
			var completed = new TaskCompletionSource<bool>();

			_Waiters.Enqueue(new Tuple<TaskCompletionSource<bool>, bool>(completed, false));

			// Wait asynchronously
			_InternalSemaphore.WaitAsync().ContinueWith(x =>
			{
				// Keep dequeuing items until there are no more items or the dequeued item didn't time out
				while (_Waiters.TryDequeue(out var dequeuedItem))
				{
					// If the dequeued item didn't time out
					if (!dequeuedItem.Item2)
					{
						// Set the result and break
						dequeuedItem.Item1.SetResult(true);
						break;
					}
				}
			});

			return completed.Task;
		}

		public Task WaitAsync(CancellationToken cancellationToken)
		{
			// Create a Task to enqueue
			var completed = new TaskCompletionSource<bool>();

			_Waiters.Enqueue(new Tuple<TaskCompletionSource<bool>, bool>(completed, false));

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
					if (!dequeuedItem.Item2)
					{
						// Set the result and break
						dequeuedItem.Item1.SetResult(true);
						break;
					}
				}
			});

			return completed.Task;
		}

		public Task<bool> WaitAsync(int millisecondsTimeout)
		{
			// Create a Task to enqueue
			var completed = new Tuple<TaskCompletionSource<bool>, bool>(new TaskCompletionSource<bool>(), false);
			
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
					completed = new Tuple<TaskCompletionSource<bool>, bool>(completed.Item1, true);
				}
				completed.Item1.SetResult(x.Result);
			});

			return completed.Item1.Task;
		}

		public Task<bool> WaitAsync(int millisecondsTimeout, CancellationToken cancellationToken)
		{
			
			// Create a Task to enqueue
			var completed = new Tuple<TaskCompletionSource<bool>, bool>(new TaskCompletionSource<bool>(), false);

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
					completed = new Tuple<TaskCompletionSource<bool>, bool>(completed.Item1, true);
				}
				completed.Item1.SetResult(x.Result);
			});

			return completed.Item1.Task;
		}

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


		/// <summary>
		/// Releases all resources
		/// </summary>
		public void Dispose()
		{
			_InternalSemaphore.Dispose();
		}

		#endregion
	}
}
