using System;
using System.IO;

namespace xtc.GameOfLife.GameOfLife
{
	/// <summary>
	/// Description of GameOfLifeConfiguration.
	/// </summary>
	public class GameOfLifeConfiguration
	{
		public int Rounds { get; set; }
		public int Interval { get; set; }
		public string Filename { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int LifeProbability { get; set; }

		private GameOfLifeConfiguration()
		{
		}

		public GameOfLifeConfiguration(int rounds, int interval, string filename)
		{
			Rounds = rounds;
			Interval = interval;
			Filename = filename;
		}

		public GameOfLifeConfiguration(int rounds, int interval, int width, int height, int lifeProbability)
		{
			Rounds = rounds;
			Interval = interval;
			Width = width;
			Height = height;
			LifeProbability = lifeProbability;
		}
		
		public static GameOfLifeConfiguration ReadFromConsole() {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Welcome to the Game of Life!");
			Console.WriteLine();

			Console.ForegroundColor = ConsoleColor.White;
			var rounds = ReadIntFromConsole("How many rounds?");
            var interval = ReadIntFromConsole("Interval? (ms)");

            while (true) {
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

	            	return new GameOfLifeConfiguration(rounds, interval, filename);
	            } 
	            else 
	            {
		            var width = ReadIntFromConsole("Width?");
		            var height = ReadIntFromConsole("Height?");
		            var life = ReadIntFromConsole("Life Probability? (1 in x)");
		
		            return new GameOfLifeConfiguration(rounds, interval, width, height, life);
	            }
            }
		}
		
		private static int ReadIntFromConsole(string prompt) {
			while (true) {
				Console.WriteLine();
				Console.WriteLine(prompt);
				
				int value;
				if (int.TryParse(Console.ReadLine(), out value))
					return value;
				
				Console.WriteLine("Invalid entry!");
			}
		}
	}
}
