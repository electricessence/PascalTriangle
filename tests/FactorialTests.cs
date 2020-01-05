using System.Numerics;
using System.Threading.Tasks;
using Xunit;

namespace PascalTriangle.Tests
{
	public class FactorialTests
	{
		static readonly BigInteger[] Expected = new BigInteger[] { 1, 1, 2, 6, 24, 120, 720 };
		[Fact]
		public static async Task SequenceTests()
		{
			{
				using var e = Factorial.GetSequenceEnumerator();
				for (var i = 0; i < Expected.Length; ++i)
				{
					e.MoveNext();
					Assert.Equal(Expected[i], e.Current);
				}
			}
			{
				using var e = Factorial.GetSequenceEnumerator();
				for (ulong i = 0; i < 1000; ++i)
				{
					e.MoveNext();
					Assert.Equal(e.Current, await Factorial.OfAsync(i));
				}
			}
		}


		[Fact]
		public static async Task InstanceReverseLookupTests()
		{
			for (ulong i = (ulong)Expected.Length; i > 0; --i)
			{
				Assert.Equal(Expected[i - 1], await Factorial.OfAsync(i - 1));
			}
		}

	}
}
