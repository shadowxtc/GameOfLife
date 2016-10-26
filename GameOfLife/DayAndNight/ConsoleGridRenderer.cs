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

namespace xtc.GameOfLife.DayAndNight
{
	/// <summary>
	/// Description of ConsoleGridRenderer.
	/// </summary>
	public class ConsoleGridRenderer
		: IGridRenderer<DayAndNightCellMetadata>
	{
		public event RenderMessagesEventHandler OnRenderMessages;
		public event RenderGridEventHandler<DayAndNightCellMetadata> OnRenderGrid;
		public event RenderCellEventHandler<DayAndNightCellMetadata> OnRenderCell;
		
		public ConsoleGridRenderer()
		{
		}
		
		public void RenderCell(Cell<DayAndNightCellMetadata> cell) {
			var color = cell.Payload.IsAlive ? ConsoleColor.Yellow : ConsoleColor.DarkGray;
			
			switch (cell.Payload.Rule) {
				case DayAndNightRule.KeepAlive:
					color = ConsoleColor.Yellow;
					break;
				case DayAndNightRule.Respawn:
					color = ConsoleColor.DarkGreen;
					break;
				case DayAndNightRule.Overcrowded:
					color = ConsoleColor.Magenta;
					break;
				case DayAndNightRule.Underpopulated:
					color = ConsoleColor.Blue;
					break;
				case DayAndNightRule.NoMatch:
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

        public void RenderGrid(Grid<DayAndNightCellMetadata> grid) {
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
					RenderCell(cell);//, cell.Payload.IsAlive ? DayAndNightRule.KeepAlive : DayAndNightRule.NoMatch);
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

		public void StartSession()
		{
			throw new NotImplementedException();
		}

		public void RenderMessages(System.Collections.Generic.IEnumerable<xtc.GameOfLife.Games.GameMessage> messages)
		{
			throw new NotImplementedException();
		}

		public void PromptToContinue()
		{
			throw new NotImplementedException();
		}

		public void EndSession()
		{
			throw new NotImplementedException();
		}
	}
}
