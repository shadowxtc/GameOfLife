using System;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.LangtonsAnt
{
	/// <summary>
	/// Description of LangtonsAntCellMetadata.
	/// </summary>
	public class LangtonsAntCellMetadata
	{
		public bool IsWhite { get; set; }
		public int RoundsSurvived { get; set; }
        public Direction2D? AntDirection { get; set; }

        private LangtonsAntCellMetadata()
		{
		}
		
		public LangtonsAntCellMetadata(bool isWhite, int roundsSurvived, Direction2D? antDirection)
		{
			IsWhite = isWhite;
		    RoundsSurvived = roundsSurvived;
		    AntDirection = antDirection;
		}
	}
}
