using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    class SnakeGame
    {
        private int screenWidth;
        private int screenHeight;
        private int score;
        private bool gameOver;
        private Snake snake;
        private Berry berry;
        private Random randomGenerator;
        private DateTime lastMoveTime;
        private const int moveDelay = 500;

        public SnakeGame(int width, int height)
        {
            screenWidth = width;
            screenHeight = height;
            score = 5;
            gameOver = false;
            randomGenerator = new Random();
            int startX = screenWidth / 2;
            int startY = screenHeight / 2;
            snake = new Snake(startX, startY);
            berry = new Berry();
            berry.SetNewPosition(randomGenerator, screenWidth, screenHeight);
            lastMoveTime = DateTime.Now;
        }

        public void Run()
        {
            Console.CursorVisible = false;
            DrawBorder();

            while (!gameOver)
            {
                ClearConsole();
                ProcessInput();
                if ((DateTime.Now - lastMoveTime).TotalMilliseconds >= moveDelay)
                {
                    lastMoveTime = DateTime.Now;
                    UpdateGame();
                }
                DrawGame();
                DrawScore();
                Thread.Sleep(10);
            }

            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + (score - 5));
        }

        private void ProcessInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyPress = Console.ReadKey(true);
                switch (keyPress.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (snake.Direction != DirectionEnum.Down)
                            snake.Direction = DirectionEnum.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        if (snake.Direction != DirectionEnum.Up)
                            snake.Direction = DirectionEnum.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (snake.Direction != DirectionEnum.Right)
                            snake.Direction = DirectionEnum.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (snake.Direction != DirectionEnum.Left)
                            snake.Direction = DirectionEnum.Right;
                        break;
                }
            }
        }

        private void UpdateGame()
        {
            snake.Move();

            if (snake.Head.xPosition <= 0 || snake.Head.xPosition >= screenWidth - 1 ||
                snake.Head.yPosition <= 0 || snake.Head.yPosition >= screenHeight - 1)
            {
                gameOver = true;
                return;
            }

            if (snake.CheckSelfCollision())
            {
                gameOver = true;
                return;
            }

            if (snake.Head.xPosition == berry.X && snake.Head.yPosition == berry.Y)
            {
                score++;
                berry.SetNewPosition(randomGenerator, screenWidth, screenHeight);
            }
            else
            {
                snake.TrimBody(score);
            }
        }

        private void DrawGame()
        {
            foreach (var segment in snake.Body)
            {
                Console.SetCursorPosition(segment.xPosition, segment.yPosition);
                Console.ForegroundColor = segment.screenColor;
                Console.Write("■");
            }

            Console.SetCursorPosition(snake.Head.xPosition, snake.Head.yPosition);
            Console.ForegroundColor = snake.Head.screenColor;
            Console.Write("■");

            Console.SetCursorPosition(berry.X, berry.Y);
            Console.ForegroundColor = berry.Color;
            Console.Write("■");
        }

        private void DrawBorder()
        {
            string horizontalBar = new string('■', screenWidth);
            Console.SetCursorPosition(0, 0);
            Console.Write(horizontalBar);
            Console.SetCursorPosition(0, screenHeight - 1);
            Console.Write(horizontalBar);

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }
        }

        private void ClearConsole()
        {
            string emptyLine = new string(' ', screenWidth - 2);
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 1; i < screenHeight - 1; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write(emptyLine);
            }
        }

        private void DrawScore()
        {
            Console.SetCursorPosition(0, screenHeight);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Score: {score - 5}   ");
        }
    }
}
