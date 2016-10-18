/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 12:37 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using xtc.GameOfLife.Grids;

namespace xtc.GameOfLife.GameOfLife
{
	/// <summary>
	/// Description of ConsoleGridRenderer.
	/// </summary>
	public class ConsoleGridRenderer
		: IGridRenderer<GameOfLifeCellMetadata>
	{
		public ConsoleGridRenderer()
		{
		}
		
		public void RenderCell(Cell<GameOfLifeCellMetadata> cell, GameOfLifeRule matchingRule) {
			var color = cell.Payload.IsAlive ? ConsoleColor.Green : ConsoleColor.DarkGray;
			
			switch (matchingRule) {
				case GameOfLifeRule.KeepAlive:
					color = ConsoleColor.DarkGreen;
					break;
				case GameOfLifeRule.Respawn:
					color = ConsoleColor.Green;
					break;
				case GameOfLifeRule.Overcrowded:
					color = ConsoleColor.Magenta;
					break;
				case GameOfLifeRule.Underpopulated:
					color = ConsoleColor.Yellow;
					break;
				case GameOfLifeRule.NoMatch:
					color = ConsoleColor.DarkGray;
					break;
				default:
					throw new InvalidOperationException("Unknown rule.");
			}

			Console.SetCursorPosition(cell.Coordinates.X + 2, cell.Coordinates.Y + 3);
			Console.ForegroundColor = color;
            //Console.Write(cell.Payload.IsAlive ? "■" : "·");
            Console.Write(cell.Payload.IsAlive ? "█" : "·");
        }

        public void RenderGrid(Grid<GameOfLifeCellMetadata> grid) {
			Console.SetCursorPosition(0, 0);
			
			Console.ResetColor();
			Console.WriteLine();
			
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("╔═");
			for (var i = 0; i < grid.Dimensions.Width; ++i)
				Console.Write("═");
			Console.WriteLine("═╗");
			
			Console.Write("║ ");
			for (var i = 0; i < grid.Dimensions.Width; ++i)
				Console.Write(" ");
			Console.WriteLine(" ║");
			
			for (var y = 0; y < grid.Dimensions.Height; ++y)
			{
				var row = grid.GetRow(y);
				
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.Write("║ ");

				var x = 0;
				foreach (var cell in row)
				{
					RenderCell(cell, cell.Payload.IsAlive ? GameOfLifeRule.KeepAlive : GameOfLifeRule.NoMatch);
					//Console.ForegroundColor = cell.Payload.IsAlive ? ConsoleColor.Green : ConsoleColor.DarkGray;
					//Console.Write(cell.Payload.IsAlive ? "■" : " ");
					++x;
				}
				
				Console.ForegroundColor = ConsoleColor.Cyan;
				Console.WriteLine(" ║");
			}

			Console.ForegroundColor = ConsoleColor.Cyan;

			Console.Write("║ ");
			for (var i = 0; i < grid.Dimensions.Width; ++i)
				Console.Write(" ");
			Console.WriteLine(" ║");

			Console.Write("╚═");
			for (var i = 0; i < grid.Dimensions.Width; ++i)
				Console.Write("═");
			Console.WriteLine("═╝");
			
			// TODO: output counters/timers?  that really should be done in game class though
		}
	}
}
