using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
    // Výčtový typ pro směry
    enum DirectionEnum
    {
        Up,
        Down,
        Left,
        Right
    }

    // Reprezentuje jeden pixel na konzoli.
    class Pixel
    {
        public int xPosition { get; set; }
        public int yPosition { get; set; }
        public ConsoleColor screenColor { get; set; }
    }

    // Třída reprezentující hada.
    class Snake
    {
        public Pixel Head { get; set; }
        public List<Pixel> Body { get; set; }
        public DirectionEnum Direction { get; set; }

        public Snake(int startX, int startY)
        {
            Head = new Pixel { xPosition = startX, yPosition = startY, screenColor = ConsoleColor.Red };
            Body = new List<Pixel>();
            Direction = DirectionEnum.Right;
        }

        // Metoda pro pohyb hada: přidá aktuální pozici hlavy do těla a změní pozici hlavy dle směru.
        public void Move()
        {
            // Uloží aktuální pozici hlavy do těla
            Body.Add(new Pixel { xPosition = Head.xPosition, yPosition = Head.yPosition, screenColor = ConsoleColor.Green });
            // Aktualizace pozice hlavy podle směru
            switch (Direction)
            {
                case DirectionEnum.Up:
                    Head.yPosition--;
                    break;
                case DirectionEnum.Down:
                    Head.yPosition++;
                    break;
                case DirectionEnum.Left:
                    Head.xPosition--;
                    break;
                case DirectionEnum.Right:
                    Head.xPosition++;
                    break;
            }
        }

        // Udržuje délku hada podle skóre (počet segmentů)
        public void TrimBody(int desiredLength)
        {
            while (Body.Count > desiredLength)
            {
                Body.RemoveAt(0);
            }
        }

        // Kontrola kolize s vlastním tělem
        public bool CheckSelfCollision()
        {
            foreach (var segment in Body)
            {
                if (segment.xPosition == Head.xPosition && segment.yPosition == Head.yPosition)
                    return true;
            }
            return false;
        }
    }

    // Třída reprezentující bobulku, kterou had jí.
    class Berry
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Color { get; set; } = ConsoleColor.Cyan;

        // Nastaví novou náhodnou pozici bobulky uvnitř herní plochy (mimo okraj).
        public void SetNewPosition(Random random, int screenWidth, int screenHeight)
        {
            X = random.Next(1, screenWidth - 1);
            Y = random.Next(1, screenHeight - 1);
        }
    }

    // Hlavní herní třída, která řídí hru.
    class SnakeGame
    {
        private int screenWidth;
        private int screenHeight; // výška herní plochy (bez řádku pro skóre)
        private int score;
        private bool gameOver;
        private Snake snake;
        private Berry berry;
        private Random randomGenerator;
        private DateTime lastMoveTime;
        private const int moveDelay = 500; // prodleva v milisekundách

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

        // Hlavní smyčka hry.
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
                DrawScore(); // Zobrazení skóre pod herním polem
                Thread.Sleep(10); // krátká pauza pro snížení zatížení CPU
            }

            // Zobrazení zprávy o ukončení hry
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + (score - 5));
        }

        // Zpracování vstupu z klávesnice.
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

        // Aktualizace stavu hry (pohyb hada, kontrola kolizí, atd.).
        private void UpdateGame()
        {
            snake.Move();

            // Kontrola kolize s okrajem
            if (snake.Head.xPosition <= 0 || snake.Head.xPosition >= screenWidth - 1 ||
                snake.Head.yPosition <= 0 || snake.Head.yPosition >= screenHeight - 1)
            {
                gameOver = true;
                return;
            }

            // Kontrola kolize se svým tělem
            if (snake.CheckSelfCollision())
            {
                gameOver = true;
                return;
            }

            // Kontrola, zda had snědl bobulku
            if (snake.Head.xPosition == berry.X && snake.Head.yPosition == berry.Y)
            {
                score++;
                berry.SetNewPosition(randomGenerator, screenWidth, screenHeight);
            }
            else
            {
                // Pokud nebyla snězena bobulka, zkrátíme ocas hada na požadovanou délku (podle skóre)
                snake.TrimBody(score);
            }
        }

        // Vykreslení herních objektů.
        private void DrawGame()
        {
            // Vykreslení těla hada
            foreach (var segment in snake.Body)
            {
                Console.SetCursorPosition(segment.xPosition, segment.yPosition);
                Console.ForegroundColor = segment.screenColor;
                Console.Write("■");
            }

            // Vykreslení hlavy hada
            Console.SetCursorPosition(snake.Head.xPosition, snake.Head.yPosition);
            Console.ForegroundColor = snake.Head.screenColor;
            Console.Write("■");

            // Vykreslení bobulky
            Console.SetCursorPosition(berry.X, berry.Y);
            Console.ForegroundColor = berry.Color;
            Console.Write("■");
        }

        // Vykreslí okraj hry.
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

        // Vymaže vnitřek herní plochy (ponechává okraj).
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

        // Vykreslí skóre pod herním polem.
        private void DrawScore()
        {
            // Score bude zobrazeno na řádku, který následuje herní pole.
            Console.SetCursorPosition(0, screenHeight);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"Score: {score - 5}   ");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Nastavení konzole: okno má jeden řádek navíc pro skóre (celkem 17 řádků)
            Console.WindowHeight = 17;
            Console.WindowWidth = 32;
            int gameAreaHeight = Console.WindowHeight - 1; // 16 řádků pro herní pole
            int gameAreaWidth = Console.WindowWidth;

            SnakeGame game = new SnakeGame(gameAreaWidth, gameAreaHeight);
            game.Run();

            // Zajištění, že konzole zůstane otevřená po skončení hry.
            Console.ReadKey();
        }
    }
}
