/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 1:37 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
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
