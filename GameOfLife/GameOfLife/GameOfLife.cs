/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/16/2016
 * Time: 1:36 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Threading;
using xtc.GameOfLife.Games;
using xtc.GameOfLife.Grids;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.GameOfLife
{
	/// <summary>
	/// Description of GameOfLife.
	/// </summary>
	public class GameOfLife
		: Game
	{
		private readonly object _lockObject = new object();
		private readonly IGridRenderer<GameOfLifeCellMetadata> _gridRenderer;
		private Grid<GameOfLifeCellMetadata> _grid;
		private Grid<GameOfLifeCellMetadata> _grid2;
        private Grid<GameOfLifeCellMetadata> _initialGrid;

        public GameOfLife(IGridRenderer<GameOfLifeCellMetadata> gridRenderer)
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

	            	var parser = new GameOfLifeParsingCellGenerator(File.OpenText(filename).ReadToEnd());
					_grid = new Grid<GameOfLifeCellMetadata>(new Dimensions2D(parser.MaxWidth, parser.MaxHeight), parser);
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
		
		            _grid = new Grid<GameOfLifeCellMetadata>(new Dimensions2D(width, height), new GameOfLifeRandomCellGenerator(life));
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
				var copyGenerator = new GameOfLifeCopyCellGenerator(_initialGrid);
				
				_grid = new Grid<GameOfLifeCellMetadata>(_initialGrid.Dimensions, copyGenerator);
				_grid2 = new Grid<GameOfLifeCellMetadata>(_initialGrid.Dimensions, copyGenerator);
				_grid.Regenerate();
				_grid2.Regenerate();
				_grid.CellGenerator = new GameOfLifeIterationCellGenerator(this, _grid2, _gridRenderer);
				_grid2.CellGenerator = new GameOfLifeIterationCellGenerator(this, _grid, _gridRenderer);
	
				Console.Clear();
			    Console.CursorVisible = false;
	
	            _gridRenderer.RenderGrid(_initialGrid);
			}
		}
		
		public override void NextRound()
		{
			base.NextRound();

			lock (_lockObject) {
				(CurrentRound % 2 == 0 ? _grid : _grid2).Regenerate();
	
				//_grid = new Grid<GameOfLifeCellMetadata>(_grid.Dimensions, new GameOfLifeIterationCellGenerator(this, _grid, _gridRenderer));
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
