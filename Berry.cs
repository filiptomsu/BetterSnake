using System;

namespace Snake
{
    class Berry
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Color { get; set; } = ConsoleColor.Cyan;

        public void SetNewPosition(Random random, int screenWidth, int screenHeight)
        {
            X = random.Next(1, screenWidth - 1);
            Y = random.Next(1, screenHeight - 1);
        }
    }
}
