using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace PascalTriangle
{
	public class Factorial : IEnumerable<BigInteger>
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

		ulong Index = 0;
		BigInteger Value = BigInteger.One;

		readonly Dictionary<ulong, BigInteger> Values
			= new Dictionary<ulong, BigInteger>(1024) { { 0, BigInteger.One } };

		public BigInteger Of(ulong index)
		{
			while (Index < index && Index < ulong.MaxValue)
			{
				lock (Values)
				{
					if (Index >= index)
						break;

					Value *= 1 + Index++;
					Values.Add(Index, Value);
					if (Index == index)
						return Value;
				}
			}

			if (Values.TryGetValue(index, out var result))
				return result;

			throw new Exception("Value missing.");
		}

		public Task<BigInteger> OfAsync(ulong index)
			=> Task.Run(() => Of(index));

		public IEnumerator<BigInteger> GetEnumerator()
		{
			ulong i = 0;

		next:
			yield return Of(i);
			if (i == ulong.MaxValue) yield break;
			++i;
			goto next;
		}

		IEnumerator IEnumerable.GetEnumerator()
			=> GetEnumerator();
	}
}
