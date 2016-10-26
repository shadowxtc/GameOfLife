/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 12:37 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using xtc.GameOfLife.Grids;
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
		void EndSession();
	}
}
