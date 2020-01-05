// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
	using System.Numerics;

	public class AdditiveSwing : IFactorialFunction
	{
		public string Name => "AdditiveSwing       ";

		public BigInteger Factorial(int n)
		{
			if (n < 0)
			{
				throw new System.ArgumentOutOfRangeException(
					Name + ": " + nameof(n) + " >= 0 required, but was " + n);
			}

			return RecFactorial(n);
		}

		private BigInteger RecFactorial(int n)
		{
			return n < 2 ? BigInteger.One : BigInteger.Pow(RecFactorial(n / 2), 2) * Swing(n);
		}

		static BigInteger Swing(int n)
		{
			var w = BigInteger.One;

			if (n > 1)
			{
				n += 2;
				var s = new BigInteger[n + 1];

				s[0] = s[1] = BigInteger.Zero;
				s[2] = w;

				for (var m = 3; m <= n; m++)
				{
					s[m] = s[m - 2];
					for (var k = m; k >= 2; k--)
					{
						s[k] += s[k - 2];
						if ((k & 1) == 1) // if k is odd
						{
							s[k] += s[k - 1];
						}
					}
				}
				w = s[n];
			}
			return w;
		}
	}
} // endOfFactorialAdditiveSwing
