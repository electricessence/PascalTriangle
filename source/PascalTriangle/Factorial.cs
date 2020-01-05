using Sharith.Factorial;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace PascalTriangle
{
	public static class Factorial
	{
		public static IEnumerator<BigInteger> GetSequenceEnumerator()
		{
			yield return BigInteger.One;

			ulong i = 1;
			var result = BigInteger.One;

		next:
			yield return result;
			result *= ++i;
			goto next;
		}

		public static IEnumerable<BigInteger> GetSequence()
		{
			using var e = GetSequenceEnumerator();
			while (e.MoveNext())
				yield return e.Current;
		}


		public static ValueTask<BigInteger> OfAsync(ulong value)
			=> PrimeSwing.FactorialAsync(value);


	}
}
