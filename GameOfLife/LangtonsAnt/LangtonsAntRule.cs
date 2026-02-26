using System;

namespace xtc.GameOfLife.LangtonsAnt
{
	/// <summary>
	/// Description of LangtonsAntRule.
	/// </summary>
	public enum LangtonsAntRule
	{
		NoMatch = 0,
		Overcrowded = 1,
		Underpopulated = 2,
		KeepAlive = 3,
		Respawn = 4
	}
}
