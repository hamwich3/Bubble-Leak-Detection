using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubble_Leak_Detection
{
    static class ImageUtils
    {

        public static Bitmap Crop(Bitmap bmp, Rectangle crop)
        {
            Bitmap cropped = new Bitmap(crop.Width, crop.Height);
            using (Graphics g = Graphics.FromImage(cropped))
            {
                g.DrawImage(bmp, new Rectangle(0, 0, crop.Width, crop.Height), crop, GraphicsUnit.Pixel);
            }
            return cropped;
        }

        public static Bitmap Resize(Bitmap bmp, float scale)
        {
            float x = bmp.Width * scale;
            float y = bmp.Height * scale;
            Bitmap resized = new Bitmap((int)x, (int)y);
            using (Graphics g = Graphics.FromImage(resized))
            {
                g.DrawImage(bmp, new Rectangle(0, 0, resized.Width, resized.Height), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
            }
            return resized;
        }

        public static Bitmap Resize256(Bitmap bmp)
        {
            Bitmap resized = new Bitmap(256, 256);
            using (Graphics g = Graphics.FromImage(resized))
            {
                g.DrawImage(bmp, new Rectangle(0, 0, resized.Width, resized.Height), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
            }
            return resized;
        }

        public static byte[] ImageToByte(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

    }
}
