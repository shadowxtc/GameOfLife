/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 12:32 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.Grids
{
	/// <summary>
	/// Description of Cell.
	/// </summary>
	public class Cell<T>
	{
		public Grid<T> Grid { get; set; }
		public Coordinates2D Coordinates { get; set; }
		public T Payload { get; set; }
			
		public IEnumerable<Cell<T>> Neighbors {
			get {
				var neighbors = new List<Cell<T>>();
				
				for (var x = Coordinates.X + (int) DirectionX2D.Left;
				     x <= Coordinates.X + (int) DirectionX2D.Right;
	                 ++x) 
					for (var y = Coordinates.Y + (int) DirectionY2D.Down;
					     y <= Coordinates.Y + (int) DirectionY2D.Up;
					     ++y)
				{
					if (!Grid.VerifyCoordinates(x, y))
					    continue;
				
					var element = Grid[new Coordinates2D(x, y)];
	
					if (element != this)
						neighbors.Add(element);
				}
				
				return neighbors;
			}
		}

		private Cell()
		{
		}
		
		public Cell(Grid<T> grid, Coordinates2D coordinates, T payload) {
			Grid = grid;
			Coordinates = coordinates;
			Payload = payload;
		}
	}
}
