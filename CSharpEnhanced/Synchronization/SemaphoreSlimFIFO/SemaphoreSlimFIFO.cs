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
	/// The first waiter is guaranteed to be released first. Implementation based on an internal <see cref="SemaphoreSlim"/>
	/// </summary>
    public class SemaphoreSlimFIFO : IDisposable
    {
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="initialCount">Initial number of callers to allow through</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="initialCount"/> is less than 0</exception>
		public SemaphoreSlimFIFO(int initialCount)
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
		public SemaphoreSlimFIFO(int initialCount, int maxCount)
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
		/// Waiting calls
		/// </summary>
		private readonly ConcurrentQueue<TaskCompletionSource<bool>> _Waiters = new ConcurrentQueue<TaskCompletionSource<bool>>();

		#endregion

		#region Public Properties

		/// <summary>
		/// Number of callers than can enter the semaphore
		/// </summary>
		public int CurrentCount => _InternalSemaphore.CurrentCount;

		#endregion

		#region Public Methods

		/// <summary>
		/// Lets through one caller
		/// </summary>
		/// <exception cref="ObjectDisposedException"></exception>
		/// <exception cref="SemaphoreFullException">When semaphore reaches the maximum size</exception>
		/// <returns>The previous count of <see cref="SemaphoreSlimFIFO"/></returns>
		public int Release() => _InternalSemaphore.Release();

		/// <summary>
		/// Lets through the sepcified number of callers
		/// </summary>
		/// <param name="releaseCount">Number of callers to let through</param>
		/// <exception cref="ObjectDisposedException"></exception>
		/// <exception cref="SemaphoreFullException">When semaphore reaches the maximum size</exception>
		/// <returns>The previous count of <see cref="SemaphoreSlimFIFO"/></returns>
		public int Release(int releaseCount) => _InternalSemaphore.Release(releaseCount);

		/// <summary>
		/// Blocks until the caller can enter the semaphore
		/// </summary>
		/// <exception cref="ObjectDisposedException"></exception>
		public void Wait() => WaitAsync().Wait();

		/// <summary>
		/// Asynchronously waits to enter the <see cref="SemaphoreSlimFIFO"/>
		/// </summary>
		/// <returns>A task that will complete when the <see cref="SemaphoreSlimFIFO"/> has been entered</returns>
		/// <exception cref="ObjectDisposedException"></exception>
		public Task WaitAsync()
		{
			// Create a Task to enqueue
			var completed = new TaskCompletionSource<bool>();

			_Waiters.Enqueue(completed);
			
			// Wait asynchronously
			_InternalSemaphore.WaitAsync().ContinueWith(x =>
			{
				// And upon completion, if there was something to dequeue
				if(_Waiters.TryDequeue(out var dequeuedItem))
				{
					// Set its result
					dequeuedItem.SetResult(true);
				}
			});

			return completed.Task;
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