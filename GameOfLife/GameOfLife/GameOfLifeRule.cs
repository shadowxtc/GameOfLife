﻿/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 1:52 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace xtc.GameOfLife.GameOfLife
{
	/// <summary>
	/// Description of GameOfLifeRule.
	/// </summary>
	public enum GameOfLifeRule
	{
		NoMatch = 0,
		Overcrowded = 1,
		Underpopulated = 2,
		KeepAlive = 3,
		Respawn = 4
	}
}
