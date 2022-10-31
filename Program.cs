using System;

namespace Enot
{
	/// X - vertical
	/// Y - horizontal
	public class Position
	{
		public double X { get; set; }
		public double Y { get; set; }

		public Position(double x, double y) => (X, Y) = (x, y);

		public override string ToString() => $"X: {X} Y: {Y}";
	}

	public class Program
	{
		public static void Main()
		{
			Console.WriteLine(new Position(5.4, -4.2));

			Console.WriteLine();
		}
	}
}