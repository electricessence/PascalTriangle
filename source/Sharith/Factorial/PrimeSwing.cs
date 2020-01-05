// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01


using Sharith.Primes;
using System;
using System.Buffers;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using XMath = Sharith.MathUtils.XMath;

// Same algorithm as PrimeSwing
// but computes swing(n) asynchronous.

namespace Sharith.Factorial
{
	public static class PrimeSwing
	{
		public static async ValueTask<BigInteger> FactorialAsync(int n)
		{
			if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), n, "Negative values not supported.");
			if (n < 20) return XMath.Factorial((byte)n);

			var sieve = new PrimeSieve(n);
			return await RecFactorialAsync(n).ConfigureAwait(false) << (n - XMath.BitCount(n)); ;

			async ValueTask<BigInteger> RecFactorialAsync(int i)
			{
				if (i < 2) return BigInteger.One;
				var swing = SwingAsync(sieve, i);
				var recFact = RecFactorialAsync(i / 2);
				await Task.Yield();
				var sqrFact = BigInteger.Pow(await recFact.ConfigureAwait(false), 2);
				return sqrFact * await swing.ConfigureAwait(false);
			}
		}

		public static ValueTask<BigInteger> FactorialAsync(ulong n)
			=> n > int.MaxValue
				? throw new ArgumentOutOfRangeException(nameof(n), n, "Greater than int.MaxValue not yet supported")
				: FactorialAsync((int)n);

		private static async ValueTask<BigInteger> SwingAsync(PrimeSieve sieve, int n)
		{
			if (n < SmallOddSwing.Length)
				return SmallOddSwing[n];

			var primorial = sieve.GetPrimorialAsync(n / 2 + 1, n);
			await Task.Yield();

			var count = 0;
			var rootN = XMath.FloorSqrt(n);
			var aPrimes = sieve.GetPrimeCollection(3, rootN);
			var bPrimes = sieve.GetPrimeCollection(rootN + 1, n / 3);
			var pool = ArrayPool<int>.Shared;
			var primeList = pool.Rent(aPrimes.NumberOfPrimes + bPrimes.NumberOfPrimes);

			foreach (var prime in aPrimes)
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

			foreach (var prime in bPrimes.Where(prime => ((n / prime) & 1) == 1))
			{
				primeList[count++] = prime;
			}

			var primeProduct = XMath.ProductAsync(primeList, 0, count);
			var result = await primeProduct.ConfigureAwait(false)
				* await primorial.ConfigureAwait(false);
			pool.Return(primeList);
			return result;
		}

		static readonly BigInteger[] SmallOddSwing = {
			1, 1, 1, 3, 3, 15, 5, 35, 35, 315, 63, 693, 231, 3003, 429, 6435, 6435,
			109395, 12155, 230945, 46189, 969969, 88179, 2028117, 676039, 16900975,
			1300075, 35102025, 5014575, 145422675, 9694845, 300540195, 300540195,
			9917826435, 583401555, 20419054425, 2268783825, 83945001525, 4418157975,
			172308161025, 34461632205, 1412926920405, 67282234305, 2893136075115,
			263012370465, 11835556670925, 514589420475, 24185702762325, 8061900920775,
			395033145117975, 15801325804719, 805867616040669, 61989816618513,
			3285460280781189, 121683714103007, 6692604275665385, 956086325095055,
			54496920530418135, 1879204156221315, 110873045217057585, 7391536347803839,
			450883717216034179, 14544636039226909, 916312070471295267, 916312070471295267 };
	}
}
