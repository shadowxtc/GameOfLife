using System;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.Grids
{
	/// <summary>
	/// Description of ICellGenerator.
	/// </summary>
	public interface ICellGenerator<T>
	{
		Cell<T> Generate(Grid<T> grid, Coordinates2D coordinates);
	}
}
