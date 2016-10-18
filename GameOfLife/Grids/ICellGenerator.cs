/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 1:37 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.Grids
{
	/// <summary>
	/// Description of ICellGenerator.
	/// </summary>
	public interface ICellGenerator<T>
	{
		Cell<T> Generate(Grid<T> grid, Coordinates2D coordinates);
	}
}
