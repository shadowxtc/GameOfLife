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
