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

	public class PrimeSwingList : IFactorialFunction
	{
		public string Name => "PrimeSwingList      ";

		private int[][] primeList;
		private int[] listLength;
		private int[] tower;
		private int[] bound;

		public BigInteger Factorial(int n)
		{
			if (n < 20) { return XMath.Factorial(n); }

			var log2N = XMath.FloorLog2(n);
			var j = log2N;
			var hN = n;

			primeList = new int[log2N][];
			listLength = new int[log2N];
			bound = new int[log2N];
			tower = new int[log2N + 1];

			while (true)
			{
				tower[j] = hN;
				if (hN == 1) break;
				bound[--j] = hN / 3;
				var pLen = hN < 4 ? 6 : (int)(2.0 * (XMath.FloorSqrt(hN)
						 + hN / (XMath.Log2(hN) - 1)));
				primeList[j] = new int[pLen];
				hN >>= 1;
			}

			tower[0] = 2;
			PrimeFactors(n);

			var init = listLength[0] == 0 ? 1 : 3;
			var oddFactorial = new BigInteger(init);

			for (var i = 1; i < log2N; i++)
			{
				oddFactorial = BigInteger.Pow(oddFactorial, 2)
					* XMath.Product(primeList[i], 0, listLength[i]);
			}
			return oddFactorial << (n - XMath.BitCount(n));
		}

		private void PrimeFactors(int n)
		{
			var maxBound = n / 3;
			var lastList = primeList.Length - 1;
			var start = tower[1] == 2 ? 1 : 0;
			var sieve = new PrimeSieve(n);

			for (var section = start; section < primeList.Length; section++)
			{
				var primes = sieve.GetPrimeCollection(tower[section] + 1, tower[section + 1]);

				foreach (var prime in primes)
				{
					primeList[section][listLength[section]++] = prime;
					if (prime > maxBound) continue;

					var np = n;
					do
					{
						var k = lastList;
						var q = np /= prime;

						do if ((q & 1) == 1) //if q is odd
							{
								primeList[k][listLength[k]++] = prime;
							}
						while (((q >>= 1) > 0) && (prime <= bound[--k]));

					} while (prime <= np);
				}
			}
		}
	}
} // endOfFactorialPrimeSwingList
