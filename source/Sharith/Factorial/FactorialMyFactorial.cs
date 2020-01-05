// This is a template.
// Just replace the body of "public BigInteger Factorial(int n)" with your function,  
// compile and start benchmarking your solution. If you found a nice solution
// please share it and send it to me for inclusion.

using System.Numerics;

namespace Sharith.Factorial
{
	public class MyFactorial : IFactorialFunction
	{
		public string Name => "MyFactorial        ";

		// --- Implement this function! ---
		public BigInteger Factorial(int n)
		{
			return new ParallelPrimeSwing().Factorial(n);
		}
	}
} // endOfMyFactorial
