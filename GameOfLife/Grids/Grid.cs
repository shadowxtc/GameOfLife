using System;
using System.Collections.Generic;
using xtc.GameOfLife.Geometry;

namespace xtc.GameOfLife.Grids
{
	/// <summary>
	/// Description of Grid.
	/// </summary>
	public class Grid<T>
	{
		private readonly Cell<T>[][] _elementMatrix;

		public ICellGenerator<T> CellGenerator { get; set; }
		public Dimensions2D Dimensions { get; private set; }

		public Cell<T> this[Coordinates2D coordinates] {
			get {
				AssertCoordinates(coordinates);
				return _elementMatrix[coordinates.X][coordinates.Y];
			}
			set {
				AssertCoordinates(coordinates);
				_elementMatrix[coordinates.X][coordinates.Y] = value;
			}
		}

		private Grid()
		{
		}
		
		public Grid(Dimensions2D dimensions, ICellGenerator<T> cellGenerator) {
			Dimensions = dimensions;
			CellGenerator = cellGenerator;
			_elementMatrix = new Cell<T>[dimensions.Width][];

			for (var x = 0; x < dimensions.Width; ++x)
				_elementMatrix[x] = new Cell<T>[dimensions.Height];
			
			Regenerate();
		}
		
		public void Regenerate() {
			for (var x = 0; x < Dimensions.Width; x++)
				for (var y = 0; y < Dimensions.Height; ++y)
					_elementMatrix[x][y] = CellGenerator.Generate(this, new Coordinates2D(x, y));
		}
		
		public IEnumerable<Cell<T>> GetRow(int y) {
			var elements = new List<Cell<T>>();
			
			for (var x = 0; x < Dimensions.Width; ++x)
				elements.Add(_elementMatrix[x][y]);
			
			return elements;
		}
		
		public IEnumerable<Cell<T>> GetColumn(int x) {
			return new List<Cell<T>>(_elementMatrix[x]);
		}
		// TODO: Override tostring
		// TODO: Override tostring in Cell
		// TODO: Add support for ICellCustomRenderer
		
		private void AssertCoordinates(Coordinates2D coordinates) {
			if (coordinates == null)
				throw new ArgumentNullException("coordinates");

			if (!VerifyCoordinates(coordinates.X, coordinates.Y))
				throw new ArgumentOutOfRangeException("coordinates");
		}
		
		public bool VerifyCoordinates(int x, int y) {
			return x < Dimensions.Width &&
			       y < Dimensions.Height &&
				   x >= 0 &&
				   y >= 0;
		}
	}
}
