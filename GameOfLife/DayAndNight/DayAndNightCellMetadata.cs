using System;

namespace xtc.GameOfLife.DayAndNight
{
	/// <summary>
	/// Description of DayAndNightCellMetadata.
	/// </summary>
	public class DayAndNightCellMetadata
	{
		public bool IsAlive { get; set; }
		public int RoundsSurvived { get; set; }
        public DayAndNightRule Rule { get; set; }

        private DayAndNightCellMetadata()
		{
		}
		
		public DayAndNightCellMetadata(bool isAlive, int roundsSurvived, DayAndNightRule rule)
		{
			IsAlive = isAlive;
		    RoundsSurvived = roundsSurvived;
		    Rule = rule;
		}
	}
}
