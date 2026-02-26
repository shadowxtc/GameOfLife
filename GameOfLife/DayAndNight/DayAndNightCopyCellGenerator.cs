using System;
using System.Linq;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.DayAndNight
{
	/// <summary>
	/// Description of DayAndNightCopyCellGenerator.
	/// </summary>
	public class DayAndNightCopyCellGenerator
		: ICellGenerator<DayAndNightCellMetadata>
	{
		private readonly Grid<DayAndNightCellMetadata> _grid;
		
		private DayAndNightCopyCellGenerator()
		{
		}
		
		public DayAndNightCopyCellGenerator(Grid<DayAndNightCellMetadata> grid)
		{
			_grid = grid;
		}

		public Cell<DayAndNightCellMetadata> Generate(Grid<DayAndNightCellMetadata> grid, Coordinates2D coordinates) {
			var currentCell = _grid[coordinates];
			return new Cell<DayAndNightCellMetadata>(grid, coordinates, new DayAndNightCellMetadata(currentCell.Payload.IsAlive, currentCell.Payload.RoundsSurvived, currentCell.Payload.Rule));
		}
	}
}
