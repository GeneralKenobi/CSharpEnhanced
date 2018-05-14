using CSharpEnhanced.Helpers;
using CSharpEnhanced.Synchronization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TestEnvironment
{

	

	class Program
    {
		static SemaphoreSlimFIFOTimeout l = new SemaphoreSlimFIFOTimeout(0,5);
		static void Main(string[] args)
		{
			int a = 5;
			object b = a;
			TypeHelpers.TryCast<string>(a, out string d);

			CancellationTokenSource c = new CancellationTokenSource();
			CancellationToken token = c.Token;
			c.Dispose();
			c = null;
			
			AutoResetEvent e = new AutoResetEvent(false);
			Stopwatch s = new Stopwatch();
			

			//c.Token.Register(() => e.Set());
			
			var task = Task.Factory.StartNew(() =>
			{

			
					s.Start();
					var waitingArray = new[] { c.Token.WaitHandle, e };
					var returned = WaitHandle.WaitAny(waitingArray);
					c.Token.ThrowIfCancellationRequested();
				
			}, c.Token).ContinueWith((x) => Debug.WriteLine($"Task canceled at {s.ElapsedMilliseconds}"));
			
			Task.Run(() =>
			{
				Thread.Sleep(500);
				e.Set();
				//c.Cancel();

				Thread.Sleep(500);
			});

			var state = task.IsCanceled;

			Console.ReadKey();
		}

		private static async void Test(int counter)
		{
			await Task.Delay(1000*counter);
			Console.WriteLine($"Call #{counter} is waiting...");
			if(l.Wait(1000))
				Console.WriteLine($"Call #{counter} went through");
			else
				Console.WriteLine($"Call #{counter} timed out");
		}
	}
}
