using System.Numerics;
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
		public void BasicExpectationTests()
		{
			// https://projecteuler.net/problem=148
			var triangle = new Triangle();
			Assert.Equal(BigInteger.One, triangle.ValueAt(0, 0));
			Assert.Equal(BigInteger.Zero, triangle.ValueAt(0, 1));
			Assert.Equal(BigInteger.One, triangle.ValueAt(1, 0));
			Assert.Equal(BigInteger.One, triangle.ValueAt(2, 0));
			Assert.Equal(BigInteger.One, triangle.ValueAt(1, 1));
			Assert.Equal(BigInteger.One, triangle.ValueAt(3, 3));
			Assert.Equal(2, triangle.ValueAt(2, 1));
			Assert.Equal(3, triangle.ValueAt(3, 1));
			Assert.Equal(6, triangle.ValueAt(4, 2));
			Assert.Equal(4, triangle.ValueAt(4, 3));
			Assert.Equal(15, triangle.ValueAt(6, 2));
			Assert.Equal(35, triangle.ValueAt(7, 3));
		}
	}
}
