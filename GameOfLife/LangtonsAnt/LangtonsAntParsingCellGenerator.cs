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

namespace xtc.GameOfLife.LangtonsAnt
{
    /// <summary>
    /// Description of LangtonsAntParsingCellGenerator.
    /// </summary>
    public class LangtonsAntParsingCellGenerator
		: ICellGenerator<LangtonsAntCellMetadata>
    {
        private readonly Dictionary<int, Dictionary<int, bool>> _map;

        public Coordinates2D AntLocation { get; private set; }
        public Direction2D? AntDirection { get; private set; }
        public int MaxHeight { get; private set; }
        public int MaxWidth { get; private set; }
        
		private LangtonsAntParsingCellGenerator()
		{
		}
		
		public LangtonsAntParsingCellGenerator(string payload)
		{
			AntLocation = null;
			AntDirection = null;
			
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
					if (cell == '#')
						break;
					
					var isWhite = false;
					switch (cell) {
						case 'w':
						case 'W':
							isWhite = true;
							break;
						case 'b':
						case 'B':
							isWhite = false;
							break;
						case 'U':
							isWhite = true;
							AntDirection = Direction2D.Up;
							AntLocation = new Coordinates2D(width, MaxHeight);
							break;
						case 'u':
							isWhite = false;
							AntDirection = Direction2D.Up;
							AntLocation = new Coordinates2D(width, MaxHeight);
							break;
						case 'R':
							isWhite = true;
							AntDirection = Direction2D.Right;
							AntLocation = new Coordinates2D(width, MaxHeight);
							break;
						case 'r':
							isWhite = false;
							AntDirection = Direction2D.Right;
							AntLocation = new Coordinates2D(width, MaxHeight);
							break;
						case 'D':
							isWhite = true;
							AntDirection = Direction2D.Down;
							AntLocation = new Coordinates2D(width, MaxHeight);
							break;
						case 'd':
							isWhite = false;
							AntDirection = Direction2D.Down;
							AntLocation = new Coordinates2D(width, MaxHeight);
							break;
						case 'L':
							isWhite = true;
							AntDirection = Direction2D.Left;
							AntLocation = new Coordinates2D(width, MaxHeight);
							break;
						case 'l':
							isWhite = false;
							AntDirection = Direction2D.Left;
							AntLocation = new Coordinates2D(width, MaxHeight);
							break;
						default:
							continue;
					}
					
					_map[MaxHeight][width] = isWhite;
					width += 1;
				}
				
				if (width > 0)
					MaxHeight += 1;

				MaxWidth = Math.Max(MaxWidth, width);
			}
		}

		public Cell<LangtonsAntCellMetadata> Generate(Grid<LangtonsAntCellMetadata> grid, Coordinates2D coordinates)
		{
		    var alive = _map[coordinates.Y][coordinates.X];

		    return new Cell<LangtonsAntCellMetadata>(grid, coordinates, new LangtonsAntCellMetadata(alive,
		        0,
		        coordinates == AntLocation ? AntDirection : null));
		}
	}
}
