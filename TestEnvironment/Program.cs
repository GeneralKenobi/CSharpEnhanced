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
			var s = new SemaphoreSlim(0);
			CancellationTokenSource source = new CancellationTokenSource();
			var t = source.Token;
		
			Task.Run(async() => { await Task.Delay(1000); source.Cancel(); }) ;

			s.Wait(10000, t);



			for (int i=1; i<6; ++i)
			{
				Test(i);
			}

			while(true)
			{
				Console.ReadLine();
				l.Release();
			}
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
