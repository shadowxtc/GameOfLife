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
