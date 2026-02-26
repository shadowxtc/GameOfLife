using System;

namespace xtc.GameOfLife.GameOfLife
{
	/// <summary>
	/// Description of GameOfLifeCellMetadata.
	/// </summary>
	public class GameOfLifeCellMetadata
	{
		public bool IsAlive { get; set; }
		public int RoundsSurvived { get; set; }
        public GameOfLifeRule Rule { get; set; }

        private GameOfLifeCellMetadata()
		{
		}
		
		public GameOfLifeCellMetadata(bool isAlive, int roundsSurvived, GameOfLifeRule rule)
		{
			IsAlive = isAlive;
		    RoundsSurvived = roundsSurvived;
		    Rule = rule;
		}
	}
}
