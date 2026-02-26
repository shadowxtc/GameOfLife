using System;
using System.IO;
using System.Threading;
using xtc.GameOfLife.Games;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.LangtonsAnt
{
	/// <summary>
	/// Description of LangtonsAnt.
	/// </summary>
	public class LangtonsAnt
		: Game
	{
		private readonly object _lockObject = new object();
		private readonly ConsoleGridRenderer _gridRenderer;
		private Grid<LangtonsAntCellMetadata> _grid;
        private Grid<LangtonsAntCellMetadata> _initialGrid;
        private LangtonsAntIterationCellGenerator _cellGenerator;
        private Coordinates2D _initialAntCoordinates;
        private Direction2D _initialAntDirection;

        public LangtonsAnt(ConsoleGridRenderer gridRenderer)
			: base(int.MaxValue)
		{
			_gridRenderer = gridRenderer;
		}
		
		protected override void ConfigureGame()
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Welcome to the Game of Life!");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine("How many rounds? ");
		    MaxRounds = int.Parse(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine("Interval? (ms)");
            int interval = int.Parse(Console.ReadLine());

            var ready = false;
            while (!ready) {
	            Console.WriteLine();
	            Console.WriteLine("Load from file? (y/n)");
	            if (Console.ReadLine() == "y")
	            {
	            	Console.WriteLine();
	            	Console.WriteLine("Filename: ");
	            	var filename = Console.ReadLine();
	            	
	            	if (!File.Exists(filename))
	            	{
						Console.WriteLine();
						Console.WriteLine("File not found.");
						Console.WriteLine();
						continue;
	            	}

	            	var parser = new LangtonsAntParsingCellGenerator(File.OpenText(filename).ReadToEnd());
	            	_initialAntCoordinates = parser.AntLocation;
	            	_initialAntDirection = parser.AntDirection.Value;
	            	
					_grid = new Grid<LangtonsAntCellMetadata>(new Dimensions2D(parser.MaxWidth, parser.MaxHeight), parser);
					ready = true;
	            } 
	            else 
	            {
		            Console.WriteLine();
		            Console.WriteLine("Width? ");
				    int width = int.Parse(Console.ReadLine());
		
		            Console.WriteLine();
		            Console.WriteLine("Height? ");
				    int height = int.Parse(Console.ReadLine());
		
		            Console.WriteLine();
		            Console.WriteLine("Life Probability? (1 in x)");
				    int life = int.Parse(Console.ReadLine());

		            Console.WriteLine();
		            Console.WriteLine("Ant location (x coordinate, 0-based)");
				    int x = int.Parse(Console.ReadLine());

		            Console.WriteLine();
		            Console.WriteLine("Ant location (y coordinate, 0-based)");
				    int y = int.Parse(Console.ReadLine());
				    
				    _initialAntCoordinates = new Coordinates2D(x, y);
				    
		            Console.WriteLine();
		            Console.WriteLine("Ant direction (u / d / l / r)");
				    switch (Console.ReadLine()[0]) {
				    	case 'U':
				    	case 'u':
				    		_initialAntDirection = Direction2D.Up;
				    		break;
				    	case 'D':
				    	case 'd':
				    		_initialAntDirection = Direction2D.Down;
				    		break;
				    	case 'R':
				    	case 'r':
				    		_initialAntDirection = Direction2D.Right;
				    		break;
				    	case 'L':
				    	case 'l':
				    		_initialAntDirection = Direction2D.Left;
				    		break;
				    	default:
				    		throw new InvalidOperationException("Invalid direction!");
				    }
				    
		            var randomizer = new LangtonsAntRandomCellGenerator(life, _initialAntCoordinates, _initialAntDirection);
		            _grid = new Grid<LangtonsAntCellMetadata>(new Dimensions2D(width, height), randomizer);
		            ready = true;
	            }

                _initialGrid = _grid;
            }
            
			ResetSimulation();

			Console.WriteLine();
			Console.WriteLine("Press any key to begin...");
			Console.ReadKey(true);
			
			AutoIncrementRound = TimeSpan.FromMilliseconds(interval);
		}

		public void ResetSimulation() {
			lock (_lockObject) {
				var copyGenerator = new LangtonsAntCopyCellGenerator(_initialGrid);
			
				_grid = new Grid<LangtonsAntCellMetadata>(_initialGrid.Dimensions, copyGenerator);
				_grid.Regenerate();
				_grid.CellGenerator = _cellGenerator = new LangtonsAntIterationCellGenerator(this, _grid, _gridRenderer, _initialAntCoordinates, _initialAntDirection);
	
				Console.Clear();
			    Console.CursorVisible = false;
	
	            _gridRenderer.RenderGrid(_initialGrid);
			}
		}
		
		public override void NextRound()
		{
			base.NextRound();

			lock (_lockObject) {
				_cellGenerator.Increment();
				//(CurrentRound % 2 == 0 ? _grid : _grid2).Regenerate();
	
				//_grid = new Grid<LangtonsAntCellMetadata>(_grid.Dimensions, new LangtonsAntIterationCellGenerator(this, _grid, _gridRenderer));
				//_gridRenderer.RenderGrid(_grid);
				
	            Console.SetCursorPosition(0, _grid.Dimensions.Height + 5);
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine();
				Console.WriteLine("Round: {0}                 ", CurrentRound);
				Console.WriteLine("Generate Time: {0} ms", DateTime.UtcNow.Subtract(RoundStarted).TotalMilliseconds);
				Console.WriteLine("Game Time: {0}", DateTime.UtcNow.Subtract(GameStarted));
	
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine();
				Console.WriteLine("Press [r] to reset, [q] to abort.  To reload/regenerate, press [g].");
			}
		}

		public override void HandleKeypress(char command) {
			switch(command)
			{
				case 'q':
					EndGame();
					break;
                case 'r':
					ResetSimulation();
			        break;
                case 'g':
			        _initialGrid.Regenerate();
                    ResetSimulation();
                    break;
                default:
					base.HandleKeypress(command);
					break;
			}
		}
		
		protected override void TeardownGame()
		{
            // TODO: show final metrics

            Console.CursorVisible = true;
            Console.WriteLine();
			Console.WriteLine("Goodbye!");
		    Console.ResetColor();
		}
	}
}
