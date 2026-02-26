using System;
using System.Linq;
using System.Collections.Generic;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.GameOfLife
{
    /// <summary>
    /// Description of GameOfLifeParsingCellGenerator.
    /// </summary>
    public class GameOfLifeParsingCellGenerator
		: ICellGenerator<GameOfLifeCellMetadata>
    {
        private readonly Dictionary<int, Dictionary<int, bool>> _map;
		
        public int MaxHeight { get; private set; }
        public int MaxWidth { get; private set; }
        
		private GameOfLifeParsingCellGenerator()
		{
		}
		
		public GameOfLifeParsingCellGenerator(string payload)
		{
			MaxHeight = 0;
			MaxWidth = 0;
			
			var lines = payload.Split('\r', '\n').Where(str => !string.IsNullOrWhiteSpace(str)).ToList();
			_map = new Dictionary<int, Dictionary<int, bool>>();
			
			foreach (var line in lines) {
				var width = 0;
				
				if (!_map.ContainsKey(MaxHeight))
					_map[MaxHeight] = new Dictionary<int, bool>();

				var characters = line.ToCharArray().ToList();
				
				foreach (var cell in characters) {
					if (cell != '.' && cell != '*')
						continue;
					
					_map[MaxHeight][width] = (cell == '*');
					width += 1;
				}
				
				MaxHeight += 1;
				MaxWidth = Math.Max(MaxWidth, width);
			}
		}

		public Cell<GameOfLifeCellMetadata> Generate(Grid<GameOfLifeCellMetadata> grid, Coordinates2D coordinates)
		{
		    var alive = _map[coordinates.Y][coordinates.X];

		    return new Cell<GameOfLifeCellMetadata>(grid, coordinates, new GameOfLifeCellMetadata(alive,
		        0,
		        alive ? GameOfLifeRule.KeepAlive : GameOfLifeRule.NoMatch));
		}
	}
}
