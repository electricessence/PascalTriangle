using System;
using System.Numerics;

namespace Sharith.Factorial
{
	public static class Extensions
	{
		public static BigInteger Factorial(this IFactorialFunction fn, ulong n)
			=> n > int.MaxValue
				? throw new ArgumentOutOfRangeException(nameof(n), n, "Greater than int.MaxValue not yet supported")
				: fn.Factorial((int)n);
	}
}
