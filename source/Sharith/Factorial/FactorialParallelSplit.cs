// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

// Same algorithm as Split
// but computing products using tasks.

namespace Sharith.Factorial
{
	using System;
	using System.Numerics;
	using System.Threading.Tasks;
	using XMath = MathUtils.XMath;

	public class ParallelSplit : IFactorialFunction
	{
		public string Name => "ParallelSplit       ";

		private delegate BigInteger ProductDelegate(int from, int to);

		BigInteger IFactorialFunction.Factorial(int n)
		{
			if (n < 0)
			{
				throw new ArgumentOutOfRangeException(
					Name + ": " + nameof(n) + " >= 0 required, but was " + n);
			}

			if (n < 2) return BigInteger.One;

			var log2N = XMath.FloorLog2(n);
			ProductDelegate prodDelegate = Product;
			var results = new IAsyncResult[log2N];

			int high = n, low = n >> 1, shift = low, taskCounter = 0;

			// -- It is more efficient to add the big intervals
			// -- first and the small ones later!
			while ((low + 1) < high)
			{
				results[taskCounter++] = prodDelegate.BeginInvoke(low + 1, high, null, null);
				high = low;
				low >>= 1;
				shift += low;
			}

			BigInteger p = BigInteger.One, r = BigInteger.One;
			while (--taskCounter >= 0)
			{
				var I = Task.Factory.StartNew(() => r * p);
				var t = p * prodDelegate.EndInvoke(results[taskCounter]);
				r = I.Result;
				p = t;
			}

			return (r * p) << shift;
		}

		private static BigInteger Product(int n, int m)
		{
			n |= 1;       // Round n up to the next odd number
			m = (m - 1) | 1; // Round m down to the next odd number

			if (m == n)
			{
				return new BigInteger(m);
			}

			if (m == (n + 2))
			{
				return new BigInteger((long)n * m);
			}

			var k = (n + m) >> 1;
			return Product(n, k) * Product(k + 1, m);
		}
	}
}
