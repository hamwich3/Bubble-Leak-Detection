using System;
using System.Diagnostics;
using System.Drawing;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace Bubble_Leak_Detection
{
    public static class Sectors
    {
        public static Bitmap ROI1;
        public static Bitmap ROI2;
        public static Bitmap ROI3;
        public static Bitmap ROI4;
        public static Bitmap ROI5;
        public static Bitmap ROI6;
        public static Bitmap ROI7;
        public static Bitmap ROI8;

        public static Bitmap ROI1Diff;
        public static Bitmap ROI2Diff;
        public static Bitmap ROI3Diff;
        public static Bitmap ROI4Diff;
        public static Bitmap ROI5Diff;
        public static Bitmap ROI6Diff;
        public static Bitmap ROI7Diff;
        public static Bitmap ROI8Diff;

        public static Bitmap ROI1DiffThresh;
        public static Bitmap ROI2DiffThresh;
        public static Bitmap ROI3DiffThresh;
        public static Bitmap ROI4DiffThresh;
        public static Bitmap ROI5DiffThresh;
        public static Bitmap ROI6DiffThresh;
        public static Bitmap ROI7DiffThresh;
        public static Bitmap ROI8DiffThresh;

        public static Bitmap ROI1DropDiff;
        public static Bitmap ROI2DropDiff;
        public static Bitmap ROI3DropDiff;
        public static Bitmap ROI4DropDiff;
        public static Bitmap ROI5DropDiff;
        public static Bitmap ROI6DropDiff;
        public static Bitmap ROI7DropDiff;
        public static Bitmap ROI8DropDiff;

        public static Bitmap Quadrant1;
        public static Bitmap Quadrant2;
        public static Bitmap Quadrant3;
        public static Bitmap Quadrant4;
        public static Bitmap Quadrant5;
        public static Bitmap Quadrant6;
        public static Bitmap Quadrant7;
        public static Bitmap Quadrant8;

        public static Rectangle Region1;
        public static Rectangle Region2;
        public static Rectangle Region3;
        public static Rectangle Region4;
        public static Rectangle Region5;
        public static Rectangle Region6;
        public static Rectangle Region7;
        public static Rectangle Region8;

        public static int RegionRadius = 150;

        public static Rectangle Quadrant1Rect;
        public static Rectangle Quadrant2Rect;
        public static Rectangle Quadrant3Rect;
        public static Rectangle Quadrant4Rect;
        public static Rectangle Quadrant5Rect;
        public static Rectangle Quadrant6Rect;
        public static Rectangle Quadrant7Rect;
        public static Rectangle Quadrant8Rect;


        private static int regionWidth = 320;
        private static int regionHeight = 320;
        public static Size RegionSize = new Size(regionWidth, regionHeight);
        public static Size ImageSize = new Size(1440, 1080);

        //public static object _lock = new object();
        public static object _lock1 = new object();
        public static object _lock2 = new object();

        static Difference diffFilter1 = new Difference();
        static Difference diffFilter2 = new Difference();


        //public static Threshold threshFilter1 = new Threshold(70);
        //public static Threshold threshFilter2 = new Threshold(70);

        //20, 200
        public static IterativeThreshold threshFilter1 = new IterativeThreshold(30, 150);
        public static IterativeThreshold threshFilter2 = new IterativeThreshold(30, 150);

        static Invert invertFilter1 = new Invert();
        static Invert invertFilter2 = new Invert();

        static Subtract subFilter1 = new Subtract();
        static Subtract subFilter2 = new Subtract();

        static Subtract circleFilter1 = new Subtract();
        static Subtract circleFilter2 = new Subtract();

        static Grayscale greyscaleFilter1 = new Grayscale(.299, .587, .114);
        static Grayscale greyscaleFilter2 = new Grayscale(.299, .587, .114);
        static Crop cropFilter1 = new Crop(new Rectangle(10, 10, 10, 10));
        static Crop cropFilter2 = new Crop(new Rectangle(10, 10, 10, 10));
        static ResizeBilinear resizeFilter1 = new ResizeBilinear(320, 320);
        static ResizeBilinear resizeFilter2 = new ResizeBilinear(320, 320);

        static BrightnessCorrection brightFilter1 = new BrightnessCorrection(-25);
        static BrightnessCorrection brightFilter2 = new BrightnessCorrection(-25);

        static BlobCounter blobFilter1 = new BlobCounter();
        static BlobCounter blobFilter2 = new BlobCounter();

        static ContrastStretch normalize1 = new ContrastStretch();
        static ContrastStretch normalize2 = new ContrastStretch();

        public static void Initialize()
        {
            double halfWidth = ImageSize.Width / 2;
            double halfHeight = ImageSize.Height / 2;

            SetCircleFilterBackgroungImages();

            Quadrant1Rect = new Rectangle(0, 0, (int)halfWidth, (int)halfHeight);
            Quadrant3Rect = new Rectangle(0, 0, (int)halfWidth, (int)halfHeight);
            Quadrant2Rect = new Rectangle((int)halfWidth, 0, (int)halfWidth, (int)halfHeight);
            Quadrant4Rect = new Rectangle((int)halfWidth, 0, (int)halfWidth, (int)halfHeight);
            Quadrant5Rect = new Rectangle(0, (int)halfHeight, (int)halfWidth, (int)halfHeight);
            Quadrant7Rect = new Rectangle(0, (int)halfHeight, (int)halfWidth, (int)halfHeight);
            Quadrant6Rect = new Rectangle((int)halfWidth, (int)halfHeight, (int)halfWidth, (int)halfHeight);
            Quadrant8Rect = new Rectangle((int)halfWidth, (int)halfHeight, (int)halfWidth, (int)halfHeight);

            float x = (ImageSize.Width / 4) - (RegionSize.Width / 2);
            float y = (ImageSize.Height / 4) - (RegionSize.Height / 2);
            Region1 = new Rectangle((int)x, (int)y, RegionSize.Width, RegionSize.Height);
            Region3 = new Rectangle((int)x, (int)y, RegionSize.Width, RegionSize.Height);

            x = ((3 * ImageSize.Width / 4)) - (RegionSize.Width / 2);
            y = (ImageSize.Height / 4) - (RegionSize.Height / 2);
            Region2 = new Rectangle((int)x, (int)y, RegionSize.Width, RegionSize.Height);
            Region4 = new Rectangle((int)x, (int)y, RegionSize.Width, RegionSize.Height);

            x = (ImageSize.Width / 4) - (RegionSize.Width / 2);
            y = ((3 * ImageSize.Height / 4)) - (RegionSize.Height / 2);
            Region5 = new Rectangle((int)x, (int)y, RegionSize.Width, RegionSize.Height);
            Region7 = new Rectangle((int)x, (int)y, RegionSize.Width, RegionSize.Height);

            x = ((3 * ImageSize.Width / 4)) - (RegionSize.Width / 2);
            y = ((3 * ImageSize.Height / 4)) - (RegionSize.Height / 2);
            Region6 = new Rectangle((int)x, (int)y, RegionSize.Width, RegionSize.Height);
            Region8 = new Rectangle((int)x, (int)y, RegionSize.Width, RegionSize.Height);
        }

        private static void setBitmap(ref Bitmap bmp, Bitmap value)
        {
            if (value == null)
            {
                bmp = null;
                return;
            }
            var tmp = bmp;
            bmp = value;
            //bmp = ImageUtils.ColorToGrayscale(value);
            //bmp = greyscaleFilter1.Apply(value);
            tmp?.Dispose();
            tmp = null;
            value?.Dispose();
            value = null;
        }

        public static void SetCircleFilterBackgroungImages()
        {
            Bitmap circle = new Bitmap(RegionSize.Width, RegionSize.Height);
            using (var g = Graphics.FromImage(circle))
            {
                SolidBrush whiteBrush = new SolidBrush(Color.FromArgb(255, 255, 255));
                SolidBrush blackBrush = new SolidBrush(Color.FromArgb(0, 0, 0));
                Pen blackPen = new Pen(Color.FromArgb(0, 0, 0));
                g.FillRectangle(whiteBrush, 0, 0, circle.Width, circle.Height);
                int xy = (int)(circle.Width * 0.5) - RegionRadius;
                g.FillEllipse(blackBrush, xy, xy, RegionRadius * 2, RegionRadius * 2);
                circleFilter1.OverlayImage = greyscaleFilter1.Apply(circle);
                circleFilter2.OverlayImage = greyscaleFilter1.Apply(circle);
            }
        }

        public static void ExtractRegionImages1(Bitmap frame1, Bitmap frame2)
        {
            frame1 = greyscaleFilter1.Apply(frame1);
            frame2 = greyscaleFilter1.Apply(frame2);
            lock (_lock1)
            {
                cropFilter1.Rectangle = Region1;
                ROI1 = circleFilter2.Apply(resizeFilter1.Apply(cropFilter1.Apply(frame1)));
                diffFilter1.OverlayImage = circleFilter2.Apply(resizeFilter1.Apply(cropFilter1.Apply(frame2)));
                ROI1Diff = diffFilter1.Apply(ROI1);
                subFilter1.OverlayImage = invertFilter1.Apply(threshFilter1.Apply(ROI1Diff));
                ROI1DropDiff = subFilter1.Apply(ROI1Diff);

                cropFilter1.Rectangle = Region2;
                ROI2 = circleFilter2.Apply(resizeFilter1.Apply(cropFilter1.Apply(frame1)));
                diffFilter1.OverlayImage = circleFilter2.Apply(resizeFilter1.Apply(cropFilter1.Apply(frame2)));
                ROI2Diff = diffFilter1.Apply(ROI2);
                subFilter1.OverlayImage = invertFilter1.Apply(threshFilter1.Apply(ROI2Diff));
                ROI2DropDiff = subFilter1.Apply(ROI2Diff);

                cropFilter1.Rectangle = Region5;
                ROI5 = circleFilter2.Apply(resizeFilter1.Apply(cropFilter1.Apply(frame1)));
                diffFilter1.OverlayImage = circleFilter2.Apply(resizeFilter1.Apply(cropFilter1.Apply(frame2)));
                ROI5Diff = diffFilter1.Apply(ROI5);
                subFilter1.OverlayImage = invertFilter1.Apply(threshFilter1.Apply(ROI5Diff));
                ROI5DropDiff = subFilter1.Apply(ROI5Diff);

                cropFilter1.Rectangle = Region6;
                ROI6 = circleFilter2.Apply(resizeFilter1.Apply(cropFilter1.Apply(frame1)));
                diffFilter1.OverlayImage = circleFilter2.Apply(resizeFilter1.Apply(cropFilter1.Apply(frame2)));
                ROI6Diff = diffFilter1.Apply(ROI6);
                subFilter1.OverlayImage = invertFilter1.Apply(threshFilter1.Apply(ROI6Diff));
                ROI6DropDiff = subFilter1.Apply(ROI6Diff);
            }
        }

        public static void ExtractRegionsDevice2(Bitmap frame1, Bitmap frame2)
        {
            frame1 = greyscaleFilter2.Apply(frame1);
            frame2 = greyscaleFilter2.Apply(frame2);
            lock (_lock2)
            {
                cropFilter2.Rectangle = Region3;
                ROI3 = circleFilter2.Apply(resizeFilter2.Apply(cropFilter2.Apply(frame1)));
                diffFilter2.OverlayImage = circleFilter2.Apply(resizeFilter2.Apply(cropFilter2.Apply(frame2)));
                ROI3Diff = diffFilter2.Apply(ROI3);
                subFilter2.OverlayImage = invertFilter2.Apply(threshFilter2.Apply(ROI3Diff));
                ROI3DropDiff = subFilter2.Apply(ROI3Diff);

                cropFilter2.Rectangle = Region4;
                ROI4 = circleFilter2.Apply(resizeFilter2.Apply(cropFilter2.Apply(frame1)));
                diffFilter2.OverlayImage = circleFilter2.Apply(resizeFilter2.Apply(cropFilter2.Apply(frame2)));
                ROI4Diff = diffFilter2.Apply(ROI4);
                subFilter2.OverlayImage = invertFilter2.Apply(threshFilter2.Apply(ROI4Diff));
                ROI4DropDiff = subFilter2.Apply(ROI4Diff);

                cropFilter2.Rectangle = Region7;
                ROI7 = circleFilter2.Apply(resizeFilter2.Apply(cropFilter2.Apply(frame1)));
                diffFilter2.OverlayImage = circleFilter2.Apply(resizeFilter2.Apply(cropFilter2.Apply(frame2)));
                ROI7Diff = diffFilter2.Apply(ROI7);
                subFilter2.OverlayImage = invertFilter2.Apply(threshFilter2.Apply(ROI7Diff));
                ROI7DropDiff = subFilter2.Apply(ROI7Diff);

                cropFilter2.Rectangle = Region8;
                ROI8 = circleFilter2.Apply(resizeFilter2.Apply(cropFilter2.Apply(frame1)));
                diffFilter2.OverlayImage = circleFilter2.Apply(resizeFilter2.Apply(cropFilter2.Apply(frame2)));
                ROI8Diff = diffFilter2.Apply(ROI8);
                subFilter2.OverlayImage = invertFilter2.Apply(threshFilter2.Apply(ROI8Diff));
                ROI8DropDiff = subFilter2.Apply(ROI8Diff);
            }
        }

        public static void ExtractSectorsDevice1(Bitmap bmp1)
        {
            lock (_lock1)
            {
                bmp1 = greyscaleFilter1.Apply(bmp1);

                cropFilter1.Rectangle = Quadrant1Rect;
                Quadrant1 = cropFilter1.Apply(bmp1);

                cropFilter1.Rectangle = Quadrant2Rect;
                Quadrant2 = cropFilter1.Apply(bmp1);

                cropFilter1.Rectangle = Quadrant5Rect;
                Quadrant5 = cropFilter1.Apply(bmp1);

                cropFilter1.Rectangle = Quadrant6Rect;
                Quadrant6 = cropFilter1.Apply(bmp1);
            }
        }

        public static void ExtractSectorsDevice2(Bitmap bmp2)
        {
            lock (_lock2)
            {
                bmp2 = greyscaleFilter1.Apply(bmp2);

                cropFilter1.Rectangle = Quadrant3Rect;
                Quadrant3 = cropFilter1.Apply(bmp2);

                cropFilter1.Rectangle = Quadrant4Rect;
                Quadrant4 = cropFilter1.Apply(bmp2);

                cropFilter1.Rectangle = Quadrant7Rect;
                Quadrant7 = cropFilter1.Apply(bmp2);

                cropFilter1.Rectangle = Quadrant8Rect;
                Quadrant8 = cropFilter1.Apply(bmp2);
            }
        }
    }
}
