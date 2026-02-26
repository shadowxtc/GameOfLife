using System;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.LangtonsAnt
{
	/// <summary>
	/// Description of LangtonsAntRandomCellGenerator.
	/// </summary>
	public class LangtonsAntRandomCellGenerator
		: ICellGenerator<LangtonsAntCellMetadata>
	{
		private static readonly Random _randomizer = new Random();
		private readonly int _probability;
		
		public Coordinates2D AntCoordinates { get; private set; }
		public Direction2D? AntDirection { get; private set; }
		
		public LangtonsAntRandomCellGenerator()
			: this(2, new Coordinates2D(0, 0), Direction2D.Right)
		{
		}
		
		public LangtonsAntRandomCellGenerator(int probability, Coordinates2D antCoordinates, Direction2D? antDirection)
		{
			_probability = probability;
			AntCoordinates = antCoordinates;
			AntDirection = antDirection;
		}

		public Cell<LangtonsAntCellMetadata> Generate(Grid<LangtonsAntCellMetadata> grid, Coordinates2D coordinates)
		{
			if (AntCoordinates == null) 
			{
				AntCoordinates = new Coordinates2D(_randomizer.Next(0, grid.Dimensions.Width), _randomizer.Next(0, grid.Dimensions.Height));
				AntDirection = (Direction2D) _randomizer.Next(0, 4);
			}

			var alive = _probability < 2
		        ? _probability == 1
		        : _randomizer.Next(0, _probability) == 1;

		    return new Cell<LangtonsAntCellMetadata>(grid,
		        coordinates,
		        new LangtonsAntCellMetadata(alive,
		            0,
		            coordinates == AntCoordinates ? AntDirection : null));
		}
	}
}
