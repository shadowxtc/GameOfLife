using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using xtc.GameOfLife.Games;

namespace xtc.GameOfLife.Grids
{
	/// <summary>
	/// Description of IGridRenderer.
	/// </summary>
	public interface IGridRenderer<T>
	{
		event RenderMessagesEventHandler OnRenderMessages;
		event RenderGridEventHandler<T> OnRenderGrid;
		event RenderCellEventHandler<T> OnRenderCell;
		
		void StartSession();
		void RenderCell(Cell<T> cell);
		void RenderGrid(Grid<T> grid);
		void RenderMessages(IEnumerable<GameMessage> messages);
		void PromptToContinue();
		Task PromptToContinueAsync(CancellationToken cancellationToken = default);
		void EndSession();
	}
}
