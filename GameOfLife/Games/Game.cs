/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 1:34 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xtc.GameOfLife.Games
{
	/// <summary>
	/// Description of Game.
	/// </summary>
	public abstract class Game
	{
		private readonly object _lockObject = new object();
	    private readonly List<TimedAction> _timedActions;

	    private bool _gameEnding = false;

		public int MaxRounds { get; set; }
		public int CurrentRound { get; set; }
		public DateTime GameStarted { get; set; }
		public DateTime RoundStarted { get; set; }
		public TimeSpan? AutoIncrementRound { get; set; }
		
        public bool GameRunning { get; private set; }
		public bool GameOver { get; private set; }
		
		private Game() {
		}
		
		protected Game(int maxRounds)
		{
			GameOver = false;
			MaxRounds = maxRounds;
			GameStarted = DateTime.UtcNow;
			AutoIncrementRound = null;
            _timedActions = new List<TimedAction>();
		}

		protected abstract void ConfigureGame();
	    protected abstract void TeardownGame();

	    public virtual async void EndGame()
	    {
	    	lock (_lockObject)
	    	{
	    		if (_gameEnding)
	    			return;
	    		
	    		_gameEnding = true;
	    	}

	    	foreach (var timedAction in _timedActions)
	        {
	        	timedAction.Stop();

	        	while (!timedAction.Stopped)
	        		await Task.Delay(50);
	        }

	        TeardownGame();

        	GameOver = true;
	    }
		
	    public virtual void HandleKeypress(char command) {
	    	// NOTE: Intentionally blank.
	    }
	    
		public virtual void StartGame()
		{
		    lock (_lockObject)
		    {
                if (GameRunning)
                    return;

		        GameRunning = true;
		    }

            ConfigureGame();

		    if (AutoIncrementRound.HasValue)
		    {
		        CreateTimer(NextRound, AutoIncrementRound.Value, maxIterations: MaxRounds).Start();
		        return;
		    }

		    NextRound();
		}

		public virtual void NextRound() {
			if (CurrentRound >= MaxRounds)
			{
				EndGame();
				return;
			}
			
			if (++CurrentRound == 1)
				StartGame();

			RoundStarted = DateTime.UtcNow;
		}

	    public TimedAction CreateTimer(Action action, TimeSpan interval, TimeSpan? maxDuration = null, int? maxIterations = null)
	    {
	        var timedAction = new TimedAction(action, interval, maxDuration, maxIterations);
	        _timedActions.Add(timedAction);
	        return timedAction;
	    }
	}
}
