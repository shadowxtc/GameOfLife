/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 1:47 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
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
