using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace PascalTriangle
{

	public class Row
	{
		// We'll use a concurrent dictionary:
		// 1) to avoid creating entries we don't care about.
		// 2) to help with concurrency in parallel situations.
		// 3) using ulong instead of int.
		readonly ConcurrentDictionary<ulong, Lazy<Task<BigInteger>>> Values = new ConcurrentDictionary<ulong, Lazy<Task<BigInteger>>>();

		public Row(ulong number, Collection collection)
		{
			Number = number; // Also corresponds to the number entries in the row.
			Rows = collection ?? throw new ArgumentNullException(nameof(collection));
			Mid = number / 2 + number % 2; // 1 = 1, 2 = 1, 3 = 2, 4 = 2, 5 = 3 etc..
		}

		public readonly ulong Number;
		readonly ulong Mid;
		public Collection Rows { get; }

		public async ValueTask<BigInteger> GetValueAt(ulong index)
		{
			if (index <= 0) return BigInteger.Zero;
			if (index == 1 || index == Number) return BigInteger.One;
			if (index > Number) return BigInteger.Zero;
			// dedupe the index:
			if (index > Mid) index = Number - index + 1;

			return await Values.GetOrAdd(index, GetEntry).Value.ConfigureAwait(false); // By using a Lazy we guarantee pessimistic concurrency (only 1 task runs).

			Lazy<Task<BigInteger>> GetEntry(ulong key)
				 => new Lazy<Task<BigInteger>>(() => GetValue(key));

			async Task<BigInteger> GetValue(ulong key)
			{
				await Task.Yield(); // Here's the key to avoiding stack overflow.  Tasks get added to the scheduler instead of the stack.
				var row = Rows.GetRowAt(Number - 1);
				var a = row.GetValueAt(key - 1);
				var b = row.GetValueAt(key);
				return await a.ConfigureAwait(false) + await b.ConfigureAwait(false);
			}
		}

		public ValueTask<BigInteger> this[ulong index] => GetValueAt(index);

		public class Collection : IEnumerable<Row>
		{
			readonly ConcurrentDictionary<ulong, Row> Rows = new ConcurrentDictionary<ulong, Row>();

			public Row GetRowAt(ulong index)
				=> Rows.GetOrAdd(index, key => new Row(key, this));

			public Row this[ulong index] => GetRowAt(index);

			public IEnumerator<Row> GetEnumerator()
			{
				ulong i = 0;
				do
				{
					i++;
					yield return GetRowAt(i);
				}
				while (i != ulong.MaxValue);
			}
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}
	}

}
