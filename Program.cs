using System;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 17;
            Console.WindowWidth = 32;
            int gameAreaHeight = Console.WindowHeight - 1;
            int gameAreaWidth = Console.WindowWidth;

            SnakeGame game = new SnakeGame(gameAreaWidth, gameAreaHeight);
            game.Run();

            Console.ReadKey();
        }
    }
}
