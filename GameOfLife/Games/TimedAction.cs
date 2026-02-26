using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace xtc.GameOfLife.Games
{
    public class TimedAction
    {
    	private readonly object _lockObject = new object();
    	
    	public int CurrentIteration { get; private set; }
    	public DateTime StartTime { get; private set; }
    	public DateTime IterationStartTime { get; private set; }
    	public Task? TimerTask { get; private set; }
    	public CancellationTokenSource CancellationTokenSource { get; private set; }
    	
    	public int? MaxIterations { get; set; }
    	public TimeSpan Interval { get; set; }
    	public TimeSpan? MaxDuration { get; set; }
    	public Action Action { get; set; }
    	
    	public bool Started { get; private set; }
    	public bool Stopped { get; private set; }
    	
    	public bool Running {
    		get {
    			return TimerTask != null && TimerTask.Status == TaskStatus.Running;
    		}
    	}
    	
    	private TimedAction() {
    	}
    	
    	public TimedAction(Action action, TimeSpan interval, TimeSpan? maxDuration = null, int? maxIterations = null) {
    		Action = action;
    		Interval = interval;
    		MaxDuration = maxDuration;
    		MaxIterations = maxIterations;
    		
    		CancellationTokenSource = MaxDuration.HasValue ?
    			new CancellationTokenSource(MaxDuration.Value) :
    			new CancellationTokenSource();
    	}

    	public void Start() {
    		lock (_lockObject) {
    			if (Started)
    				return;
    			
    			Started = true;
    			StartTime = DateTime.UtcNow;

    			TimerTask = IterateAsync();
    		}
    	}
    	
    	public void Stop() {
    		CancellationTokenSource.Cancel();
    	}
    	
    	private async Task IterateAsync() {
    		while(!CancellationTokenSource.IsCancellationRequested) {
	    		if (MaxIterations.HasValue && CurrentIteration >= MaxIterations.Value)
	    			break;

	    		if (MaxDuration.HasValue && DateTime.UtcNow.Subtract(StartTime) >= MaxDuration.Value)
	    			break;
	    		
	    		await Task.Delay(Interval);
	    		
	    		CurrentIteration += 1;
	    		IterationStartTime = DateTime.UtcNow;
	    		
	    		Action();
    		}
    		
			Stopped = true;
    	}
    }
}
