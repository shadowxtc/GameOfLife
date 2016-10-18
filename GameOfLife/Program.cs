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
using xtc.GameOfLife.GameOfLife;

namespace xtc.GameOfLife
{
	class Program
	{
		public static void Main(string[] args)
		{
			while (true) {
			    var game = new GameOfLife.GameOfLife(new ConsoleGridRenderer());
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