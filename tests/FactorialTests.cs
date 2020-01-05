using System.Numerics;
using Xunit;

namespace PascalTriangle.Tests
{
	public class FactorialTests
	{
		static readonly BigInteger[] Expected = new BigInteger[] { 1, 1, 2, 6, 24, 120, 720 };
		[Fact]
		public static void SequenceTests()
		{
			using var e = Factorial.GetSequenceEnumerator();
			for (var i = 0; i < Expected.Length; ++i)
			{
				e.MoveNext();
				Assert.Equal(Expected[i], e.Current);
			}
		}


		[Fact]
		public static void InstanceReverseLookupTests()
		{
			for (ulong i = (ulong)Expected.Length; i > 0; --i)
			{
				Assert.Equal(Expected[i - 1], Factorial.Of(i - 1));
			}
		}

	}
}
