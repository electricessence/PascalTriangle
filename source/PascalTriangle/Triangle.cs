using System.Numerics;
using System.Threading.Tasks;

namespace PascalTriangle
{
	public class Triangle
	{
		public Triangle(Factorial? factorials = null)
		{
			Factorials = factorials ?? new Factorial();
		}

		public Factorial Factorials { get; }

		public async ValueTask<BigInteger> ValueAtAsync(ulong row, ulong column)
		{
			if (column == 0 || column == row) return BigInteger.One;
			if (column == 1) return row;
			if (column > row) return BigInteger.Zero;
			// dedupe the index:
			var mid = row / 2 + row % 2; // 1 = 1, 2 = 1, 3 = 2, 4 = 2, 5 = 3 etc..
			if (column > mid) column = row - column;
			if (column == 1) return row;

			var nF = Factorials.OfAsync(row);
			var kF = Factorials.OfAsync(column);
			var nkF = Factorials.OfAsync(row - column);

			var divisor = await kF * await nkF;
			return await nF / divisor;
		}
	}
}
