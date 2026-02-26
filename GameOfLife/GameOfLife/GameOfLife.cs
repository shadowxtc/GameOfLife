using System;
using System.IO;
using System.Collections.Generic;
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
        private readonly GameOfLifeConfiguration _configuration;

        public GameOfLife(IGridRenderer<GameOfLifeCellMetadata> gridRenderer, GameOfLifeConfiguration configuration)
			: base(int.MaxValue)
		{
			_gridRenderer = gridRenderer;
			_configuration = configuration;
		}
		
		protected override void ConfigureGame()
		{
			ConfigureGameAsync().GetAwaiter().GetResult();
		}

		protected override async Task ConfigureGameAsync()
		{
			_gridRenderer.StartSession();

			if (string.IsNullOrWhiteSpace(_configuration.Filename)) {
	            _grid = new Grid<GameOfLifeCellMetadata>(new Dimensions2D(_configuration.Width, _configuration.Height), new GameOfLifeRandomCellGenerator(_configuration.LifeProbability));
			} else {
            	var parser = new GameOfLifeParsingCellGenerator(File.OpenText(_configuration.Filename).ReadToEnd());
				_grid = new Grid<GameOfLifeCellMetadata>(new Dimensions2D(parser.MaxWidth, parser.MaxHeight), parser);
			}

            _initialGrid = _grid;

			ResetSimulation();

			await _gridRenderer.PromptToContinueAsync();

			AutoIncrementRound = TimeSpan.FromMilliseconds(_configuration.Interval);
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

				_gridRenderer.StartSession();
	            _gridRenderer.RenderGrid(_initialGrid);
			}
		}
		
		public override void NextRound()
		{
			base.NextRound();

			lock (_lockObject) {
				var activeGrid = CurrentRound % 2 == 0 ? _grid : _grid2;
				activeGrid.Regenerate();
				_gridRenderer.RenderGrid(activeGrid);
				ShowMetrics(false);
			}
		}

		private void ShowMetrics(bool ending) {
			var cells = _grid.Dimensions.Width * _grid.Dimensions.Height;
			var duration = DateTime.UtcNow.Subtract(RoundStarted).TotalMilliseconds;
			var cps = ((decimal)1 / ((decimal)duration / (decimal)1000)) * (decimal)cells;
			
			var messages = new List<GameMessage>();
			messages.Add(new GameMessage(string.Format("Grid {0}x{1} - Cells {2} - Round: {3}", _grid.Dimensions.Width, _grid.Dimensions.Height, cells, CurrentRound)));
			messages.Add(new GameMessage(string.Format("Generate Time: {0:#,##0.0} ms ({1:#,##0.0} cps)", duration, cps)));
			messages.Add(new GameMessage(string.Format("Game Time: {0}", DateTime.UtcNow.Subtract(GameStarted))));

			messages.Add(new GameMessage(""));
				
			if (ending) {
				messages.Add(new GameMessage("Goodbye!", true));
			} else {
				messages.Add(new GameMessage("Press [r] to reset, [q] to abort.  To reload/regenerate, press [g].", true));
			}
			
			_gridRenderer.RenderMessages(messages);
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
			ShowMetrics(true);
			_gridRenderer.EndSession();
		}
	}
}
