using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

		public ulong Number { get; }
		readonly ulong Mid;
		public Collection Rows { get; }

		public bool IsNonComputeValue(ref ulong index, out BigInteger value)
		{
			value = BigInteger.One;
			if (index == 0 || index == Number) return true;
			if (index > Number) throw new ArgumentOutOfRangeException(nameof(index), index, "Value is greater than the number of entries in the row.");
			if (index > Mid) index = Number - index;
			if (index == 1)
			{
				value = Number;
				return true;
			}

			return false;
		}

		public bool TryGetValue(ulong index, [NotNullWhen(true)] out Task<BigInteger>? result)
		{
			if (IsNonComputeValue(ref index, out var value))
			{
				result = Task.FromResult(value);
				return true;
			}

			if (Values.TryGetValue(index, out var lazy))
			{
				result = lazy.Value;
				return true;
			}

			result = null;
			return false;
		}

		public async ValueTask<BigInteger> GetValueAtAsync(ulong index, ulong crawlDepth = 2)
		{
			if (IsNonComputeValue(ref index, out var value))
				return value;

			return await Values
				.GetOrAdd(index, GetEntry)
				.Value // By using a Lazy we guarantee pessimistic concurrency (only 1 task runs);
				.ConfigureAwait(false);

			Lazy<Task<BigInteger>> GetEntry(ulong key)
				 => new Lazy<Task<BigInteger>>(() => GetValue(key));

			async Task<BigInteger> GetValue(ulong key)
			{
				await Task.Yield(); // Here's the key to avoiding stack overflow.  Tasks get added to the scheduler instead of the stack.
				var previousRow = Rows.GetRowAt(Number - 1);

				// Both predecessors are availalbe?
				previousRow.TryGetValue(key - 1, out var aEntry);
				if (previousRow.TryGetValue(key, out var bEntry) && aEntry != null)
					return await aEntry.ConfigureAwait(false) + await bEntry.ConfigureAwait(false);

				// Neither?
				if (crawlDepth == 0 || aEntry is null && bEntry is null)
					return await Triangle.ValueAtAsync(Number, key);

				// At this point, crawldepth is greater than zero and one of the entries is not null.
				--crawlDepth;
				var a = previousRow.GetValueAtAsync(key - 1, crawlDepth);
				var b = previousRow.GetValueAtAsync(key, crawlDepth);
				return await a.ConfigureAwait(false) + await b.ConfigureAwait(false);
			}
		}

		public ValueTask<BigInteger> this[ulong index] => GetValueAtAsync(index);

		public async ValueTask PreloadAllValues()
		{
			for (ulong i = 2; i <= Mid; i++)
				await GetValueAtAsync(i);
		}

		public class Collection : IEnumerable<Row>
		{
			readonly ConcurrentDictionary<ulong, Row> Rows = new ConcurrentDictionary<ulong, Row>();

			public Row GetRowAt(ulong index)
				=> Rows.GetOrAdd(index, key => new Row(key, this));

			public bool RemoveRow(ulong index)
				=> Rows.TryRemove(index, out _);

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
