using System;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.LangtonsAnt
{
	/// <summary>
	/// Description of ConsoleGridRenderer.
	/// </summary>
	public class ConsoleGridRenderer
		: IGridRenderer<LangtonsAntCellMetadata>
	{
		public event RenderMessagesEventHandler OnRenderMessages;
		public event RenderGridEventHandler<LangtonsAntCellMetadata> OnRenderGrid;
		public event RenderCellEventHandler<LangtonsAntCellMetadata> OnRenderCell;
		
		public ConsoleGridRenderer()
		{
		}
	
		public void RenderCell(Cell<LangtonsAntCellMetadata> cell) {
			var color = cell.Payload.IsWhite ? ConsoleColor.White : ConsoleColor.Black;
			var cellChar = '█';
			
			Console.SetCursorPosition(cell.Coordinates.X + 2, cell.Coordinates.Y + 3);

			if (cell.Payload.AntDirection.HasValue) {
				switch (cell.Payload.AntDirection.Value) {
					case Direction2D.Up:
						cellChar = '↑';
						break;
					case Direction2D.Right:
						cellChar = '→';
						break;
					case Direction2D.Down:
						cellChar = '↓';
						break;
					case Direction2D.Left:
						cellChar = '←';
						break;
					default:
						throw new InvalidOperationException("Invalid direction.");
				}
				
				Console.ForegroundColor = ConsoleColor.Red;
				Console.BackgroundColor = color;
			} else {
				Console.ForegroundColor = color;
			}
			
            Console.Write(cellChar);
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void RenderGrid(Grid<LangtonsAntCellMetadata> grid) {
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
					RenderCell(cell);//, cell.Payload.IsAlive ? LangtonsAntRule.KeepAlive : LangtonsAntRule.NoMatch);
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

		public Task PromptToContinueAsync(CancellationToken cancellationToken = default) =>
			Task.CompletedTask;

		public void EndSession()
		{
			throw new NotImplementedException();
		}
	}
}
