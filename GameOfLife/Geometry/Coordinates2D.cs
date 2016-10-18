﻿/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/15/2016
 * Time: 8:35 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace xtc.GameOfLife.Geometry
{
	/// <summary>
	/// Description of Coordinates2D.
	/// </summary>
	public class Coordinates2D
	{
		public int X { get; set; }
		public int Y { get; set; }
		
		private Coordinates2D()
		{
		}
		
		public Coordinates2D(int x, int y) {
			X = x;
			Y = y;
		}
	}
}
