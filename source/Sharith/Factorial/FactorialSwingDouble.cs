// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

namespace Sharith.Factorial
{
	using System.Numerics;

	public class SwingDouble : IFactorialFunction
	{
		public string Name => "SwingDouble         ";

		BigInteger f;
		long gN;

		public BigInteger Factorial(int n)
		{
			if (n < 0)
			{
				throw new System.ArgumentOutOfRangeException(
					Name + ": " + nameof(n) + " >= 0 required, but was " + n);
			}

			gN = 1;
			f = BigInteger.One;
			return RecFactorial(n);
		}

		private BigInteger RecFactorial(int n)
		{
			return n < 2 ? BigInteger.One : BigInteger.Pow(RecFactorial(n / 2), 2) * Swing(n);
		}

		private BigInteger Swing(long n)
		{
			var s = gN - 1 + ((n - gN + 1) % 4);
			bool oddN = (gN & 1) != 1;

			for (; gN <= s; gN++)
			{
				if (oddN = !oddN) f *= gN;
				else f = (f * 4) / gN;
			}

			if (oddN) for (; gN <= n; gN += 4)
				{
					var m = ((gN + 1) * (gN + 3)) << 1;
					var d = (gN * (gN + 2)) >> 3;

					f = (f * m) / d;
				}
			else for (; gN <= n; gN += 4)
				{
					var m = (gN * (gN + 2)) << 1;
					var d = ((gN + 1) * (gN + 3)) >> 3;

					f = (f * m) / d;
				}

			return f;
		}
	}
} // endOfSwingDouble
