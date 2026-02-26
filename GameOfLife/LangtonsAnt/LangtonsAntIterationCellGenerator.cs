using System;
using System.Linq;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.LangtonsAnt
{
	/// <summary>
	/// Description of LangtonsAntIterationCellGenerator.
	/// </summary>
	public class LangtonsAntIterationCellGenerator
		: ICellGenerator<LangtonsAntCellMetadata>
	{
		private readonly LangtonsAnt _game;
		private readonly Grid<LangtonsAntCellMetadata> _grid;
		private readonly IGridRenderer<LangtonsAntCellMetadata> _gridRenderer;
		
		public Coordinates2D AntCoordinates { get; set; }
		public Direction2D AntDirection { get; set; }
		
		private LangtonsAntIterationCellGenerator()
		{
		}
		
		public LangtonsAntIterationCellGenerator(LangtonsAnt game, Grid<LangtonsAntCellMetadata> grid, IGridRenderer<LangtonsAntCellMetadata> gridRenderer, Coordinates2D antCoordinates, Direction2D antDirection)
		{
			_game = game;
			_grid = grid;
			_gridRenderer = gridRenderer;
			AntCoordinates = antCoordinates;
			AntDirection = antDirection;
		}

		public void Increment() {
			var currentAntCell = _grid[AntCoordinates];
			Cell<LangtonsAntCellMetadata> newAntCell;
			
			if (currentAntCell.Payload.IsWhite) {
				currentAntCell.Payload.IsWhite = false;
				AntDirection = RotateClockwise(AntDirection);
			} else {
				currentAntCell.Payload.IsWhite = true;
				AntDirection = RotateCounterClockwise(AntDirection);
			}
			
			newAntCell = ProgressCell(currentAntCell, AntDirection);
			
			if (newAntCell == null)
			{
				AntDirection = Invert(AntDirection);
				newAntCell = ProgressCell(currentAntCell, AntDirection);
			}
			
			AntCoordinates = newAntCell.Coordinates;
			
			currentAntCell.Payload.AntDirection = null;
			newAntCell.Payload.AntDirection = AntDirection;
			
			_gridRenderer.RenderCell(currentAntCell);
			_gridRenderer.RenderCell(newAntCell);
		}
		
		private Cell<LangtonsAntCellMetadata> ProgressCell(Cell<LangtonsAntCellMetadata> currentCell, Direction2D direction) {
			var newCoordinates = new Coordinates2D(currentCell.Coordinates.X, currentCell.Coordinates.Y);
			
			switch(direction) {
				case Direction2D.Up:
					newCoordinates.Y -= 1;
					break;
				case Direction2D.Right:
					newCoordinates.X += 1;
					break;
				case Direction2D.Down:
					newCoordinates.Y += 1;
					break;
				case Direction2D.Left:
					newCoordinates.X -= 1;
					break;
				default:
					throw new InvalidOperationException("Invalid direction.");
			}
			
			if (newCoordinates.X < 0 || newCoordinates.X >= _grid.Dimensions.Width ||
			    newCoordinates.Y < 0 || newCoordinates.Y >= _grid.Dimensions.Height)
				return null;
			
			return _grid[newCoordinates];
		}
		
		private Direction2D Invert(Direction2D direction) {
			switch(direction) {
				case Direction2D.Up:
					return Direction2D.Down;
				case Direction2D.Down:
					return Direction2D.Up;
				case Direction2D.Left:
					return Direction2D.Right;
				case Direction2D.Right:
					return Direction2D.Left;
				default:
					throw new InvalidOperationException("Invalid direction.");
			}
		}
		
		private Direction2D RotateClockwise(Direction2D direction) {
			switch(direction) {
				case Direction2D.Up:
					return Direction2D.Right;
				case Direction2D.Down:
					return Direction2D.Left;
				case Direction2D.Left:
					return Direction2D.Up;
				case Direction2D.Right:
					return Direction2D.Down;
				default:
					throw new InvalidOperationException("Invalid direction.");
			}
		}
		
		private Direction2D RotateCounterClockwise(Direction2D direction) {
			switch(direction) {
				case Direction2D.Up:
					return Direction2D.Left;
				case Direction2D.Down:
					return Direction2D.Right;
				case Direction2D.Left:
					return Direction2D.Down;
				case Direction2D.Right:
					return Direction2D.Up;
				default:
					throw new InvalidOperationException("Invalid direction.");
			}
		}

		public Cell<LangtonsAntCellMetadata> Generate(Grid<LangtonsAntCellMetadata> grid, Coordinates2D coordinates) {
			return _grid[coordinates];
		}
	}
}
