using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using xtc.GameOfLife.GameOfLife;
using xtc.GameOfLife.Games;
using xtc.GameOfLife.Geometry;
using xtc.GameOfLife.Grids;

namespace GameOfLifeUI
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private const int CellSize = 7;
		private const int CellOffset = 10;
		private const int PaddingX = 20;
		private const int PaddingY = 70;

		private readonly GDIPlusGridRenderer _gridRenderer;
		private xtc.GameOfLife.GameOfLife.GameOfLife? _currentGame;
		private int _lastGridWidth;
		private int _lastGridHeight;

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			// Reduce flicker when drawing the grid
			typeof(Form).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
				.SetValue(this, true);

			_gridRenderer = new GDIPlusGridRenderer(this);
			_gridRenderer.OnRenderMessages += (messages) => {
				if (IsDisposed || !IsHandleCreated) return;
				try {
					if (InvokeRequired)
						BeginInvoke(new RenderMessagesEventHandler(RenderMessages), new object[] { messages });
					else
						RenderMessages(messages);
				} catch { /* form closing */ }
			};

			_gridRenderer.OnRenderGrid += (grid) => {
				if (IsDisposed || !IsHandleCreated) return;
				try {
					if (InvokeRequired)
						BeginInvoke(new RenderGridEventHandler<GameOfLifeCellMetadata>(RenderGrid), new object[] { grid });
					else
						RenderGrid(grid);
				} catch { /* form closing */ }
			};

			_gridRenderer.OnRenderCell += (cell) => {
				if (IsDisposed || !IsHandleCreated) return;
				try {
					if (InvokeRequired)
						BeginInvoke(new RenderCellEventHandler<GameOfLifeCellMetadata>(RenderCell), new object[] { cell });
					else
						RenderCell(cell);
				} catch { /* form closing */ }
			};

			this.button1.Click += async (sender, e) => {
				this.button1.Visible = false;
				int gridWidth = Math.Max(1, (ClientSize.Width - PaddingX) / CellSize);
				int gridHeight = Math.Max(1, (ClientSize.Height - PaddingY) / CellSize);
				var gol = new GameOfLife(_gridRenderer, new GameOfLifeConfiguration(500, 0, gridWidth, gridHeight, 2));
				_currentGame = gol;
				_lastGridWidth = 0;
				_lastGridHeight = 0;
				await gol.StartGameAsync();
			};

			this.MouseClick += MainForm_MouseClick;
		}
		
		private void MainForm_MouseClick(object? sender, MouseEventArgs e) {
			if (_currentGame == null || _currentGame.GameOver) return;
			// Toggle pause when clicking the grid area (below the button area)
			if (e.Y < ClientSize.Height - PaddingY) {
				if (_currentGame.IsPaused) {
					_currentGame.Resume();
					_gridRenderer.RenderMessages(new List<GameMessage> { new GameMessage("Resumed", true) });
				} else {
					_currentGame.Pause();
					_gridRenderer.RenderMessages(new List<GameMessage> { new GameMessage("Paused - click to resume", true) });
				}
			}
		}

		private void ResizeForm(int x, int y) {
			var deltaX = x - this.ClientRectangle.Width;
			var deltaY = y - this.ClientRectangle.Height;

			this.Width += deltaX;
			this.Height += deltaY;
		}

		private void RenderCell(Cell<GameOfLifeCellMetadata> cell) {
			if (IsDisposed || !IsHandleCreated) return;
			try {
				using (var g = Graphics.FromHwnd(this.Handle))
					DrawCell(g, cell);
			} catch { /* form closing */ }
		}
		
		private void RenderGrid(Grid<GameOfLifeCellMetadata> grid) {
			if (IsDisposed || !IsHandleCreated) return;
			try {
				if (WindowState != FormWindowState.Maximized) {
					ResizeForm((grid.Dimensions.Width * CellSize) + PaddingX, (grid.Dimensions.Height * CellSize) + PaddingY);
				}

				int w = grid.Dimensions.Width;
				int h = grid.Dimensions.Height;
				bool dimensionsChanged = _lastGridWidth != w || _lastGridHeight != h;
				_lastGridWidth = w;
				_lastGridHeight = h;

				using (var g = Graphics.FromHwnd(this.Handle)) {
					// Only clear when grid size changed (e.g. reset); otherwise draw over existing for incremental effect
					if (dimensionsChanged) {
						using (var b = new SolidBrush(Color.Black))
							g.FillRectangle(b, this.ClientRectangle);
					}

					using (var p = new Pen(Color.Cyan)) {
						g.DrawRectangle(p, 2, 2, this.ClientRectangle.Width - 4, this.ClientRectangle.Height - 54);
						g.DrawRectangle(p, 5, 5, this.ClientRectangle.Width - 10, this.ClientRectangle.Height - 60);
					}

					// Left-to-right then top-to-bottom so changes appear to sweep across
					for (int x = 0; x < w; x++) {
						for (int y = 0; y < h; y++) {
							var cell = grid[new Coordinates2D(x, y)];
							DrawCell(g, cell);
						}
					}
				}
			} catch { /* handle invalidated during draw */ }
		}

		private static Color GetCellColor(GameOfLifeCellMetadata payload) {
			var color = payload.IsAlive ? Color.DarkGreen : Color.Gray;
			switch (payload.Rule) {
				case GameOfLifeRule.KeepAlive: return Color.Green;
				case GameOfLifeRule.Respawn: return Color.LightGreen;
				case GameOfLifeRule.Overcrowded: return Color.Magenta;
				case GameOfLifeRule.Underpopulated: return Color.LightYellow;
				case GameOfLifeRule.NoMatch: return Color.DarkGray;
				default: throw new InvalidOperationException("Unknown rule.");
			}
		}

		private void DrawCell(Graphics g, Cell<GameOfLifeCellMetadata> cell) {
			Color color = GetCellColor(cell.Payload);
			int px = (cell.Coordinates.X * CellSize) + CellOffset;
			int py = (cell.Coordinates.Y * CellSize) + CellOffset;

			using (var b = new SolidBrush(Color.Black))
				g.FillRectangle(b, px, py, CellSize, CellSize);

			if (cell.Payload.IsAlive) {
				using (var b = new SolidBrush(color))
					g.FillRectangle(b, px + 1, py + 1, 5, 5);
				foreach (var c in cell.Neighbors.Where(n => n.Payload.IsAlive)) {
					int dx = px + 3, dy = py + 3;
					if (c.Coordinates.X > cell.Coordinates.X) dx += 3;
					else if (c.Coordinates.X < cell.Coordinates.X) dx -= 3;
					if (c.Coordinates.Y > cell.Coordinates.Y) dy += 3;
					else if (c.Coordinates.Y < cell.Coordinates.Y) dy -= 3;
					using (var b = new SolidBrush(Color.White))
						g.FillRectangle(b, dx, dy, 1, 1);
				}
			} else {
				using (var b = new SolidBrush(color))
					g.FillRectangle(b, px + 3, py + 3, 1, 1);
			}
		}

		private void RenderMessages(IEnumerable<GameMessage> messages) {
			if (IsDisposed || !IsHandleCreated) return;
			try {
			int position = this.ClientRectangle.Height - 50;

			using (var g = Graphics.FromHwnd(this.Handle)) {
				using (var b = new SolidBrush(Color.Black))
					g.FillRectangle(b, 0, position, this.ClientRectangle.Width, this.ClientRectangle.Height - position);
				
				using (var f = new Font("Consolas", 7, FontStyle.Regular, GraphicsUnit.Point)) {
					foreach (var message in messages) {
						using (var b = new SolidBrush(message.IsWarning ? Color.Yellow : Color.Red)) {
							g.DrawString(message.Message, f, b, 0, position);
							position += g.MeasureString(message.Message, f).ToSize().Height;
						}
					}
				}
			}
			} catch { /* form closing */ }
		}
	}
}
