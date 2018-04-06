using System.Drawing;

namespace aMaze_ingSolver.GraphUtils
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

        public static int Modulo(int value, int moduloValue)
        {
            if(value < 0)
            {
                return moduloValue + value;
            }

            else
            {
                return value % moduloValue;
            }
        }

        
    }
}
