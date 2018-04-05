﻿using System.Drawing;

namespace aMaze_ingSolver.Tree
{
    class Utils
    {
        public static Direction GetDirection(Point origin, Point destination)
        {
            int deltaX = origin.X - destination.X;
            int deltaY = origin.Y - destination.Y;

            if (deltaX == 0)
            {
                return (deltaY > 0) ? Direction.Up : Direction.Down;
            }
            else if (deltaY == 0)
            {
                return (deltaX > 0) ? Direction.Left : Direction.Right;
            }

            return Direction.NoDirection;
        }

        
    }
}
