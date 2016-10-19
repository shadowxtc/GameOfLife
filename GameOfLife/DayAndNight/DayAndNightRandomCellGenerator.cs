/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 2:08 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.DayAndNight
{
	/// <summary>
	/// Description of DayAndNightRandomCellGenerator.
	/// </summary>
	public class DayAndNightRandomCellGenerator
		: ICellGenerator<DayAndNightCellMetadata>
	{
		private static readonly Random _randomizer = new Random();
		private readonly int _probability;
		
		public DayAndNightRandomCellGenerator()
			: this(2)
		{
		}
		
		public DayAndNightRandomCellGenerator(int probability)
		{
			_probability = probability;
		}

		public Cell<DayAndNightCellMetadata> Generate(Grid<DayAndNightCellMetadata> grid, Coordinates2D coordinates)
		{
		    var alive = _probability < 2
		        ? _probability == 1
		        : _randomizer.Next(0, _probability) == 1;

		    return new Cell<DayAndNightCellMetadata>(grid,
		        coordinates,
		        new DayAndNightCellMetadata(alive,
		            0,
		            alive ? DayAndNightRule.KeepAlive : DayAndNightRule.NoMatch));
		}
	}
}
