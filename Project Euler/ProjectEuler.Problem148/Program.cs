using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectEuler.Problem148
{
	class Program
	{
		static readonly BigInteger SearchingFor = 7;
		static readonly int MaxRow = 1_000_000_000;

		static void Main(string[] _)
		{
			BigInteger total = BigInteger.Zero;
			//// Don't store 1s or row digits.
			//// Setup first row that matters.
			int foundCount = SearchingFor == 6 ? 1 : 0;
			int rowIndex = 4;

			var previousRow = new LinkedList<BigInteger>();
			previousRow.AddLast(6);

			var nextRow = new LinkedList<BigInteger>();
			nextRow.AddLast(BigInteger.Zero);
			/////////////////////////////////

		loop:

			IncrementIfFound(rowIndex);
			total += foundCount;
			if (foundCount != 0 && rowIndex % 1000 == 0)
			{
				Console.WriteLine("Row({0}): {2}", rowIndex, foundCount, total);
			}
			//Console.WriteLine(string.Join(' ', previousRow));
			//Console.ReadKey();

			foundCount = 0;
			if (rowIndex == MaxRow) return;

			Parallel.ForEach(Nodes(), nodes =>
			{
				var (pN, nN) = nodes;
				BigInteger left = pN.Previous?.Value ?? rowIndex;
				BigInteger right = pN.Value;
				nN.Value = IncrementIfFound(left + right, 2);
			});

			++rowIndex;

			if (rowIndex % 2 == 0)
			{
				var last = previousRow.Last.Value;
				nextRow.AddLast(last + last);
			}

			var swap = previousRow;
			previousRow = nextRow;
			nextRow = swap;

			Console.WriteLine("Row({0}): {1}", rowIndex, total);

			goto loop;

			BigInteger IncrementIfFound(in BigInteger value, int count = 1)
			{
				if (value % SearchingFor == 0)
				{
					for (var i = 0; i < count; i++)
						Interlocked.Increment(ref foundCount);
				}
				return value;
			}

			IEnumerable<(LinkedListNode<BigInteger> p, LinkedListNode<BigInteger> n)> Nodes()
			{
				var pN = previousRow.First;
				var nN = nextRow.First;
				yield return (pN, nN);

				while ((pN = pN.Next) != null)
				{
					nN = nN.Next ?? nextRow.AddLast(BigInteger.Zero);
					yield return (pN, nN);
				}
			}

		}
	}
}
