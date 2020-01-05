// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
	using Sharith.Primes;
	using System;
	using System.Linq;
	using System.Numerics;
	using System.Threading.Tasks;
	using XMath = MathUtils.XMath;

	public class ParallelPrimeSplit : IFactorialFunction
	{
		PrimeSieve sieve;

		public string Name => "ParallelPrimeSplit  ";

		delegate BigInteger SwingDelegate(int n);

		public BigInteger Factorial(ulong n)
		{
			if (n > int.MaxValue)
				throw new ArgumentOutOfRangeException(nameof(n), n, "Not yet supported above int.MaxValue.");
			return Factorial((int)n);
		}

		public BigInteger Factorial(int n)
		{
			if (n < 20) { return XMath.Factorial((byte)n); }

			sieve = new PrimeSieve(n);
			var log2N = XMath.FloorLog2(n);

			SwingDelegate swingDelegate = Swing;
			var results = new IAsyncResult[log2N];

			int h = 0, shift = 0, taskCounter = 0;

			// -- It is more efficient to add the big intervals
			// -- first and the small ones later!
			while (h != n)
			{
				shift += h;
				h = n >> log2N--;
				if (h > 2)
				{
					results[taskCounter++] = swingDelegate.BeginInvoke(h, null, null);
				}
			}

			BigInteger p = BigInteger.One, r = BigInteger.One, rl = BigInteger.One;

			for (var i = 0; i < taskCounter; i++)
			{
				var t = rl * swingDelegate.EndInvoke(results[i]);
				p *= t;
				rl = r;
				r *= p;
			}

			return r << shift;
		}

		private BigInteger Swing(int n)
		{
			if (n < 33) return SmallOddSwing[n];

			var primorial = Task.Factory.StartNew(() => sieve.GetPrimorial(n / 2 + 1, n));
			var count = 0;
			var rootN = XMath.FloorSqrt(n);
			var aPrimes = sieve.GetPrimeCollection(3, rootN);
			var bPrimes = sieve.GetPrimeCollection(rootN + 1, n / 3);
			var piN = aPrimes.NumberOfPrimes + bPrimes.NumberOfPrimes;
			var primeList = new int[piN];

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

			var primeProduct = XMath.Product(primeList, 0, count);
			return primeProduct * primorial.Result;
		}

		static readonly BigInteger[] SmallOddSwing = {
			1,1,1,3,3,15,5,35,35,315,63,693,231,3003,429,6435,6435,109395,
			12155,230945,46189,969969,88179,2028117,676039,16900975,1300075,
			35102025,5014575,145422675,9694845,300540195,300540195 };
	}
}
