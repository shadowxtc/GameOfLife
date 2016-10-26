/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/23/2016
 * Time: 11:33 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
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
			RenderMessages(new List<GameMessage> { new GameMessage("Get Ready!!!", true) });
			Thread.Sleep(500);

			RenderMessages(new List<GameMessage> { new GameMessage("Get Ready!", true), new GameMessage(""), new GameMessage("Starting in 3...") });
			Thread.Sleep(1000);

			RenderMessages(new List<GameMessage> { new GameMessage("Get Ready!", true), new GameMessage(""), new GameMessage("Starting in 2...") });
			Thread.Sleep(1000);

			RenderMessages(new List<GameMessage> { new GameMessage("Get Ready!", true), new GameMessage(""), new GameMessage("Starting in 1...") });
			Thread.Sleep(1000);

			RenderMessages(new List<GameMessage> { new GameMessage("GO!!!", true) });
			Thread.Sleep(100);
		}

		public void EndSession()
		{
			// no-op
		}
	}
}
