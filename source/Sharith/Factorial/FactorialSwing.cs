// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
	using System;
	using System.Numerics;
	using XMath = MathUtils.XMath;
	public class Swing : IFactorialFunction
	{
		public string Name => "Swing               ";

		private BigInteger oddFactNdiv4, oddFactNdiv2;
		private const int Smallswing = 33;
		private const int Smallfact = 17;

		public BigInteger Factorial(int n)
		{
			if (n < 0)
			{
				throw new ArithmeticException(
					Name + ": " + nameof(n) + " >= 0 required, but was " + n);
			}

			oddFactNdiv4 = oddFactNdiv2 = BigInteger.One;

			return OddFactorial(n) << (n - XMath.BitCount(n));
		}

		private BigInteger OddFactorial(int n)
		{
			BigInteger oddFact;

			if (n < Smallfact)
			{
				oddFact = SmallOddFactorial[n];
			}
			else
			{
				var sqrOddFact = OddFactorial(n / 2);
				var ndiv4 = n / 4;
				var oddFactNd4 = ndiv4 < Smallfact
							   ? SmallOddFactorial[ndiv4]
							   : oddFactNdiv4;

				oddFact = BigInteger.Pow(sqrOddFact, 2) * OddSwing(n, oddFactNd4);
			}

			oddFactNdiv4 = oddFactNdiv2;
			oddFactNdiv2 = oddFact;
			return oddFact;
		}

		static BigInteger OddSwing(int n, BigInteger oddFactNdiv4)
		{
			if (n < Smallswing) return SmallOddSwing[n];

			var len = (n - 1) / 4;
			if ((n % 4) != 2) len++;
			var high = n - ((n + 1) & 1);

			return Product(high, len) / oddFactNdiv4;
		}

		static BigInteger Product(int m, int len)
		{
			if (len == 1) return new BigInteger(m);
			if (len == 2) return new BigInteger((long)m * (m - 2));

			var hlen = len >> 1;
			return Product(m - hlen * 2, len - hlen) * Product(m, hlen);
		}

		static readonly BigInteger[] SmallOddSwing = {
			1,1,1,3,3,15,5,35,35,315,63,693,231,3003,429,6435,6435,109395,
			12155,230945,46189,969969,88179,2028117,676039,16900975,1300075,
			35102025,5014575,145422675,9694845,300540195,300540195 };

		static readonly BigInteger[] SmallOddFactorial = {
			1,1,1,3,3,15,45,315,315,2835,14175,155925,467775,6081075,
			42567525,638512875,638512875 };

	} // endOfFactorialSwing
}