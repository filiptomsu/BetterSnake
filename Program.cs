using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
///█ ■
////https://www.youtube.com/watch?v=SGZgvMwjq2U
namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;

            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;
            
            Random randomNumber = new Random();
            
            int score = 5;
            int gameover = 0;
            
            Pixel food = new Pixel();
            food.XPos = screenWidth / 2;
            food.YPos = screenHeight / 2;
            food.color = ConsoleColor.Red;

            Direction movement = Direction.Right;
            List<int> XPosBody = new List<int>();
            List<int> YPosBody = new List<int>();
            int XBerry = randomNumber.Next(0, screenWidth);
            int YBerry = randomNumber.Next(0, screenHeight);
            DateTime time = DateTime.Now;
            DateTime time2 = DateTime.Now;
            bool buttonPressed = false;
            while (true)
            {
                Console.Clear();
                if (food.XPos == screenWidth - 1 || food.XPos == 0 || food.YPos == screenHeight - 1 || food.YPos == 0)
                {
                    gameover = 1;
                }
                for (int i = 0; i < screenWidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("■");
                }
                for (int i = 0; i < screenWidth; i++)
                {
                    Console.SetCursorPosition(i, screenHeight - 1);
                    Console.Write("■");
                }
                for (int i = 0; i < screenHeight; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("■");
                }
                for (int i = 0; i < screenHeight; i++)
                {
                    Console.SetCursorPosition(screenWidth - 1, i);
                    Console.Write("■");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                if (XBerry == food.XPos && YBerry == food.YPos)
                {
                    score++;
                    XBerry = randomNumber.Next(1, screenWidth - 2);
                    YBerry = randomNumber.Next(1, screenHeight - 2);
                }
                for (int i = 0; i < XPosBody.Count(); i++)
                {
                    Console.SetCursorPosition(XPosBody[i], YPosBody[i]);
                    Console.Write("■");
                    if (XPosBody[i] == food.XPos && YPosBody[i] == food.YPos)
                    {
                        gameover = 1;
                    }
                }
                if (gameover == 1)
                {
                    break;
                }
                Console.SetCursorPosition(food.XPos, food.YPos);
                Console.ForegroundColor = food.color;
                Console.Write("■");
                Console.SetCursorPosition(XBerry, YBerry);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");
                time = DateTime.Now;
                buttonPressed = false;
                while (true)
                {
                    time2 = DateTime.Now;
                    if (time2.Subtract(time).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                        if (pressedKey.Key.Equals(ConsoleKey.UpArrow) && movement != Direction.Down && buttonPressed == false)
                        {
                            movement = Direction.Up;
                            buttonPressed = true;
                        }
                        if (pressedKey.Key.Equals(ConsoleKey.DownArrow) && movement != Direction.Up && buttonPressed == false)
                        {
                            movement = Direction.Down;
                            buttonPressed = true;
                        }
                        if (pressedKey.Key.Equals(ConsoleKey.LeftArrow) && movement != Direction.Right && buttonPressed == false)
                        {
                            movement = Direction.Left;
                            buttonPressed = true;
                        }
                        if (pressedKey.Key.Equals(ConsoleKey.RightArrow) && movement != Direction.Left && buttonPressed == false)
                        {
                            movement = Direction.Right;
                            buttonPressed = true;
                        }
                    }
                }
                XPosBody.Add(food.XPos);
                YPosBody.Add(food.YPos);
                switch (movement)
                {
                    case Direction.Up:
                        food.YPos--;
                        break;
                    case Direction.Down:
                        food.YPos++;
                        break;
                    case Direction.Left:
                        food.XPos--;
                        break;
                    case Direction.Right:
                        food.XPos++;
                        break;
                }
                if (XPosBody.Count() > score)
                {
                    XPosBody.RemoveAt(0);
                    YPosBody.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
        }
        class Pixel
        {
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor color { get; set; }
        }
        enum Direction
        {
            Left, Right, Up, Down
        }
    }
    
}
//¦