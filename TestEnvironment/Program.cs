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
			//Task.Run(() =>
			//{
			//	Thread.Sleep(1000);
			//	Console.WriteLine("Standard Task waiting...");
			//	l.Wait();
			//	Console.WriteLine("Standard Task went through...");
			//});
			Task.Run(() =>
			{
				Thread.Sleep(1500);
				Console.WriteLine("Timeout Task waiting...");
				l.Wait();

			});
			Task.Run(() =>
			{
				Console.WriteLine("Timeout Task waiting...");
				if(l.Wait(1000))
					Console.WriteLine("Timeout Task went through...");
				else
					Console.WriteLine("Timeout Task timed out...");

			});
			Task.Run(() =>
			{
				Console.WriteLine("Timeout Task waiting...");
				if (l.Wait(1000))
					Console.WriteLine("Timeout Task went through...");
				else
					Console.WriteLine("Timeout Task timed out...");

			});
			

			Thread.Sleep(2000);
			int gen = l.Waiting;

			int act = l.ActiveWaiters();

			int inact = l.TimedoutWaiters();

			//Task.Run(() =>
			//{
			//	Thread.Sleep(1500);
			//	l.Release();
			//});

			Console.ReadLine();

			l.Release();
			Thread.Sleep(1000);
			gen = l.Waiting;

			act = l.ActiveWaiters();

			inact = l.TimedoutWaiters();
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
