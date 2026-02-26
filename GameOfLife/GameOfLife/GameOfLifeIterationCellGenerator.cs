using System;
using System.Linq;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.GameOfLife
{
	/// <summary>
	/// Description of GameOfLifeIterationCellGenerator.
	/// </summary>
	public class GameOfLifeIterationCellGenerator
		: ICellGenerator<GameOfLifeCellMetadata>
	{
		private readonly GameOfLife _game;
		private readonly Grid<GameOfLifeCellMetadata> _grid;
		private readonly IGridRenderer<GameOfLifeCellMetadata> _gridRenderer;
		
		private GameOfLifeIterationCellGenerator()
		{
		}
		
		public GameOfLifeIterationCellGenerator(GameOfLife game, Grid<GameOfLifeCellMetadata> grid, IGridRenderer<GameOfLifeCellMetadata> gridRenderer)
		{
			_game = game;
			_grid = grid;
			_gridRenderer = gridRenderer;
		}

		public Cell<GameOfLifeCellMetadata> Generate(Grid<GameOfLifeCellMetadata> grid, Coordinates2D coordinates) {
			var currentCell = _grid[coordinates];
			var livingNeighbors = currentCell.Neighbors.Count(neighbor => neighbor.Payload.IsAlive);
			var matchingRule = GameOfLifeRule.NoMatch;
			var payload = currentCell.Payload;
			
			if (livingNeighbors < 2)
			{
				matchingRule = GameOfLifeRule.Underpopulated;
			}
			else if (livingNeighbors > 3)
			{
				matchingRule = GameOfLifeRule.Overcrowded;
			}
			else if (livingNeighbors >= 2 && livingNeighbors <= 3 && payload.IsAlive)
			{
				matchingRule = GameOfLifeRule.KeepAlive;
			}
			else if (livingNeighbors == 3 && !payload.IsAlive)
			{
				matchingRule = GameOfLifeRule.Respawn;
			}

		    var newPayloadAlive = payload.IsAlive;

			// TODO: increase game counters: totalgenerated, totalkilled[reason], totalborn[reason], totalcputime
			// TODO: increase game round counters: generated, killed[reason], born[reason], cputime

			var cell = grid[coordinates];
			if (cell == null) 
				cell = new Cell<GameOfLifeCellMetadata>(grid, coordinates, new GameOfLifeCellMetadata(newPayloadAlive, payload.RoundsSurvived, matchingRule));

            switch (matchingRule)
            {
                case GameOfLifeRule.KeepAlive:
                    cell.Payload.RoundsSurvived += 1;
                    break;
                case GameOfLifeRule.NoMatch:
                    break;
                case GameOfLifeRule.Overcrowded:
                case GameOfLifeRule.Underpopulated:
                    cell.Payload.RoundsSurvived = 0;
                    newPayloadAlive = false;
                    break;
                case GameOfLifeRule.Respawn:
                    cell.Payload.RoundsSurvived = 0;
                    newPayloadAlive = true;
                    break;
                default:
                    throw new InvalidOperationException("Invalid game rule state!");
            }

            cell.Payload.IsAlive = newPayloadAlive;
		    cell.Payload.Rule = matchingRule;

			if (newPayloadAlive != payload.IsAlive || (matchingRule == GameOfLifeRule.KeepAlive && cell.Payload.RoundsSurvived == 2))
				_gridRenderer.RenderCell(cell);//, matchingRule);

			return cell;
		}
	}
}
