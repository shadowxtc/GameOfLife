using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xtc.GameOfLife.Games;
using xtc.GameOfLife.Grids;

namespace xtc.GameOfLife.GameOfLife
{
	/// <summary>
	/// Description of GDIPlusGridRenderer.
	/// </summary>
	public class GDIPlusGridRenderer
		: IGridRenderer<GameOfLifeCellMetadata>
	{
		private readonly Form _form;
		
		public event RenderMessagesEventHandler OnRenderMessages;
		public event RenderGridEventHandler<GameOfLifeCellMetadata> OnRenderGrid;
		public event RenderCellEventHandler<GameOfLifeCellMetadata> OnRenderCell;

		public GDIPlusGridRenderer(Form form)
		{
			_form = form;
		}
		
		public void RenderCell(Cell<GameOfLifeCellMetadata> cell) {
			OnRenderCell(cell);
        }

        public void RenderGrid(Grid<GameOfLifeCellMetadata> grid) {
			OnRenderGrid(grid);

			for (var y = 0; y < grid.Dimensions.Height; ++y)
			{
				var row = grid.GetRow(y);
				
				foreach (var cell in row)
					RenderCell(cell);
			}
		}

		public void StartSession()
		{
			// no-op
		}

		public void RenderMessages(System.Collections.Generic.IEnumerable<xtc.GameOfLife.Games.GameMessage> messages)
		{
			OnRenderMessages(messages);
		}

		public void PromptToContinue()
		{
			PromptToContinueAsync().GetAwaiter().GetResult();
		}

		public async Task PromptToContinueAsync(CancellationToken cancellationToken = default)
		{
			RenderMessages(new List<GameMessage> { new GameMessage("Get Ready!!!", true) });
			await Task.Delay(500, cancellationToken);

			RenderMessages(new List<GameMessage> { new GameMessage("Get Ready!", true), new GameMessage(""), new GameMessage("Starting in 3...") });
			await Task.Delay(1000, cancellationToken);

			RenderMessages(new List<GameMessage> { new GameMessage("Get Ready!", true), new GameMessage(""), new GameMessage("Starting in 2...") });
			await Task.Delay(1000, cancellationToken);

			RenderMessages(new List<GameMessage> { new GameMessage("Get Ready!", true), new GameMessage(""), new GameMessage("Starting in 1...") });
			await Task.Delay(1000, cancellationToken);

			RenderMessages(new List<GameMessage> { new GameMessage("GO!!!", true) });
			await Task.Delay(100, cancellationToken);
		}

		public void EndSession()
		{
			// no-op
		}
	}
}
