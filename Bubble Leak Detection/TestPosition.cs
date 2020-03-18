using System;
using System.Drawing;
using System.Linq;

namespace Bubble_Leak_Detection
{
    class TestPosition
    {

        public string DisplayText { get; set; } = "";
        public string SerialText { get; set; } = "";
        public string LotNoText { get; set; } = "";

        public double LeakObjectProbability
        {
            get { return _objectProb; }
            set
            {
                _objectProb = value;
                LeakLocated = value > ObjectDetectionThreshold;
                if (LeakLocated)
                {
                    if (TestActive) LeakLocatedCount++;
                    //Maybe = LeakLocatedCount > MaybeThreshold;
                    //Fail = LeakLocatedCount > FailThresh;
                }
            }
        }

        public double LeakClassificationProbability
        {
            get { return _classProb; }
            set
            {
                _classProb = value;
                LeakVerified = value > ClassificationThreshold;
                if (LeakVerified)
                {
                    if (TestActive) LeakVerifiedCount++;
                    Maybe = LeakVerifiedCount > MaybeThreshold;
                    Fail = LeakVerifiedCount >= FailThresh;
                }
            }
        }

        public double ObjectDetectionThreshold = 0.35;
        public double ClassificationThreshold = 0.4;
        public bool LeakLocated = false;
        public bool LeakVerified = false;
        public double LeakTotalProbability = 0;
        public bool TestActive = false;
        public int LeakLocatedCount = 0;
        public int LeakVerifiedCount = 0;
        public int FailThresh = 10;
        public int MaybeThreshold = 5;
        public bool Maybe = false;
        public bool Fail = false;

        public Rectangle ROI;
        public Rectangle Sector;

        private double _objectProb;
        private double _classProb;

        public Rectangle[] detections;
        public Bitmap[] detectionImages = Enumerable.Repeat<Bitmap>(new Bitmap(256, 256), 5).ToArray();

        public Rectangle maxProbRect;

        public TestPosition()
        {

        }

        public void GetDetectionImages(Bitmap bmp)
        {
            for (int i = 0; i < 5; i++)
            {
                var tmp = detectionImages?[i];
                if (detections[0] != null && bmp != null) detectionImages[i] = ImageUtils.Crop(bmp, detections[i]);
                if (tmp != null)
                {
                    tmp.Dispose();
                    tmp = null;
                }
            }
        }

        public void Reset()
        {
            LeakLocatedCount = 0;
            LeakVerifiedCount = 0;
            Maybe = false;
            Fail = false;
        }

    }
}
