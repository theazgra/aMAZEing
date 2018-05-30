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
    /// <summary>
    /// All extensions in one place, bad idea I know...
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Check if given location is present.
        /// </summary>
        /// <param name="vertices">Vertices.</param>
        /// <param name="location">Location.</param>
        /// <returns>True if location is present.</returns>
        public static bool ContainsLocation(this IEnumerable<Vertex> vertices, Point location)
        {
            Vertex found = vertices.FirstOrDefault(n => n.X == location.X && n.Y == location.Y);

            return (found != null);
        }

        /// <summary>
        /// Move in given direction.
        /// </summary>
        /// <param name="origin">Origin point.</param>
        /// <param name="direction">Where to move.</param>
        /// <returns>New point where we moved.</returns>
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

        
        /// <summary>
        /// Move up.
        /// </summary>
        /// <param name="origin">Origin point.</param>
        /// <returns>New point.</returns>
        public static Point Up(this Point origin) => new Point(origin.X, origin.Y - 1);
        /// <summary>
        /// Move down.
        /// </summary>
        /// <param name="origin">Origin point.</param>
        /// <returns>New point.</returns>
        public static Point Down(this Point origin) => new Point(origin.X, origin.Y + 1);
        /// <summary>
        /// Move left.
        /// </summary>
        /// <param name="origin">Origin point.</param>
        /// <returns>New point.</returns>
        public static Point Left(this Point origin) => new Point(origin.X - 1, origin.Y);
        /// <summary>
        /// Move right.
        /// </summary>
        /// <param name="origin">Origin point.</param>
        /// <returns>New point.</returns>
        public static Point Right(this Point origin) => new Point(origin.X + 1, origin.Y);

        /// <summary>
        /// Color to string.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToString(this Color c)
        {
            return string.Format("[R:{0};G:{1};B:{2}]", c.R, c.G, c.B);
        }

        /// <summary>
        /// Check if there is path on row.
        /// </summary>
        /// <param name="matrix">Maze data.</param>
        /// <param name="row">Row where to move.</param>
        /// <param name="colFrom">From where to move.</param>
        /// <param name="colTo">Where to finish.</param>
        /// <returns>True if there is path.</returns>
        public static bool FreeXPath(this BoolMatrix matrix, int row, int colFrom, int colTo)
        {
            for (int col = colFrom; col < colTo; col++)
            {
                if (matrix.IsWall(col, row))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check if there is path on column.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="col">Column on which to move.</param>
        /// <param name="rowFrom">From where to move.</param>
        /// <param name="rowTo">Where to finish.</param>
        /// <returns>True if there is path</returns>
        public static bool FreeYPath(this BoolMatrix matrix, int col, int rowFrom, int rowTo)
        {
            for (int row = rowFrom; row < rowTo; row++)
            {
                if (matrix.IsWall(col, row))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Check if this position is path.
        /// </summary>
        /// <param name="matrix">Maze data.</param>
        /// <param name="p">Location.</param>
        /// <returns>True if location is path.</returns>
        public static bool IsPath(this BoolMatrix matrix, Point p)
        {
            return IsPath(matrix, p.Y, p.X);
        }

        /// <summary>
        /// Check if this position is path.
        /// </summary>
        /// <param name="matrix">Maze data.</param>
        /// <param name="row">Y</param>
        /// <param name="col">X</param>
        /// <returns>True if location is path.</returns>
        public static bool IsPath(this BoolMatrix matrix, int row, int col)
        {
            return !IsWall(matrix, col, row);
        }

        /// <summary>
        /// Check if this position is wall.
        /// </summary>
        /// <param name="matrix">Maze data.</param>
        /// <param name="p">Location.</param>
        /// <returns>True if location is wall.</returns>
        public static bool IsWall(this BoolMatrix matrix, Point p)
        {
            return IsWall(matrix, p.X, p.Y);
        }

        /// <summary>
        /// Check if this position is wall.
        /// </summary>
        /// <param name="matrix">Maze data.</param>
        /// <param name="row">Y</param>
        /// <param name="col">X</param>
        /// <returns>True if location is wall.</returns>
        public static bool IsWall(this BoolMatrix matrix, int col, int row)
        {
            if (col < 0 || col >= matrix.Cols || row < 0 || row >= matrix.Rows)
                return true;

            //True is white color, so no wall
            return !matrix.Data[row, col];
        }

        /// <summary>
        /// Add item into list if is not present.
        /// </summary>
        /// <typeparam name="T">List item type.</typeparam>
        /// <param name="list">List where to add.</param>
        /// <param name="item">Item to add if is missing.</param>
        public static void AddIfNotIn<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
                list.Add(item);
        }

        /// <summary>
        /// Convert color to bool, 
        /// </summary>
        /// <param name="color"></param>
        /// <returns>Truw if white.</returns>
        public static bool ToBool(this Color color)
        {
            return (color.R == 255) && (color.G == 255) && (color.B == 255);
        }

        /// <summary>
        /// Resize image.
        /// </summary>
        /// <param name="image">Image to resize.</param>
        /// <param name="width">New width.</param>
        /// <param name="height">New height.</param>
        /// <param name="interpolation">Resize interpolation</param>
        /// <returns>Resized image.</returns>
        public static Bitmap ResizeImage
            (this Image image, int width, int height, InterpolationMode interpolation = InterpolationMode.NearestNeighbor)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;

                graphics.InterpolationMode = interpolation;

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

        /// <summary>
        /// Dostance from origin to destination in path.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="destination">Destination.</param>
        /// <returns>Path length.</returns>
        public static int PathDistanceTo(this Vertex origin, Vertex destination)
        {
            switch (Utils.GetDirection(origin.Location, destination.Location))
            {
                case Direction.Up:
                    return (origin.Location.Y - destination.Location.Y);
                case Direction.Down:
                    return -(origin.Location.Y - destination.Location.Y);
                case Direction.Left:
                    return (origin.Location.X - destination.Location.X);
                case Direction.Right:
                    return -(origin.Location.X - destination.Location.X);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Get euclidean distance between two vertices.
        /// </summary>
        /// <param name="origin">Origin vertex.</param>
        /// <param name="destination">Destination vertex.</param>
        /// <returns>Distance between those two vertices.</returns>
        public static float EuclideanDistanceTo(this Vertex origin, Vertex destination)
        {
            Point o = origin.Location;
            Point d = destination.Location;

            double root = Math.Pow((o.X - d.X), 2) + Math.Pow((o.Y - d.Y), 2);

            float distance = (float)(Math.Sqrt(root));
            return distance;
        }

        /// <summary>
        /// Scale point.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="scale"></param>
        /// <returns>Scaled point.</returns>
        public static Point Scale(this Point point, float scale)
        {
            if (scale == 1.0f)
                return point;
            int newX = (int)(point.X * scale);
            int newY = (int)(point.Y * scale);
            return new Point(newX, newY);
        }

        /// <summary>
        /// Create edge between two vertices, but don't connect them.
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <returns>Virtual edge.</returns>
        public static OrientedEdge CreateVirtualEdgeTo(this Vertex origin, Vertex destination)
        {
            OrientedEdge virtualEdge = new OrientedEdge(origin, destination, Utils.GetDirection(origin.Location, destination.Location));
            return virtualEdge;
        }
    }
}
