using System;
using System.Collections.Generic;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Games;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.GameOfLife
{
	/// <summary>
	/// Description of ConsoleGridRenderer.
	/// </summary>
	public class ConsoleGridRenderer
		: IGridRenderer<GameOfLifeCellMetadata>
	{
		private Dimensions2D _dimensions;

		public event RenderGridEventHandler<GameOfLifeCellMetadata> OnRenderGrid;
		public event RenderCellEventHandler<GameOfLifeCellMetadata> OnRenderCell;
		public event RenderMessagesEventHandler OnRenderMessages;
		
		public ConsoleGridRenderer()
		{
			_dimensions = null;
		}
		
		public void StartSession() {
			Console.CursorVisible = false;
			Console.Clear();
		}
		
		public void EndSession() {
			Console.CursorVisible = true;
		}
		
		public void RenderCell(Cell<GameOfLifeCellMetadata> cell) {
			var color = cell.Payload.IsAlive ? ConsoleColor.Green : ConsoleColor.DarkGray;
			
			switch (cell.Payload.Rule) {
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
			_dimensions = grid.Dimensions;
			
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
					RenderCell(cell);//, cell.Payload.IsAlive ? GameOfLifeRule.KeepAlive : GameOfLifeRule.NoMatch);
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
		
		public void RenderMessages(IEnumerable<GameMessage> messages)
		{
			Console.SetCursorPosition(0, _dimensions == null ? 0 : _dimensions.Height + 5);

            foreach (var message in messages) {
				Console.ForegroundColor = message.IsWarning ? ConsoleColor.Yellow : ConsoleColor.Gray;
				Console.WriteLine(message.Message + "          ");
			}
		}

		public void PromptToContinue() {
			Console.WriteLine();
			Console.WriteLine("Press any key to begin...");
			Console.ReadKey(true);
		}

		public Task PromptToContinueAsync(CancellationToken cancellationToken = default) =>
			Task.Run(PromptToContinue, cancellationToken);
	}
}
