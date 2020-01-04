using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PascalTriangle
{

	class Program
	{
		public static async Task Main()
		{
			const ulong row = 3000; // Typical stack overflow will happen in less than 3000.
			const ulong column = row / 2;
			var rows = new Row.Collection();
			await Search(row, column);
			await Search(row + 1, column); // Should be near instantaneous after previous run.

			async ValueTask Search(ulong row, ulong column)
			{
				Console.WriteLine("Row {0}, Column {1}:", row, column);
				var watch = Stopwatch.StartNew();
				watch.Start();
				var value = await rows[row][column];
				watch.Stop();

				Console.WriteLine(value);
				Console.WriteLine($"Time Elapsed: {watch.ElapsedMilliseconds} ms");
				Console.WriteLine();
			}
		}
	}
}
