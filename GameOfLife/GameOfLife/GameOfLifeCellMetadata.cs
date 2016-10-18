/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 1:47 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
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
