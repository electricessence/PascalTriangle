using System.Numerics;

namespace PascalTriangle
{
	public class Triangle
	{
		readonly Factorial Factorials = new Factorial();

		public BigInteger ValueAt(ulong row, ulong column)
		{
			if (column == 0 || column == row) return BigInteger.One;
			if (column == 1) return row;
			if (column > row) return BigInteger.Zero;
			// dedupe the index:
			var mid = row / 2 + row % 2; // 1 = 1, 2 = 1, 3 = 2, 4 = 2, 5 = 3 etc..
			if (column > mid) column = row - column;
			if (column == 1) return row;

			var divisor = Factorials.Of(column) * Factorials.Of(row - column);
			return Factorials.Of(row) / divisor;
		}
	}
}
