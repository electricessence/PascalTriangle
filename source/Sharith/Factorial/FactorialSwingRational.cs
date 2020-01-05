// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
	using System.Numerics;

	public class SwingRational : IFactorialFunction
	{
		public string Name => "SwingRational       ";

		long den, num, h;
		int i;

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

		private BigInteger Swing(int n)
		{
			switch (n % 4)
			{
				case 1: h = n / 2 + 1; break;
				case 2: h = 2; break;
				case 3: h = 2 * (n / 2 + 2); break;
				default: h = 1; break;
			}

			num = 2 * (n + 2 - ((n + 1) & 1));
			den = 1;
			i = n / 4;

			return Product(i + 1).Numerator;
		}

		private Rational Product(int l)
		{
			if (l > 1)
			{
				var m = l / 2;
				return Product(m) * Product(l - m);
			}

			return i-- > 0 ? new Rational(num -= 4, den++) : new Rational(h, 1);
		}

		//-------------------------------------------------------------
		// A minimalistic rational arithmetic *only* for the use of
		// SwingRationalDouble. The sole purpose for inclusion
		// here is to make the description of the algorithm more
		// independent and more easy to port.
		//-------------------------------------------------------------
		private class Rational
		{
			readonly BigInteger num; // Numerator
			readonly BigInteger den; // Denominator

			public BigInteger Numerator
			{
				get
				{
					var cd = BigInteger.GreatestCommonDivisor(num, den);
					return num / cd;
				}
			}

			public Rational(long _num, long _den)
			{
				long cd = Gcd(_den, _num);
				num = new BigInteger(_num / cd);
				den = new BigInteger(_den / cd);
			}

			public Rational(BigInteger _num, BigInteger _den)
			{
				// If (and only if) the arithmetic supports a
				// *real* fast Gcd this would lead to a speed up:
				// BigInteger cd = BigInteger.Gcd(_num, _den);
				// num = new BigInteger(_num / cd);
				// den = new BigInteger(_den / cd);
				num = _num;
				den = _den;
			}

			public static Rational operator *(Rational a, Rational r)
			{
				return new Rational(a.num * r.num, a.den * r.den);
			}

			private static long Gcd(long a, long b)
			{
				long x, y;

				if (a < b) { x = b; y = a; }
				else { x = a; y = b; }

				while (y != 0)
				{
					long t = x % y; x = y; y = t;
				}
				return x;
			}
		} // endOfRational
	}
} // endOfSwingRational
