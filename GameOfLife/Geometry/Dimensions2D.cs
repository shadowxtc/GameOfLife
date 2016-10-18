/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/15/2016
 * Time: 8:32 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace xtc.GameOfLife.Geometry
{
	/// <summary>
	/// Description of Dimensions2D.
	/// </summary>
	public class Dimensions2D
	{
		public int Width { get; set; }
		public int Height { get; set; }
		
		private Dimensions2D()
		{
		}
		
		public Dimensions2D(int width, int height) {
			Width = width;
			Height = height;
		}
	}
}
