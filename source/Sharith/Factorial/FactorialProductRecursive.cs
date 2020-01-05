// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
	using System.Numerics;

	public class ProductRecursive : IFactorialFunction
	{
		public string Name => "ProductRecursive    ";

		public BigInteger Factorial(int n)
		{
			if (n < 0)
			{
				throw new System.ArgumentOutOfRangeException(
					Name + ": " + nameof(n) + " >= 0 required, but was " + n);
			}

			return 1 < n ? RecProduct(1, n) : BigInteger.One;
		}

		private BigInteger RecProduct(int n, int len)
		{
			if (1 < len)
			{
				var l = len >> 1;
				return RecProduct(n, l) * RecProduct(n + l, len - l);
			}

			return new BigInteger(n);
		}
	}
} // endOfFactorialProductRecursive
