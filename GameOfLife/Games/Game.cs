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
		private bool _paused;

		public bool IsPaused => _paused;

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
		protected virtual Task ConfigureGameAsync() { ConfigureGame(); return Task.CompletedTask; }
	    protected abstract void TeardownGame();

	    public virtual async Task EndGameAsync()
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

	    public void EndGame() => _ = EndGameAsync();
		
	    public virtual void HandleKeypress(char command) {
	    	// NOTE: Intentionally blank.
	    }
	    
		public virtual void StartGame()
		{
		    StartGameAsync().GetAwaiter().GetResult();
		}

		public virtual async Task StartGameAsync()
		{
		    lock (_lockObject)
		    {
                if (GameRunning)
                    return;

		        GameRunning = true;
		    }

            await ConfigureGameAsync();

		    if (AutoIncrementRound.HasValue)
		    {
		        CreateTimer(NextRound, AutoIncrementRound.Value, maxIterations: MaxRounds).Start();
		        return;
		    }

		    NextRound();
		}

		public virtual void NextRound() {
			if (_paused)
				return;
			if (CurrentRound >= MaxRounds)
			{
				EndGame();
				return;
			}
			
			if (++CurrentRound == 1)
				StartGame();

			RoundStarted = DateTime.UtcNow;
		}

		public void Pause() { _paused = true; }
		public void Resume() { _paused = false; }

	    public TimedAction CreateTimer(Action action, TimeSpan interval, TimeSpan? maxDuration = null, int? maxIterations = null)
	    {
	        var timedAction = new TimedAction(action, interval, maxDuration, maxIterations);
	        _timedActions.Add(timedAction);
	        return timedAction;
	    }
	}
}
