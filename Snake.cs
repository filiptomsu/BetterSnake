using System;
using System.Collections.Generic;

namespace Snake
{
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

        public void Move()
        {
            Body.Add(new Pixel { xPosition = Head.xPosition, yPosition = Head.yPosition, screenColor = ConsoleColor.Green });
            switch (Direction)
            {
                case DirectionEnum.Up: Head.yPosition--; break;
                case DirectionEnum.Down: Head.yPosition++; break;
                case DirectionEnum.Left: Head.xPosition--; break;
                case DirectionEnum.Right: Head.xPosition++; break;
            }
        }

        public void TrimBody(int desiredLength)
        {
            while (Body.Count > desiredLength)
                Body.RemoveAt(0);
        }

        public bool CheckSelfCollision()
        {
            return Body.Contains(Head);
        }
    }
}
