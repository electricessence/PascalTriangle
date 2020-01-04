using PascalTriangle;
using System.Numerics;
using System.Threading.Tasks;
using Xunit;

namespace PascalsTriangle.Tests
{
	public class BasicTests
	{
		[Fact]
		public async Task BasicExpectationTests()
		{
			// https://projecteuler.net/problem=148
			var rows = new Row.Collection();
			Assert.Equal(BigInteger.Zero, await rows[0][0]);
			Assert.Equal(BigInteger.Zero, await rows[1][0]);
			Assert.Equal(BigInteger.Zero, await rows[1][2]);
			Assert.Equal(BigInteger.One, await rows[1][1]);
			Assert.Equal(BigInteger.One, await rows[2][1]);
			Assert.Equal(BigInteger.One, await rows[3][1]);
			Assert.Equal(BigInteger.One, await rows[3][3]);
			Assert.Equal(BigInteger.One, await rows[4][4]);
			Assert.Equal(2, await rows[3][2]);
			Assert.Equal(15, await rows[7][5]);
		}
	}
}
