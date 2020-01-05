// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
	using System.Numerics;

	public class ProductNaive : IFactorialFunction
	{
		public string Name => "ProductNaive        ";

		public BigInteger Factorial(int n)
		{
			if (n < 0)
			{
				throw new System.ArgumentOutOfRangeException(
					Name + ": " + nameof(n) + " >= 0 required, but was " + n);
			}

			var nFact = BigInteger.One;

			for (var i = 2; i <= n; i++)
			{
				nFact *= i;
			}
			return nFact;
		}
	}
} // endOfProductNaive
