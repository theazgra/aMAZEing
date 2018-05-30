using System.Drawing;

namespace aMaze_ingSolver.GraphUtils
{
    class Utils
    {
        /// <summary>
        /// Get direction between two vertices.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="destination">Destination.</param>
        /// <returns>Direction.</returns>
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

        /// <summary>
        /// Simple modulo python way.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public static int Modulo(int value, int modulo)
        {
            if (value < 0)
            {
                return modulo + value;
            }
            else
            {
                return value % modulo;
            }
        }
    }
}
