using System;

namespace Enot
{
	public class Position
	{
		public double X { get; set; }
		public double Y { get; set; }

		public Position(double x, double y) => (X, Y) = (x, y);

		public override string ToString() => $"X: {X} Y: {Y}";

		public static bool operator ==(Position a, Position b)
			=> a.X == b.X && a.Y == b.Y;

		public static bool operator !=(Position a, Position b)
			=> !(a == b);
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