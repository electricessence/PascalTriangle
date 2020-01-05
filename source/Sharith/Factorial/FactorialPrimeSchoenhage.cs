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

	public class PrimeSchoenhage : IFactorialFunction
	{
		public string Name => "PrimeSchoenhage     ";
		private int[] primeList;
		private int[] multiList;

		public BigInteger Factorial(int n)
		{
			if (n < 20) { return XMath.Factorial(n); }

			var lgN = XMath.FloorLog2(n);
			var piN = 2 + (15 * n) / (8 * (lgN - 1));

			primeList = new int[piN];
			multiList = new int[piN];

			var len = PrimeFactors(n);
			var exp2 = n - XMath.BitCount(n);

			return NestedSquare(len) << exp2;
		}

		private BigInteger NestedSquare(int len)
		{
			if (len == 0) return BigInteger.One;

			var i = 0;
			var mult = multiList[0];

			while (mult > 1)
			{
				if ((mult & 1) == 1)  // is mult odd ?
				{
					primeList[len++] = primeList[i];
				}

				multiList[i++] = mult >> 1;
				mult = multiList[i];
			}

			return XMath.Product(primeList, i, len - i)
				  * BigInteger.Pow(NestedSquare(i), 2);
		}

		private int PrimeFactors(int n)
		{
			var sieve = new PrimeSieve(n);
			var primes = sieve.GetPrimeCollection(3, n);

			int maxBound = n / 2, count = 0;

			foreach (var prime in primes)
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
} // endOfFactorialPrimeSchoenhage
