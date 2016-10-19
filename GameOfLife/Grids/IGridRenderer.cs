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
using xtc.GameOfLife.GameOfLife;

namespace xtc.GameOfLife.Grids
{
	/// <summary>
	/// Description of IGridRenderer.
	/// </summary>
	public interface IGridRenderer<T>
	{
		void RenderCell(Cell<T> cell);
		void RenderGrid(Grid<T> grid);
	}
}
