using aMaze_ingSolver.GraphUtils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver
{
    static class Extensions
    {
        public static bool ContainsLocation(this IEnumerable<Vertex> vertices, Point location)
        {
            Vertex found = vertices.FirstOrDefault(n => n.X == location.X && n.Y == location.Y);

            return (found != null);
        }

        public static Point MoveInDirection(this Point origin, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return origin.Up();
                case Direction.Down:
                    return origin.Down();
                case Direction.Left:
                    return origin.Left();
                case Direction.Right:
                    return origin.Right();
                default:
                    return origin;
            }
        }

        //Maze movement
        public static Point Up(this Point origin) => new Point(origin.X, origin.Y - 1);
        public static Point Down(this Point origin) => new Point(origin.X, origin.Y + 1);
        public static Point Left(this Point origin) => new Point(origin.X - 1, origin.Y);
        public static Point Right(this Point origin) => new Point(origin.X + 1, origin.Y);

        public static string ToString(this Color c)
        {
            return string.Format("[R:{0};G:{1};B:{2}]", c.R, c.G, c.B);
        }

        public static bool IsPath(this BoolMatrix matrix, Point p)
        {
            return IsPath(matrix, p.Y, p.X);
        }

        public static bool IsPath(this BoolMatrix matrix, int row, int col)
        {
            return !IsWall(matrix, col, row);
        }

        public static bool IsWall(this BoolMatrix matrix, Point p)
        {
            return IsWall(matrix, p.X, p.Y);
        }

        public static bool IsWall(this BoolMatrix matrix, int col, int row)
        {
            if (col < 0 || col >= matrix.Cols || row < 0 || row >= matrix.Rows)
                return true;

            //True is white color, so no wall
            return !matrix.Data[row, col];
        }

        public static void AddIfNotIn<T>(this List<T> enumerable, T item)
        {
            if (!enumerable.Contains(item))
                enumerable.Add(item);
        }

        public static bool ToBool(this Color color)
        {
            return (color.R == 255) && (color.G == 255) && (color.B == 255);
        }

        public static Bitmap ResizeImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
