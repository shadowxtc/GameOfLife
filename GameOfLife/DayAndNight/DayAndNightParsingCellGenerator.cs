/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 2:08 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections.Generic;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.DayAndNight
{
    /// <summary>
    /// Description of DayAndNightParsingCellGenerator.
    /// </summary>
    public class DayAndNightParsingCellGenerator
		: ICellGenerator<DayAndNightCellMetadata>
    {
        private readonly Dictionary<int, Dictionary<int, bool>> _map;
		
        public int MaxHeight { get; private set; }
        public int MaxWidth { get; private set; }
        
		private DayAndNightParsingCellGenerator()
		{
		}
		
		public DayAndNightParsingCellGenerator(string payload)
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
					if (cell != '.' && cell != '*' && cell != 'O')
						continue;
					
					_map[MaxHeight][width] = (cell == '*' || cell == 'O');
					width += 1;
				}
				
				MaxHeight += 1;
				MaxWidth = Math.Max(MaxWidth, width);
			}
		}

		public Cell<DayAndNightCellMetadata> Generate(Grid<DayAndNightCellMetadata> grid, Coordinates2D coordinates)
		{
		    var alive = _map[coordinates.Y][coordinates.X];

		    return new Cell<DayAndNightCellMetadata>(grid, coordinates, new DayAndNightCellMetadata(alive,
		        0,
		        alive ? DayAndNightRule.KeepAlive : DayAndNightRule.NoMatch));
		}
	}
}
