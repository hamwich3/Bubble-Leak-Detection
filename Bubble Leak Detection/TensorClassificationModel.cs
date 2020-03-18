using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using TensorFlow;

namespace Bubble_Leak_Detection
{
    public class TensorClassificationModel
    {
        //paths for models/images
        private string AssetPath;
        private string TensorFlowModelFilePath;
        private string TensorFlowLabelsFilePath;

        byte[] model;
        string[] labels;
        TFGraph graph;
        TFSession session;

        Stopwatch sw = new Stopwatch();

        public TensorClassificationModel()
        {

        }

        public void LoadModel(string assetpath)
        {
            AssetPath = assetpath;
            TensorFlowModelFilePath = assetpath;
            TensorFlowLabelsFilePath = Path.Combine(Path.GetDirectoryName(assetpath), "labels.txt");
            if (!File.Exists(TensorFlowModelFilePath) || !File.Exists(TensorFlowLabelsFilePath))
            {
                System.Windows.Forms.MessageBox.Show("Files in " + AssetPath + " missing!");
                Environment.Exit(0);
            }
            graph = new TFGraph();
            model = File.ReadAllBytes(TensorFlowModelFilePath);
            labels = File.ReadAllLines(TensorFlowLabelsFilePath);
            graph.Import(model);
            session = new TFSession(graph);
            // First run takes the longest, so run it now
            InitializeModel();
        }

        public void InitializeModel()
        {
            Bitmap tempBmp = new Bitmap(256, 256);
            ClassifyImage(tempBmp);
            tempBmp.Dispose();
        }

        public double ClassifyImage(string file)
        {
            sw.Restart();
            var tensor = CreateTensorFromImageFile(file);
            return ClassifyTensor(tensor);
        }

        public double ClassifyImage(Image img)
        {
            sw.Restart();
            var tensor = CreateTensorFromBitmap(img);
            return ClassifyTensor(tensor);
        }

        private double ClassifyTensor(TFTensor tensor)
        {
            var bestIdx = 0;
            float best = 0;

            var runner = session.GetRunner();
            runner.AddInput(graph["Placeholder"][0], tensor).Fetch(graph["loss"][0]);
            var output = runner.Run();

            var result = output[0];
            
            var probabilities = ((float[][])result.GetValue(jagged: true))[0];
            for (int i = 0; i < probabilities.Length; i++)
            {
                if (probabilities[i] > best)
                {
                    bestIdx = i;
                    best = probabilities[i];
                }
                //Console.WriteLine($"{labels[i]} ({probabilities[i] * 100.0}%)");
            }

            string percent = (best * 100.0).ToString("N2");
            return probabilities[0];
        }

        // Convert the image in filename to a Tensor suitable as input to the Inception model.
        private static TFTensor CreateTensorFromImageFile(string file, TFDataType destinationDataType = TFDataType.Float)
        {
            var contents = File.ReadAllBytes(file);

            // DecodeJpeg uses a scalar String-valued tensor as input.
            var tensor = TFTensor.CreateString(contents);

            // Construct a graph to normalize the image
            using (var graph = ConstructGraphToNormalizeImage(out TFOutput input, out TFOutput output, destinationDataType))
            {
                // Execute that graph to normalize this one image
                using (var session = new TFSession(graph))
                {
                    var normalized = session.Run(
                        inputs: new[] { input },
                        inputValues: new[] { tensor },
                        outputs: new[] { output });

                    return normalized[0];
                }
            }
        }

        private static TFTensor CreateTensorFromBitmap(Image bitmap, TFDataType destinationDataType = TFDataType.Float)
        {
            //var contents = File.ReadAllBytes(file);
            var contents = ImageUtils.ImageToByte(bitmap);

            // DecodeJpeg uses a scalar String-valued tensor as input.
            var tensor = TFTensor.CreateString(contents);

            // Construct a graph to normalize the image
            using (var graph = ConstructGraphToNormalizeImage(out TFOutput input, out TFOutput output, destinationDataType))
            {
                // Execute that graph to normalize this one image
                using (var session = new TFSession(graph))
                {
                    var normalized = session.Run(
                        inputs: new[] { input },
                        inputValues: new[] { tensor },
                        outputs: new[] { output });

                    return normalized[0];
                }
            }
        }
        // Additional pointers for using TensorFlow & CustomVision together
        // Python: https://github.com/tensorflow/tensorflow/blob/master/tensorflow/examples/label_image/label_image.py
        // C++: https://github.com/tensorflow/tensorflow/blob/master/tensorflow/examples/label_image/main.cc
        // Java: https://github.com/Azure-Samples/cognitive-services-android-customvision-sample/blob/master/app/src/main/java/demo/tensorflow/org/customvision_sample/MSCognitiveServicesClassifier.java
        private static TFGraph ConstructGraphToNormalizeImage(out TFOutput input, out TFOutput output, TFDataType destinationDataType = TFDataType.Float)
        {
            const int W = 224;
            const int H = 224;
            const float Scale = 1;

            // Depending on your CustomVision.ai Domain - set appropriate Mean Values (RGB)
            // https://github.com/Azure-Samples/cognitive-services-android-customvision-sample for RGB values (in BGR order)
            //var bgrValues = new TFTensor(new float[] { 104.0f, 117.0f, 123.0f }); // General (Compact) & Landmark (Compact)
            //var bgrValues = new TFTensor(0f); // Retail (Compact)

            var graph = new TFGraph();
            input = graph.Placeholder(TFDataType.String);

            var caster = graph.Cast(graph.DecodePng(contents: input, channels: 3), DstT: TFDataType.Float);
            var dims_expander = graph.ExpandDims(caster, graph.Const(0, "batch"));
            var resized = graph.ResizeBilinear(dims_expander, graph.Const(new int[] { H, W }, "size"));
            //var resized_mean = graph.Sub(resized, graph.Const(bgrValues, "mean"));
            var normalised = graph.Div(resized, graph.Const(Scale));
            output = normalised;
            return graph;
        }
    }
}
