using System.Numerics;
using System.Threading.Tasks;

namespace PascalTriangle
{
	public class Triangle
	{
		public async ValueTask<BigInteger> ValueAtAsync(ulong row, ulong column)
		{
			if (column == 0 || column == row) return BigInteger.One;
			if (column == 1) return row;
			if (column > row) return BigInteger.Zero;
			// dedupe the index:
			var mid = row / 2 + row % 2; // 1 = 1, 2 = 1, 3 = 2, 4 = 2, 5 = 3 etc..
			if (column > mid) column = row - column;
			if (column == 1) return row;

			var nF = Factorial.OfAsync(row);
			var kF = Factorial.OfAsync(column);
			var nkF = Factorial.OfAsync(row - column);

			var divisor = await kF * await nkF;
			return await nF / divisor;
		}
	}
}
