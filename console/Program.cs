using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PascalTriangle
{

	class Program
	{
		readonly Triangle Triangle = new Triangle();

		public static async Task Main()
		{
			var p = new Program();
			await p.Run();
		}

		async ValueTask Run()
		{
			await Test(3000);
			await Test(10000);
			await Test(100000);
			await Test(1000000);

			Console.ReadKey();
		}

		async ValueTask Test(ulong row)
		{
			ulong column = row / 2;
			await Search(row, column);
			await Search(row + 1, column);

			async ValueTask Search(ulong row, ulong column)
			{
				Console.WriteLine("Row {0}, Column {1}:", row, column);
				var watch = Stopwatch.StartNew();
				watch.Start();
				var value = await Triangle.ValueAtAsync(row, column);
				watch.Stop();

				Console.WriteLine(value);
				Console.WriteLine("Time Elapsed: {0}", watch.Elapsed);
				Console.WriteLine();
			}
		}
	}
}
