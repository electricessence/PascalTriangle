using System.Numerics;
using System.Threading.Tasks;
using Xunit;

namespace PascalTriangle.Tests
{
	public class TriangleTests
	{
		/*
		 0 					 1
		 1				   1   1
		 2				 1 	 2 	 1
		 3			   1   3   3   1
		 4			 1 	 4 	 6 	 4 	 1
		 5		   1   5  10  10   5   1
		 6		 1   6  15 	20 	15 	 6   1
		 7	   1   7  21  35  35  21   7   1
		 */

		[Fact]
		public async Task BasicExpectationTests()
		{
			Assert.Equal(BigInteger.One, await Triangle.ValueAtAsync(0, 0));
			Assert.Equal(BigInteger.Zero, await Triangle.ValueAtAsync(0, 1));
			Assert.Equal(BigInteger.One, await Triangle.ValueAtAsync(1, 0));
			Assert.Equal(BigInteger.One, await Triangle.ValueAtAsync(2, 0));
			Assert.Equal(BigInteger.One, await Triangle.ValueAtAsync(1, 1));
			Assert.Equal(BigInteger.One, await Triangle.ValueAtAsync(3, 3));
			Assert.Equal(2, await Triangle.ValueAtAsync(2, 1));
			Assert.Equal(3, await Triangle.ValueAtAsync(3, 1));
			Assert.Equal(6, await Triangle.ValueAtAsync(4, 2));
			Assert.Equal(4, await Triangle.ValueAtAsync(4, 3));
			Assert.Equal(15, await Triangle.ValueAtAsync(6, 2));
			Assert.Equal(35, await Triangle.ValueAtAsync(7, 3));
		}
	}
}
