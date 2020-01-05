// -------- ToujoursEnBeta
// Author & Copyright : Peter Luschny
// License: LGPL version 3.0 or (at your option)
// Creative Commons Attribution-ShareAlike 3.0
// Comments mail to: peter(at)luschny.de
// Created: 2010-03-01

using System;
using System.Buffers;
using System.Numerics;
using System.Threading.Tasks;

namespace Sharith.MathUtils
{
	public static class XMath
	{
		/// <summary>
		/// Bit count
		/// </summary>
		/// <param name="w"></param>
		/// <returns>Number of bits.</returns>
		public static int BitCount(int w)
		{
			w -= (int)((0xaaaaaaaa & w) >> 1);
			w = (w & 0x33333333) + ((w >> 2) & 0x33333333);
			w = w + (w >> 4) & 0x0f0f0f0f;
			w += w >> 8;
			w += w >> 16;

			return w & 0xff;
		}

		/// <summary>
		/// Calculates the bit length of an integer.
		/// </summary>
		/// <param name="w"></param>
		/// <returns>bit length</returns>
		public static int BitLength(int w) =>
			w < 1 << 15
			? (w < 1 << 7 ? (w < 1 << 3 ? (w < 1 << 1 ? (w < 1 << 0 ? (w < 0 ? 32 : 0) : 1)
			: (w < 1 << 2 ? 2 : 3)) : (w < 1 << 5 ? (w < 1 << 4 ? 4 : 5) : (w < 1 << 6 ? 6 : 7)))
			: (w < 1 << 11 ? (w < 1 << 9 ? (w < 1 << 8 ? 8 : 9) : (w < 1 << 10 ? 10 : 11))
			: (w < 1 << 13 ? (w < 1 << 12 ? 12 : 13) : (w < 1 << 14 ? 14 : 15))))
			: (w < 1 << 23
			? (w < 1 << 19 ? (w < 1 << 17 ? (w < 1 << 16 ? 16 : 17) : (w < 1 << 18 ? 18 : 19))
			: (w < 1 << 21 ? (w < 1 << 20 ? 20 : 21) : (w < 1 << 22 ? 22 : 23)))
			: (w < 1 << 27
			? (w < 1 << 25 ? (w < 1 << 24 ? 24 : 25) : (w < 1 << 26 ? 26 : 27))
			: (w < 1 << 29 ? (w < 1 << 28 ? 28 : 29)
			: (w < 1 << 30 ? 30 : 31))));

		//public static ulong BitLength(BigInteger w)
		//{
		//	if (w <= byte.MaxValue)
		//		return (ulong)BitLength((byte)w);

		//	if (w <= int.MaxValue)
		//		return (ulong)BitLength((int)w);

		//	ulong bitLength = 0;
		//	do
		//	{
		//		bitLength++;
		//	}
		//	while ((w >>= 1) != 0);
		//	return bitLength;
		//}

		public static int BitLength(byte b)
			=> b < 8 ? b < 2 ? b < 1 ? 0 : 1 : b < 4 ? 2 : 3 : b < 32 ? b < 16 ? 4 : 5 : b < 64 ? 6 : 7;

		/// <summary>
		/// High bound for number of primes not exeeding n.
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static int PiHighBound(long n)
		{
			return n < 17
				? 6
				: (int)Math.Floor(n / (Math.Log(n) - 1.5));
		}

		/// <summary>
		/// Logarithm to base 2.
		/// </summary>
		/// <param name="value"></param>
		/// <returns><tt>Log<sub>2</sub>value</tt>.</returns>
		public static double Log2(double value)
			=> Math.Log(value) * 1.4426950408889634;

		/// <summary>
		/// Floor of the binary logarithm.
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static int FloorLog2(int n)
		{
			if (n <= 0) throw new ArgumentOutOfRangeException(nameof(n) + " >= 0 required");
			return BitLength(n) - 1;
		}

		/// <summary>
		/// Floor of the square root.
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static int FloorSqrt(int n)
		{
			if (n < 0) throw new ArgumentOutOfRangeException(nameof(n) + " >= 0 required");
			return (int)Math.Floor(Math.Sqrt(n));
		}

		/// <summary>
		/// Ceiling of the binary logarithm.
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static int CeilLog2(int n)
		{
			int ret = FloorLog2(n);
			if (n != (1 << ret)) ret++;
			return ret;
		}

		/// <summary>
		/// Berechnung der groessten Zahl, deren Quadrat n nicht uebersteigt.
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public static int Sqrt(int n)
		{
			if (n == 0) return 0;
			if (n < 0) throw new ArgumentOutOfRangeException(nameof(n) + " >= 0 required");
			
			int unten, oben = BitLength(n);

			do
			{
				unten = n / oben;
				oben += unten;
				oben >>= 1;
			}
			while (oben > unten);

			return oben;
		}

		public static int OmegaSwingHighBound(int n)
		{
			return n < 4
				? 6
				: (int)Math.Floor(Math.Sqrt(n) + n / (Log2(n) - 1));
		}

		public static double AsymptFactorial(double x)
		{
			// error of order O(x^-5)
			// double ln2Pi = 1.8378770664093455 = Math.log(2 * Math.PI);
			double a = x + x + 1;
			return (1.8378770664093455 + Math.Log(a / 2) * a - a
					- (1 - 7 / (30 * a * a)) / (6 * a)) / 2;
		}


		public static string Exp(double x)
		{
			double l = x * 0.43429448190325176D; // x/Math.log(10);
			double e = Math.Floor(l);
			double m = Math.Pow(10, l - e);
			string mat = (Convert.ToString(m)).Substring(0, 6);
			return mat + " E+" + Convert.ToString((int)e);
		}

		private static readonly long[] SmallFactorials = {
			1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800,
			479001600, 6227020800, 87178291200, 1307674368000, 20922789888000,
			355687428096000, 6402373705728000, 121645100408832000, 2432902008176640000 };

		public static BigInteger Factorial(int n)
		{
			if (n < 0)
				throw new ArgumentOutOfRangeException(nameof(n), n, "Must be at least 0");

			return Factorial((byte)n);
		}

		public static BigInteger Factorial(byte n)
		{
			if (n > SmallFactorials.Length)
				throw new ArgumentOutOfRangeException("max n is 20 but was " + n.ToString());

			return new BigInteger(SmallFactorials[n]);
		}

		const int ParallelThreshold = 1024;

		// <returns>a[start]*a[start+1]*...*a[start+length-1]</returns>
		public static async ValueTask<BigInteger> ProductAsync(int[] a, int start, int length)
		{
			if (length == 0) return BigInteger.One;
			await Task.Yield();

			int len = (length + 1) / 2;
			var pool = ArrayPool<long>.Shared;
			var b = pool.Rent(len);
			try
			{
				int i, j, k;

				for (k = 0, i = start, j = start + length - 1; i < j; i++, k++, j--)
				{
					b[k] = a[i] * (long)a[j];
				}

				if (i == j) b[k++] = a[j];

				if (k < ParallelThreshold)
					return await RecProductAsync(b, 0, k - 1);

				var left = RecProductAsync(b, 0, (k - 1) / 2);
				var right = RecProductAsync(b, ((k - 1) / 2) + 1, k - 1);
				return await left.ConfigureAwait(false)
					* await right.ConfigureAwait(false);
			}
			finally
			{
				pool.Return(b);
			}
		}

		public static async ValueTask<BigInteger> ProductAsync(int[] a, int start, int length, int increment)
		{
			if (length == 0) return BigInteger.One;
			await Task.Yield();

			int len = (1 + (length + 1) / 2) / increment;
			var pool = ArrayPool<long>.Shared;
			var b = pool.Rent(len);
			try
			{
				int i, k = 0;
				bool toggel = false;

				for (i = start; i < start + length; i += increment)
				{
					if ((toggel = !toggel)) b[k] = a[i];
					else b[k++] *= a[i];
				}

				if (len < ParallelThreshold)
					return await RecProductAsync(b, 0, len - 1);

				var left = RecProductAsync(b, 0, (len - 1) / 2);
				var right = RecProductAsync(b, ((len - 1) / 2) + 1, len - 1);
				return await left.ConfigureAwait(false)
					* await right.ConfigureAwait(false);
			}
			finally
			{
				pool.Return(b);
			}
		}

		public static ValueTask<BigInteger> ProductAsync(int[] a)
			=> ProductAsync(a, 0, a.Length);

		public static async ValueTask<BigInteger> ProductAsync(long[] a, int len)
		{
			int n = len - 1;
			if (len < ParallelThreshold)
				return await RecProductAsync(a, 0, n);

			await Task.Yield();
			var left = RecProductAsync(a, 0, n / 2);
			var right = RecProductAsync(a, n / 2 + 1, n);
			return await left.ConfigureAwait(false)
				* await right.ConfigureAwait(false);
		}

		public static ValueTask<BigInteger> ProductAsync(long[] a)
			=> ProductAsync(a, a.Length);

		public static async ValueTask<BigInteger> RecProductAsync(long[] s, int n, int m)
		{
			if (n == m) return new BigInteger(s[n]);
			if (n > m) return BigInteger.One;
			await Task.Yield();

			int k = (n + m) >> 1;
			var left = RecProductAsync(s, n, k);
			var right = RecProductAsync(s, k + 1, m);
			return await left.ConfigureAwait(false)
				* await right.ConfigureAwait(false);
		}

		private static async ValueTask<BigInteger> BigRecProductAsync(BigInteger[] s, int n, int m)
		{
			if (n == m) return s[n];
			if (n > m) return BigInteger.One;

			await Task.Yield();
			int k = (n + m) >> 1;
			var left = BigRecProductAsync(s, n, k);
			var right = BigRecProductAsync(s, k + 1, m);
			return await left.ConfigureAwait(false)
				* await right.ConfigureAwait(false);
		}

		public static async ValueTask<BigInteger> ProductAsync(BigInteger[] a, int start, int len)
		{
			int n = len - 1;
			if (len < ParallelThreshold)
				return await BigRecProductAsync(a, start, n);

			await Task.Yield();
			var left = BigRecProductAsync(a, start, start + n / 2);
			var right = BigRecProductAsync(a, start + (n / 2) + 1, start + n);
			return await left.ConfigureAwait(false)
				* await right.ConfigureAwait(false);
		}

		public static ValueTask<BigInteger> ProductAsync(BigInteger[] a)
			=> BigRecProductAsync(a, 0, a.Length - 1);

	}
}


