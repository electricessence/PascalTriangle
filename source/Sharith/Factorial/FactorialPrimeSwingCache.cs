// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
	using Sharith.Primes;
	using System.Collections.Generic;
	using System.Numerics;
	using XMath = MathUtils.XMath;

	public class PrimeSwingCache : IFactorialFunction
	{
		public string Name => "PrimeSwingCache     ";

		private Dictionary<int, CachedPrimorial> cache;
		private PrimeSieve sieve;
		private int[] primeList;

		public BigInteger Factorial(int n)
		{
			if (n < 20) { return XMath.Factorial(n); }

			cache = new Dictionary<int, CachedPrimorial>();
			sieve = new PrimeSieve(n);

			var pLen = (int)(2.0 * (XMath.FloorSqrt(n)
					 + n / (XMath.Log2(n) - 1)));
			primeList = new int[pLen];

			var exp2 = n - XMath.BitCount(n);
			return RecFactorial(n) << exp2;
		}

		private BigInteger RecFactorial(int n)
		{
			if (n < 2) return BigInteger.One;

			//-- Not commutative!! 
			return Swing(n) * BigInteger.Pow(RecFactorial(n / 2), 2);
		}

		private BigInteger Swing(int n)
		{
			if (n < 33) return SmallOddSwing[n];

			var count = 0;
			var rootN = XMath.FloorSqrt(n);
			var j = 1;
			var prod = BigInteger.One;
			int high;

			while (true)
			{
				high = n / j++;
				var low = n / j++;

				if (low < rootN) { low = rootN; }
				if (high - low < 32) break;

				var primorial = GetPrimorial(low + 1, high);
				prod *= primorial;
			}

			var primes = sieve.GetPrimeCollection(3, high);

			foreach (var prime in primes)
			{
				int q = n, p = 1;

				while ((q /= prime) > 0)
				{
					if ((q & 1) == 1)
					{
						p *= prime;
					}
				}

				if (p > 1)
				{
					primeList[count++] = p;
				}
			}

			return prod * XMath.Product(primeList, 0, count);
		}

		BigInteger GetPrimorial(int low, int high)
		{
			BigInteger primorial;

			if (cache.TryGetValue(low, out CachedPrimorial lowPrimorial))
			{
				//-- This is the catch! The intervals expand.
				var mid = lowPrimorial.High + 1;
				var highPrimorial = sieve.GetPrimorial(mid, high);
				primorial = highPrimorial * lowPrimorial.Value;
			}
			else
			{
				primorial = sieve.GetPrimorial(low, high);
			}

			cache[low] = new CachedPrimorial(high, primorial);
			return primorial;
		}

		static readonly BigInteger[] SmallOddSwing = {1, 1, 1, 3, 3, 15, 5, 35, 35,
		315, 63, 693, 231, 3003, 429, 6435, 6435, 109395, 12155, 230945,
		46189, 969969, 88179, 2028117, 676039, 16900975, 1300075, 35102025,
		5014575, 145422675, 9694845, 300540195, 300540195};

		private struct CachedPrimorial
		{
			public readonly int High;  // class { get; set; } 
			public readonly BigInteger Value; // class { get; set; } 

			public CachedPrimorial(int highBound, BigInteger val)
			{
				High = highBound;
				Value = val;
			}
		}
	}
} // endOfFactorialPrimeSwingCache

