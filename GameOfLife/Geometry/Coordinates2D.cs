namespace xtc.GameOfLife.Geometry;

/// <summary>
/// Description of Coordinates2D.
/// </summary>
public class Coordinates2D
{
	public int X { get; set; }
	public int Y { get; set; }

	private Coordinates2D()
	{
	}

	public Coordinates2D(int x, int y) {
		X = x;
		Y = y;
	}

	public override string ToString()
	{
		return string.Format("[Coordinates2D X={0}, Y={1}]", X, Y);
	}

	public override bool Equals(object? obj)
	{
		Coordinates2D? other = obj as Coordinates2D;
		return other is not null && X == other.X && Y == other.Y;
	}

	public override int GetHashCode()
	{
		int hashCode = 0;
		unchecked {
			hashCode += 1000000007 * X.GetHashCode();
			hashCode += 1000000009 * Y.GetHashCode();
		}
		return hashCode;
	}

	public static bool operator ==(Coordinates2D? lhs, Coordinates2D? rhs) {
		if (ReferenceEquals(lhs, rhs))
			return true;
		if (lhs is null || rhs is null)
			return false;
		return lhs.Equals(rhs);
	}

	public static bool operator !=(Coordinates2D? lhs, Coordinates2D? rhs) {
		return !(lhs == rhs);
	}
}
