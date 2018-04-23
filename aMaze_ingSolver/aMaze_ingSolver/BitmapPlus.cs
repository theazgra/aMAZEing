using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace aMaze_ingSolver
{
    unsafe class BitmapPlus : IDisposable
    {
        public Bitmap Source { get; private set; }
        public BitmapData Data { get; private set; }
        int _pixelCount;
        IntPtr imagePointer = IntPtr.Zero;

        public byte[] Pixels { get; set; }
        private int _depth;
        private int _colorStep;
        private ImageLockMode _imgLockMode;

        public int Cols { get; }
        public int Rows { get; }



        public BitmapPlus(Bitmap bitmap, ImageLockMode lockMode)
        {
            Source = bitmap;
            Cols = Source.Width;
            Rows = Source.Height;

            _pixelCount = Source.Width * Source.Height;
            _imgLockMode = lockMode;

            Rectangle lockRectangle = new Rectangle(0, 0, Source.Width, Source.Height);

            _depth = Image.GetPixelFormatSize(Source.PixelFormat);

            if (_depth != 8 && _depth != 24 && _depth != 32)
            {
                throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
            }

            Data = Source.LockBits(lockRectangle, _imgLockMode, Source.PixelFormat);

            _colorStep = _depth / 8;
            Pixels = new byte[_pixelCount * _colorStep];
            imagePointer = Data.Scan0;

            // Copy data from pointer to array
            Marshal.Copy(imagePointer, Pixels, 0, Pixels.Length);
        }

        public Color GetPixel(int x, int y)
        {
            Color clr = Color.Empty;
            // Get start index of the specified pixel
            int i = ((y * Source.Width) + x) * _colorStep;

            if (i > Pixels.Length - _colorStep)
                throw new IndexOutOfRangeException();

            if (_depth == 32) // For 32 bpp get Red, Green, Blue and Alpha
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                byte a = Pixels[i + 3]; // a
                clr = Color.FromArgb(a, r, g, b);
            }
            else if (_depth == 24) // For 24 bpp get Red, Green and Blue
            {
                byte b = Pixels[i];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                clr = Color.FromArgb(r, g, b);
            }
            else if (_depth == 8) // For 8 bpp get color value (Red, Green and Blue values are the same)
            {
                byte c = Pixels[i];
                clr = Color.FromArgb(c, c, c);
            }
            return clr;
        }

        public void SetPixel(int x, int y, Color color)
        {
            // Get start index of the specified pixel
            int i = ((y * Source.Width) + x) * _colorStep;

            if (_depth == 32) // For 32 bpp set Red, Green, Blue and Alpha
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
                Pixels[i + 3] = color.A;
            }
            else if (_depth == 24) // For 24 bpp set Red, Green and Blue
            {
                Pixels[i] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
            }
            else if (_depth == 8) // For 8 bpp set color value (Red, Green and Blue values are the same)
            {
                Pixels[i] = color.B;
            }
        }

        public void Dispose()
        {
            try
            {
                Marshal.Copy(Pixels, 0, imagePointer, Pixels.Length);
                Source.UnlockBits(Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
