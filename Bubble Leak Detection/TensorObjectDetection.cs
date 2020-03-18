﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorFlow;
using NumSharp;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace Bubble_Leak_Detection
{
    public class TensorObjectDetection
    {
        /*===============================
         * 
         * Loads a Tensorflow model generated by Microsoft Azure Custom Vision Service
         * object detection, general (compact) domain, export TensorFlow => TensorFlow
         * 
         * Platform target must be x64 or you get BadImageFormatException
         * 
         * Processor must support AVX instructions or you get (0xc000001d) 'Illegal Instruction'
         * 
         * An exception will be thrown loading the model with the tensorflow library included with TensorFlowSharp 1.13.0
         * Replace libtensorflow.dll (EatonBubbleDetect\packages\TensorFlowSharp.1.13.0\runtimes\win7-x64\native)
         * with tensorflow.dll v1.14.0 downloaded from: https://www.tensorflow.org/install/lang_c (Windows CPU only)
         * rename to "libtensorflow.dll"
         * 
         * The TensorFlow model comes in a .zip with 2 python scripts that perform object detection. The math to get the object information from the 
         * tensor comes from those scripts. 
         * 
         * TODO:
         * 
         *  Consider converting all floats to doubles.
         *  Every NDArray output by python script would have to be in doubles, or find a way to detect and convert here
         *  
         *  Add functions that put the images through the python scripts for comparison
         *  
         *  Change input tensor size to 256 x 256 for speed
         * 
         ===============================*/


        private string AssetPath;
        private string TensorFlowModelFilePath;
        private string TensorFlowLabelsFilePath;

        private byte[] model;
        private string[] labels;
        private TFGraph graph;
        static private TFSession session;

        private Stopwatch sw = new Stopwatch();

        public TensorObjectDetection()
        {

        }

        public void LoadModel(string assetpath, bool initialize = true)
        {
            LoadStringsCheckFiles(assetpath);
            graph = new TFGraph();
            model = File.ReadAllBytes(TensorFlowModelFilePath);
            labels = File.ReadAllLines(TensorFlowLabelsFilePath);
            graph.Import(model);
            session = new TFSession(graph);
            if (initialize) InitializeModel();
        }

        private void LoadStringsCheckFiles(string assetpath)
        {
            AssetPath = assetpath;
            TensorFlowModelFilePath = assetpath;
            TensorFlowLabelsFilePath = Path.Combine(Path.GetDirectoryName(assetpath), "labels.txt");
            if (!File.Exists(TensorFlowModelFilePath) || !File.Exists(TensorFlowLabelsFilePath))
            {
                System.Windows.Forms.MessageBox.Show("Files in " + AssetPath + " missing!");
                Environment.Exit(0);
            }
        }

        public void InitializeModel()
        {
            Bitmap tempBmp = new Bitmap(256, 256);
            Rectangle size = new Rectangle(10, 10, 10, 10);
            Rectangle[] tempRect;
            DetectObjects(tempBmp, size, out tempRect);
            tempBmp.Dispose();
        }

        public double[] DetectObjects(string file, Rectangle size, out Rectangle[] rect)
        {
            sw.Restart();
            TFTensor tensor = CreateTensorFromImageFile(file);
            return DetectObjectsFromTensor(tensor, size, out rect);
        }

        public double[] DetectObjects(string file)
        {
            sw.Restart();
            TFTensor tensor = CreateTensorFromImageFile(file);
            Rectangle[] rect;
            Rectangle size = new Rectangle(10, 10, 10, 10);
            return DetectObjectsFromTensor(tensor, size, out rect);
        }

        public double[] DetectObjects(Image img, Rectangle size, out Rectangle[] rect)
        {
            sw.Restart();
            TFTensor tensor = CreateTensorFromBitmap(img);
            return DetectObjectsFromTensor(tensor, size, out rect);
        }

        public double[] DetectObjects(Image img)
        {
            sw.Restart();
            TFTensor tensor = CreateTensorFromBitmap(img);
            Rectangle[] rect;
            Rectangle size = new Rectangle(10, 10, 10, 10);
            return DetectObjectsFromTensor(tensor, size, out rect);
        }

        private double[] DetectObjectsFromTensor(TFTensor tensor, Rectangle size, out Rectangle[] rect)
        {
            TFSession.Runner runner = session.GetRunner();
            runner.AddInput(graph["Placeholder"][0], tensor).Fetch(graph["model_outputs"][0]);
            TFTensor[] output = runner.Run();

            TFTensor result = output[0];
            float[][][] data = ((float[][][][])result.GetValue(jagged: true))[0];

            //NDArray is a NumSharp class allows us to perform operations on large arrays
            //Essentially a port of python's numpy for C#
            NDArray prediction_output = np.array(data);

            return GetHighestProbabilityBoundingBox(prediction_output, size, out rect);
        }

        private double[] GetHighestProbabilityBoundingBox(NDArray prediction_output, Rectangle size, out Rectangle[] box)
        {
            //Anchors for extracting information from tensor returned by model.
            //(copied from python scripts)
            NDArray anchors = np.array(new float[,] { { 0.573F, 0.677F }, { 1.87F, 2.06F }, { 3.34F, 5.47F }, { 7.88F, 3.53F }, { 9.77F, 9.17F } });

            int num_anchor = anchors.shape[0];
            int height = prediction_output.shape[0];
            int width = prediction_output.shape[1];
            int channels = prediction_output.shape[2];
            int num_class = (int)(channels / num_anchor) - 5;
            var p = ((prediction_output.size / height) / width) / num_anchor;
            int[] shape = { height, width, num_anchor, p };
            //{13, 13, 5, 6}
            NDArray outputs = prediction_output.reshape(shape);

            //Extract bounding box information
            NDArray x = logistic(outputs["..., 0"]);
            NDArray qx = np.arange(width)[Slice.NewAxis, Slice.All, Slice.NewAxis];
            x = np.add(x, qx) / width;

            NDArray y = logistic(outputs["..., 1"]);
            NDArray qy = np.arange(height)[Slice.All, Slice.NewAxis, Slice.NewAxis];
            y = np.add(y, qy) / height;

            NDArray w = np.exp(outputs["..., 2"]);
            NDArray qw = anchors[":, 0"][Slice.NewAxis, Slice.NewAxis, Slice.All];
            w = np.multiply(w, qw) / (Double)width;

            NDArray h = np.exp(outputs["..., 3"]);
            NDArray qh = anchors[":, 1"][Slice.NewAxis, Slice.NewAxis, Slice.All];
            h = np.multiply(h, qh) / (Double)height;

            //(x,y) in the network outputs is the center of the bounding box. Convert them to top-left
            x = np.subtract(x, w / 2);
            y = np.subtract(y, h / 2);
            NDArray boxes = np.stack(new NDArray[] { x, y, w, h }, axis: -1).reshape(-1, 4);

            //Get confidence for the bounding boxes
            NDArray boxConfidence = logistic(outputs["..., 4"]);

            //Get class probabilities for the bounding boxes
            NDArray rawProbability = outputs["..., 5:"];
            NDArray classProbabilities = np.exp(np.subtract(rawProbability, np.amax(rawProbability, axis: 3)[Slice.Ellipsis, Slice.NewAxis]));
            classProbabilities = np.multiply(classProbabilities / np.sum(classProbabilities, axis: 3)[Slice.Ellipsis, Slice.NewAxis], boxConfidence[Slice.Ellipsis, Slice.NewAxis]);
            classProbabilities = classProbabilities.reshape(-1, num_class);

            ////Read outputs generated by python scripts, used to test our numbers against
            //classProbabilities = np.fromfile(Path.Combine(PythonFolderPath, "class_probs"), NPTypeCode.Single).reshape(845, 1);
            //boxes = np.fromfile(Path.Combine(PythonFolderPath, "boxes"), NPTypeCode.Double).reshape(845, 4);

            var max_probs = np.amax(classProbabilities, axis: 1);
            var max_classes = np.argmax(classProbabilities, axis: 1);
            var maxProbsArray = max_probs.ToArray<Single>();
            bool classArrayIsInt32 = max_classes.dtype.ToString() == "System.Int32";
            int[] maxClassesArray;
            if (classArrayIsInt32) maxClassesArray = max_classes.ToArray<Int32>();
            else maxClassesArray = Enumerable.Repeat<int>(0, 2000).ToArray();
            float largestProbability = 0;
            int index1 = 0;
            int index2 = 0;
            int index3 = 0;
            int index4 = 0;
            int index5 = 0;
            
            for (int i = 0; i < maxProbsArray.Length; i++)
            {
                if (largestProbability < maxProbsArray[i] && (!classArrayIsInt32 || maxClassesArray[i] == 0))
                {
                    largestProbability = maxProbsArray[i];
                    index5 = index4;
                    index4 = index3;
                    index3 = index2;
                    index2 = index1;
                    index1 = i;
                }
            }


            int[] indexes = { index1, index2, index3, index4, index5 };
            double[] probs = { maxProbsArray[index1], maxProbsArray[index2], maxProbsArray[index3], maxProbsArray[index4], maxProbsArray[index5] };
            box = Enumerable.Repeat<Rectangle>(new Rectangle(), 5).ToArray();

            for (int i = 0; i < box.Length; i++)
            {
                var boundingBox1 = boxes[indexes[i]].ToArray<Double>();
                boundingBox1[0] = boundingBox1[0] * ((double)size.Width) + size.X;
                boundingBox1[1] = boundingBox1[1] * ((double)size.Height) + size.Y;
                boundingBox1[2] = boundingBox1[2] * ((double)size.Width);
                boundingBox1[3] = boundingBox1[3] * ((double)size.Height);
                var rect1 = new Rectangle((int)boundingBox1[0], (int)boundingBox1[1], (int)boundingBox1[2], (int)boundingBox1[3]);
                box[i] = rect1;
            }
            return probs;
        }



        private NDArray logistic(NDArray x)
        {
            var data = x.ToArray<float>();
            for (int i = 0; i < data.Length; i++)
            {
                var n = data[i];
                data[i] = (float)(n > 0 ? 1.0 / (1.0 + Math.Exp(-n)) : Math.Exp(n) / (1.0 + Math.Exp(n)));
            }
            var np1 = np.array<float>(data).reshape(x.shape);
            return np1;
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
            //const int W = 416;
            //const int H = 416;
            const int W = 512;
            const int H = 512;
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
