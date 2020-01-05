// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
	using Sharith.Primes;

	using System.Numerics;
	using XMath = MathUtils.XMath;

	public class PrimeBorwein : IFactorialFunction
	{
		public string Name => "PrimeBorwein        ";

		int[] primeList;
		int[] multiList;

		public BigInteger Factorial(int n)
		{
			if (n < 0)
			{
				throw new System.ArgumentOutOfRangeException(
					Name + ": " + nameof(n) + " >= 0 required, but was " + n);
			}

			if (n < 20) { return XMath.Factorial(n); }

			var lgN = XMath.FloorLog2(n);
			var piN = 2 + (15 * n) / (8 * (lgN - 1));

			primeList = new int[piN];
			multiList = new int[piN];

			var len = PrimeFactors(n);
			var exp2 = n - XMath.BitCount(n);

			return RepeatedSquare(len, 1) << exp2;
		}

		private BigInteger RepeatedSquare(int len, int k)
		{
			if (len == 0) return BigInteger.One;

			int i = 0, mult = multiList[0];

			while (mult > 1)
			{
				if ((mult & 1) == 1)  // is mult odd ?
				{
					primeList[len++] = primeList[i];
				}

				multiList[i++] = mult >> 1;
				mult = multiList[i];
			}

			var p = XMath.Product(primeList, i, len - i);
			return BigInteger.Pow(p, k) * RepeatedSquare(i, 2 * k);
		}

		private int PrimeFactors(int n)
		{
			var sieve = new PrimeSieve(n);
			var primeCollection = sieve.GetPrimeCollection(3, n);

			int maxBound = n / 2, count = 0;

			foreach (var prime in primeCollection)
			{
				var m = prime > maxBound ? 1 : 0;

				if (prime <= maxBound)
				{
					var q = n;
					while (q >= prime)
					{
						m += q /= prime;
					}
				}
				primeList[count] = prime;
				multiList[count++] = m;
			}
			return count;
		}
	}
} // endOfFactorialPrimeBorwein
