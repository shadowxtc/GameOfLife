using System;
using System.Linq;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.LangtonsAnt
{
	/// <summary>
	/// Description of LangtonsAntCopyCellGenerator.
	/// </summary>
	public class LangtonsAntCopyCellGenerator
		: ICellGenerator<LangtonsAntCellMetadata>
	{
		private readonly Grid<LangtonsAntCellMetadata> _grid;
		
		private LangtonsAntCopyCellGenerator()
		{
		}
		
		public LangtonsAntCopyCellGenerator(Grid<LangtonsAntCellMetadata> grid)
		{
			_grid = grid;
		}

		public Cell<LangtonsAntCellMetadata> Generate(Grid<LangtonsAntCellMetadata> grid, Coordinates2D coordinates) {
			var currentCell = _grid[coordinates];
			return new Cell<LangtonsAntCellMetadata>(grid, coordinates, new LangtonsAntCellMetadata(currentCell.Payload.IsWhite, currentCell.Payload.RoundsSurvived, currentCell.Payload.AntDirection));
		}
	}
}
