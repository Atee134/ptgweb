using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ptg.Common
{
    public static class BitmapHelper
    {
        public static byte[] WriteToByteArray(byte[,] heightMapdata)
        {
            byte[] content;
            int width = heightMapdata.GetLength(0);
            int height = heightMapdata.GetLength(1);
            using (var stream = new MemoryStream())
            {
                var bitmap = new Bitmap(width, height); // TODO put this back here PixelFormat.Format16bppGrayScale);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var value = heightMapdata[x, y];
                        bitmap.SetPixel(x, y, Color.FromArgb(value, value, value)); // TODO with 16bit grayscale format this throws exception, use lockbits instead of setpixel anyway
                    }
                }

                bitmap.Save(stream, ImageFormat.Bmp);
                content = stream.ToArray();
            }

            return content;
        }

        public static byte[] WriteToByteArray(Color[,] heightMapdata)
        {
            byte[] content;
            int width = heightMapdata.GetLength(0);
            int height = heightMapdata.GetLength(1);
            using (var stream = new MemoryStream())
            {
                var bitmap = new Bitmap(width, height); // TODO put this back here PixelFormat.Format16bppGrayScale);

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        bitmap.SetPixel(x, y, heightMapdata[x, y]); // TODO with 16bit grayscale format this throws exception, use lockbits instead of setpixel anyway
                    }
                }

                bitmap.Save(stream, ImageFormat.Bmp);
                content = stream.ToArray();
            }

            return content;
        }
    }
}
