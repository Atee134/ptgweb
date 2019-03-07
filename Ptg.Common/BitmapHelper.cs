using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Ptg.Common
{
    public static class BitmapHelper
    {
        private static byte[,] ConverToByteArray(float[,] heightmapFloatData)
        {
            byte[,] heightmapByteData = new byte[heightmapFloatData.GetLength(0), heightmapFloatData.GetLength(1)];

            for (int x = 0; x < heightmapByteData.GetLength(0); x++)
            {
                for (int y = 0; y < heightmapByteData.GetLength(1); y++)
                {
                    heightmapByteData[x, y] = (byte)heightmapFloatData[x, y];
                }
            }

            return heightmapByteData;
        }

        public static byte[] WriteToByteArray(float[,] heightmapFloatData)
        {
            return WriteToByteArray(ConverToByteArray(heightmapFloatData));
        }

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

                bitmap.Save(stream, ImageFormat.Png);
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

                bitmap.Save(stream, ImageFormat.Png);
                content = stream.ToArray();
            }

            return content;
        }
    }
}
