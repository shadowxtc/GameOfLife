using System;
using System.Linq;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.DayAndNight
{
	/// <summary>
	/// Description of DayAndNightIterationCellGenerator.
	/// </summary>
	public class DayAndNightIterationCellGenerator
		: ICellGenerator<DayAndNightCellMetadata>
	{
		private readonly DayAndNight _game;
		private readonly Grid<DayAndNightCellMetadata> _grid;
		private readonly IGridRenderer<DayAndNightCellMetadata> _gridRenderer;
		
		private DayAndNightIterationCellGenerator()
		{
		}
		
		public DayAndNightIterationCellGenerator(DayAndNight game, Grid<DayAndNightCellMetadata> grid, IGridRenderer<DayAndNightCellMetadata> gridRenderer)
		{
			_game = game;
			_grid = grid;
			_gridRenderer = gridRenderer;
		}

		public Cell<DayAndNightCellMetadata> Generate(Grid<DayAndNightCellMetadata> grid, Coordinates2D coordinates) {
			var currentCell = _grid[coordinates];
			var livingNeighbors = currentCell.Neighbors.Count(neighbor => neighbor.Payload.IsAlive);
			var matchingRule = DayAndNightRule.NoMatch;
			var payload = currentCell.Payload;

			if (payload.IsAlive) 
			{
				if (livingNeighbors < 3) 
				{
					matchingRule = DayAndNightRule.Underpopulated;
				}
				else if (livingNeighbors == 5)
				{
					matchingRule = DayAndNightRule.Overcrowded;
				}
				else
				{
					matchingRule = DayAndNightRule.KeepAlive;
				}
			}
			else if (livingNeighbors == 3 || livingNeighbors > 5)
			{
				matchingRule = DayAndNightRule.Respawn;
			}

		    var newPayloadAlive = payload.IsAlive;

			// TODO: increase game counters: totalgenerated, totalkilled[reason], totalborn[reason], totalcputime
			// TODO: increase game round counters: generated, killed[reason], born[reason], cputime

			var cell = grid[coordinates];
			if (cell == null) 
				cell = new Cell<DayAndNightCellMetadata>(grid, coordinates, new DayAndNightCellMetadata(newPayloadAlive, payload.RoundsSurvived, matchingRule));

            switch (matchingRule)
            {
                case DayAndNightRule.KeepAlive:
                    cell.Payload.RoundsSurvived += 1;
                    break;
                case DayAndNightRule.NoMatch:
                    break;
                case DayAndNightRule.Overcrowded:
                case DayAndNightRule.Underpopulated:
                    cell.Payload.RoundsSurvived = 0;
                    newPayloadAlive = false;
                    break;
                case DayAndNightRule.Respawn:
                    cell.Payload.RoundsSurvived = 0;
                    newPayloadAlive = true;
                    break;
                default:
                    throw new InvalidOperationException("Invalid game rule state!");
            }

            cell.Payload.IsAlive = newPayloadAlive;
		    cell.Payload.Rule = matchingRule;

			if (newPayloadAlive != payload.IsAlive || (matchingRule == DayAndNightRule.KeepAlive && cell.Payload.RoundsSurvived == 2))
				_gridRenderer.RenderCell(cell);//, matchingRule);

			return cell;
		}
	}
}
