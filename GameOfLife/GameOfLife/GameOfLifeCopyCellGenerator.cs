using System;
using System.Linq;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.GameOfLife
{
	/// <summary>
	/// Description of GameOfLifeCopyCellGenerator.
	/// </summary>
	public class GameOfLifeCopyCellGenerator
		: ICellGenerator<GameOfLifeCellMetadata>
	{
		private readonly Grid<GameOfLifeCellMetadata> _grid;
		
		private GameOfLifeCopyCellGenerator()
		{
		}
		
		public GameOfLifeCopyCellGenerator(Grid<GameOfLifeCellMetadata> grid)
		{
			_grid = grid;
		}

		public Cell<GameOfLifeCellMetadata> Generate(Grid<GameOfLifeCellMetadata> grid, Coordinates2D coordinates) {
			var currentCell = _grid[coordinates];
			return new Cell<GameOfLifeCellMetadata>(grid, coordinates, new GameOfLifeCellMetadata(currentCell.Payload.IsAlive, currentCell.Payload.RoundsSurvived, currentCell.Payload.Rule));
		}
	}
}
