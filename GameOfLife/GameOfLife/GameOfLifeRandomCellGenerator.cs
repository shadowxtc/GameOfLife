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

namespace xtc.GameOfLife.GameOfLife
{
	/// <summary>
	/// Description of GameOfLifeRandomCellGenerator.
	/// </summary>
	public class GameOfLifeRandomCellGenerator
		: ICellGenerator<GameOfLifeCellMetadata>
	{
		private static readonly Random _randomizer = new Random();
		private readonly int _probability;
		
		public GameOfLifeRandomCellGenerator()
			: this(2)
		{
		}
		
		public GameOfLifeRandomCellGenerator(int probability)
		{
			_probability = probability;
		}

		public Cell<GameOfLifeCellMetadata> Generate(Grid<GameOfLifeCellMetadata> grid, Coordinates2D coordinates)
		{
		    var alive = _probability < 2
		        ? _probability == 1
		        : _randomizer.Next(0, _probability) == 1;

		    return new Cell<GameOfLifeCellMetadata>(grid,
		        coordinates,
		        new GameOfLifeCellMetadata(alive,
		            0,
		            alive ? GameOfLifeRule.KeepAlive : GameOfLifeRule.NoMatch));
		}
	}
}
