// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2004-03-01

/////////////////////////////////
//// buggy for large values of n
/////////////////////////////////
namespace Sharith.Factorial
{
	using System.Numerics;

	public class Hyper : IFactorialFunction
	{
		public string Name => "Hyper               ";

		bool nostart;
		long s, k, a;

		public BigInteger Factorial(int n)
		{
			if (n < 0)
			{
				throw new System.ArgumentOutOfRangeException(
					Name + ": " + nameof(n) + " >= 0 required, but was " + n);
			}

			nostart = false;
			var h = n / 2;
			s = h + 1;
			k = s + h;
			a = (n & 1) == 1 ? k : 1;
			if ((h & 1) == 1) a = -a;
			k += 4;

			return HyperFact(h + 1) << h;
		}

		private BigInteger HyperFact(int l)
		{
			if (l > 1)
			{
				var m = l >> 1;
				return HyperFact(m) * HyperFact(l - m);
			}

			if (nostart)
			{
				s -= k -= 4;
				return s;
			}
			nostart = true;
			return a;
		}
	}
} // endOfFactorialHyper
