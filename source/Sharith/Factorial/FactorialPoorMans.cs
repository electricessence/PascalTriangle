// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

// The Poor Man's Implementation of the factorial.
// All math is on board, no additional libraries
// are needed. Good enough to compute the factorial
// up to n=10000 in a few seconds.

namespace Sharith.Factorial
{
	public class FactorialPoorMans
	{
		private long n;

		public string Factorial(int n)
		{
			if (n < 0)
			{
				throw new System.ArgumentException(message: nameof(n) + " >= 0 required, but was " + n);
			}

			if (n < 2)
			{
				return "1";
			}

			var p = new DecInteger(1);
			var r = new DecInteger(1);

			this.n = 1;

			int h = 0, shift = 0, high = 1;
			var log2N = (int)System.Math.Floor(System.Math.Log(n) * 1.4426950408889634);

			while (h != n)
			{
				shift += h;
				h = n >> log2N--;
				var len = high;
				high = (h - 1) | 1;
				len = (high - len) / 2;

				if (len > 0)
				{
					p *= Product(len);
					r *= p;
				}
			}

			r *= DecInteger.Pow2(shift);
			return r.ToString();
		}

		private DecInteger Product(int n)
		{
			var m = n / 2;
			if (m == 0)
			{
				return new DecInteger(this.n += 2);
			}

			return n == 2 ? new DecInteger((this.n += 2) * (this.n += 2)) : Product(n - m) * Product(m);
		}
	} // endOfFactorialPoorMans

	internal class DecInteger
	{
		private const long Mod = 100000000L;
		private readonly int[] digits;
		private readonly int digitsLength;

		public DecInteger(long value)
		{
			digits = new int[] { (int)value, (int)(value >> 32) };
			digitsLength = 2;
		}

		private DecInteger(int[] digits, int length)
		{
			this.digits = digits;
			digitsLength = length;
		}

		public static DecInteger Pow2(int e)
		{
			return e < 31 ? new DecInteger((int)System.Math.Pow(2, e)) : Pow2(e / 2) * Pow2(e - e / 2);
		}

		public static DecInteger operator *(DecInteger a, DecInteger b)
		{
			int alen = a.digitsLength, blen = b.digitsLength;
			var clen = alen + blen + 1;
			var digits = new int[clen];

			for (var i = 0; i < alen; i++)
			{
				long temp = 0;
				for (var j = 0; j < blen; j++)
				{
					temp = temp + (a.digits[i] * (long)b.digits[j]) + digits[i + j];
					digits[i + j] = (int)(temp % Mod);
					temp /= Mod;
				}
				digits[i + blen] = (int)temp;
			}

			var k = clen - 1;
			while (digits[k] == 0)
			{
				k--;
			}

			return new DecInteger(digits, k + 1);
		}

		public override string ToString()
		{
			var sb = new System.Text.StringBuilder(digitsLength * 10);
			sb = sb.Append(digits[digitsLength - 1]);
			for (var j = digitsLength - 2; j >= 0; j--)
			{
				sb = sb.Append((digits[j] + (int)Mod).ToString().Substring(1));
			}

			return sb.ToString();
		}
	}
}

// public static void Main (string[] arguments)
// {
//    int n = 1000;
//    if (arguments.Length != 0)
//    {
//        n = System.Convert.ToInt32(arguments[0]);
//    }
//    else
//    {
//        System.Console.WriteLine("Please give an argument!");
//    }
//    FactorialPoorMans f = new FactorialPoorMans();
//    System.Console.WriteLine(n + "! = " + f.Factorial(n));
//    System.Console.ReadLine();
// }
