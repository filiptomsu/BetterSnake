using System;

namespace Snake
{
    class Pixel
    {
        public int xPosition { get; set; }
        public int yPosition { get; set; }
        public ConsoleColor screenColor { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Pixel other)
                return xPosition == other.xPosition && yPosition == other.yPosition;
            return false;
        }

        public override int GetHashCode()
        {
            return xPosition * 397 ^ yPosition;
        }
    }
}
