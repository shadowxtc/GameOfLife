using System;

namespace xtc.GameOfLife.Games
{
	/// <summary>
	/// Description of GameMessage.
	/// </summary>
	public class GameMessage
	{
		public string Message { get; set; }
		public bool IsWarning { get; set; }
		
		private GameMessage()
		{
		}
		
		public GameMessage(string message)
			: this(message, false)
		{
		}
		
		public GameMessage(string message, bool isWarning) {
			Message = message;
			IsWarning = isWarning;
		}
	}
}
