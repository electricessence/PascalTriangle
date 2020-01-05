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

		readonly Row.Collection TriangleRowCollection = new Row.Collection();

		[Theory]
		[InlineData(1, 0, 0)]
		[InlineData(1, 1, 0)]
		[InlineData(1, 2, 0)]
		[InlineData(1, 1, 1)]
		[InlineData(1, 3, 3)]
		[InlineData(2, 2, 1)]
		[InlineData(3, 3, 1)]
		[InlineData(6, 4, 2)]
		[InlineData(4, 4, 3)]
		[InlineData(15, 6, 2)]
		[InlineData(35, 7, 3)]
		public async Task A_StaticCalculationTests(ulong expected, ulong row, ulong column)
		{
			Assert.Equal(expected, await Triangle.ValueAtAsync(row, column));
		}

		[Theory]
		[InlineData(1, 0, 0)]
		[InlineData(1, 1, 0)]
		[InlineData(1, 2, 0)]
		[InlineData(1, 1, 1)]
		[InlineData(1, 3, 3)]
		[InlineData(2, 2, 1)]
		[InlineData(3, 3, 1)]
		[InlineData(6, 4, 2)]
		[InlineData(4, 4, 3)]
		[InlineData(15, 6, 2)]
		[InlineData(35, 7, 3)]
		public async Task B_TriangleRowCollectionTests(ulong expected, ulong row, ulong column)
		{
			Assert.Equal(expected, await TriangleRowCollection[row][column]);
		}

		[Fact]
		public async Task B_First100Rows()
		{
			for (ulong row = 0; row < 100; ++row)
			{
				for (ulong column = 0; column < row; ++column)
				{
					Assert.Equal(
						await Triangle.ValueAtAsync(row, column),
						await TriangleRowCollection[row][column]);
				}
			}
		}
	}
}
