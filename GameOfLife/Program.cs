/*
 * Created by SharpDevelop.
 * User: shado
 * Date: 10/15/2016
 * Time: 8:11 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Threading;
using xtc.GameOfLife.Games;

namespace xtc.GameOfLife
{
	class Program
	{
		public static void Main(string[] args)
		{
			while (true) {
				Console.WriteLine();
				Console.WriteLine("Welcome to shadowxtc's C# Cellular Automata Simulation Playground");
				Console.WriteLine();
				Console.WriteLine("[1] Game of Life");
				Console.WriteLine("[2] Day and Night");
				Console.WriteLine("[3] Langton's Ant");
				Console.WriteLine();
				Console.WriteLine("Your choice?");
				
				Game game;
				var result = Console.ReadLine();
				
				if (result == "1") {
					game = (Game) new GameOfLife.GameOfLife(new GameOfLife.ConsoleGridRenderer());
				}else if (result == "2") {
					game = (Game) new DayAndNight.DayAndNight(new DayAndNight.ConsoleGridRenderer());
				}else {
					game = (Game) new LangtonsAnt.LangtonsAnt(new LangtonsAnt.ConsoleGridRenderer());
				}
				
			    game.StartGame();
			    
			    while (game.CurrentRound < game.MaxRounds && !game.GameOver)
			    {
			    	Thread.Sleep(50);
			    	
			    	if (Console.KeyAvailable)
			    		game.HandleKeypress(Console.ReadKey(true).KeyChar);
			    }
			    
			    game.EndGame();
			    
			    while (!game.GameOver) 
			    	Thread.Sleep(50);
		
			    Console.WriteLine("Again? (y/n)");
			    
			    if (Console.ReadLine() != "y")
			    	break;
			}

			Console.WriteLine();
			Console.Write("Press any key to exit...");
			Console.ReadKey(true);
		}
	}
}