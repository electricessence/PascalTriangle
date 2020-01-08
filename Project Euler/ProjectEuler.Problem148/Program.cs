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
		static readonly ulong MaxRow = 1_000_000_000;
		static void Main(string[] _)
		{
			BigInteger total = BigInteger.Zero;
			//// Don't store 1s or row digits.
			//// Setup first row that matters.
			var previousRow = new LinkedList<BigInteger>();
			previousRow.AddLast(BigInteger.One);

			var nextRow = new LinkedList<BigInteger>();
			nextRow.AddLast(BigInteger.Zero);
			/////////////////////////////////

			while (previousRow.First!.Value < MaxRow)
			{
				var count = FillNextRowFromPrevious(previousRow, nextRow);

				total += count;
				var row = nextRow.First!.Value;
				if (row % 1000 == 0)
					Console.WriteLine("Row({0}): {2}", row, count, total);

				var swap = previousRow;
				previousRow = nextRow;
				nextRow = swap;
			}

			Console.WriteLine("Row({0}): {1}", MaxRow, total);
		}

		static long FillNextRowFromPrevious(
			LinkedList<BigInteger> previousRow,
			LinkedList<BigInteger> nextRow)
		{
			long foundCount = 0;
			Parallel.ForEach(Nodes(previousRow, nextRow),
				nodes =>
				nodes.current.Value
					= IncrementIfFound(NextRowValue(nodes.previous)));

			// Both sides of the triangle.
			foundCount += foundCount;

			// Even rows take the previous value and double it.
			if (nextRow.First!.Value % 2 == 0)
			{
				var last = previousRow.Last!.Value;
				nextRow.AddLast(IncrementIfFound(last + last));
			}

			return foundCount;

			BigInteger IncrementIfFound(in BigInteger value)
			{
				if (value % SearchingFor == 0)
					Interlocked.Increment(ref foundCount);
				return value;
			}
		}

		static IEnumerable<(LinkedListNode<BigInteger> previous, LinkedListNode<BigInteger> current)> Nodes(
			LinkedList<BigInteger> previousRow,
			LinkedList<BigInteger> nextRow)
		{
			LinkedListNode<BigInteger>? pN = previousRow.First!;
			LinkedListNode<BigInteger>? nN = nextRow.First!;

			yield return (pN, nN);

			while ((pN = pN.Next!) != null)
			{
				nN = nN.Next ?? nextRow.AddLast(BigInteger.Zero);
				yield return (pN, nN);
			}
		}

		static BigInteger NextRowValue(
			LinkedListNode<BigInteger> previousRowNode)
			=> previousRowNode.Value
			 + (previousRowNode.Previous?.Value ?? BigInteger.One);
	}
}
