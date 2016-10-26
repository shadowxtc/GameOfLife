/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/23/2016
 * Time: 5:16 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using xtc.GameOfLife.GameOfLife;
using xtc.GameOfLife.Games;
using xtc.GameOfLife.Grids;

namespace GameOfLifeUI
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private readonly GDIPlusGridRenderer _gridRenderer;
		private readonly BackgroundWorker _bgWorker;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();

			_bgWorker = new BackgroundWorker();
			

			_gridRenderer = new GDIPlusGridRenderer(this);
			_gridRenderer.OnRenderMessages += (messages) => {
				if (this.IsDisposed)
					return;
				
				if (this.InvokeRequired) {
					this.Invoke(new RenderMessagesEventHandler(RenderMessages), new [] { messages });
				} else {
					RenderMessages(messages);
				}
			};
			
			_gridRenderer.OnRenderGrid += (grid) => {
				if (this.IsDisposed)
					return;
				
				if (this.InvokeRequired) {
					this.Invoke(new RenderGridEventHandler<GameOfLifeCellMetadata>(RenderGrid), new [] { grid });
				} else {
					RenderGrid(grid);
				}
			};
			
			_gridRenderer.OnRenderCell += (cell) => {
				if (this.IsDisposed)
					return;
				
				if (this.InvokeRequired) {
					this.Invoke(new RenderCellEventHandler<GameOfLifeCellMetadata>(RenderCell), new [] { cell });
				} else {
					RenderCell(cell);
				}
			};

			this.button1.Click += (sender, e) => {
				this.button1.Visible = false;
				var gol = new GameOfLife(_gridRenderer, new GameOfLifeConfiguration(500, 0, this.ClientRectangle.Width / 5, this.ClientRectangle.Height / 5, 2));
				gol.StartGame();
			};
		}
		
		private void ResizeForm(int x, int y) {
			var deltaX = x - this.ClientRectangle.Width;
			var deltaY = y - this.ClientRectangle.Height;

			this.Width += deltaX;
			this.Height += deltaY;
		}

		private void RenderCell(Cell<GameOfLifeCellMetadata> cell) {
			var color = cell.Payload.IsAlive ? Color.DarkGreen : Color.Gray;
			
			switch (cell.Payload.Rule) {
				case GameOfLifeRule.KeepAlive:
					color = Color.Green;
					break;
				case GameOfLifeRule.Respawn:
					color = Color.LightGreen;
					break;
				case GameOfLifeRule.Overcrowded:
					color = Color.Magenta;
					break;
				case GameOfLifeRule.Underpopulated:
					color = Color.LightYellow;
					break;
				case GameOfLifeRule.NoMatch:
					color = Color.DarkGray;
					break;
				default:
					throw new InvalidOperationException("Unknown rule.");
			}

			var x = (cell.Coordinates.X * 5) + 10;
			var y = (cell.Coordinates.Y * 5) + 10;
			
			using (var g = Graphics.FromHwnd(this.Handle)) {
				using (var b = new SolidBrush(cell.Payload.IsAlive ? color : Color.Black))
					g.FillRectangle(b, x + 1, y + 1, 3, 3);
					
				if (!cell.Payload.IsAlive)
					using (var b = new SolidBrush(color))
						g.FillRectangle(b, x + 2, y + 2, 1, 1);
			}
		}
		
		private void RenderGrid(Grid<GameOfLifeCellMetadata> grid) {
			int paddingX = 20; // .|.|......  ......|.|.
			int paddingY = 70; // 10 top, 10 bottom just like sides, plus room for text
			
			ResizeForm((grid.Dimensions.Width * 5) + paddingX, (grid.Dimensions.Height * 5) + paddingY);

			using (var g = Graphics.FromHwnd(this.Handle)) {
				using (var b = new SolidBrush(Color.Black))
					g.FillRectangle(b, this.ClientRectangle);

				using (var p = new Pen(Color.Cyan)) {
					g.DrawRectangle(p, 2, 2, this.ClientRectangle.Width - 4, this.ClientRectangle.Height - 54);
					g.DrawRectangle(p, 5, 5, this.ClientRectangle.Width - 10, this.ClientRectangle.Height - 60);
				}
			}
		}

		private void RenderMessages(IEnumerable<GameMessage> messages) {
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
		}
	}
}
