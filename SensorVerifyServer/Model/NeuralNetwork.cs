using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SensorVerifyServer
{
    public partial class MainWindow : Window
    {
        public Thread workerThread;
        private List<train> tmp;

        private ActivationNetwork ann;

        double learningRate ;
        double sigmoidAlphaValue;
        int neuronsInFirstLayer;
        double iterations;

        bool useRegularization ;
        bool useNguyenWidrow ;
        bool useSameWeights;

        double iteration;
        double error;

        public bool ANN_END = false;

        public void ANNBuild(List<train> datalist)
        {
            tmp = new List<train>();

            learningRate = 0.1;
            sigmoidAlphaValue = 2;
            neuronsInFirstLayer = 20;
            iterations = 100;

            useRegularization = false;
            useNguyenWidrow = false;
            useSameWeights = false;

            int length = datalist.Count;
            for (int i = 0; i < length; ++i)
                tmp.Add(datalist[i]);

            workerThread = new Thread(new ThreadStart(SearchSolution));
            workerThread.Start();

            log("the ann model has been trained");
        }

        public void SearchSolution()
        {
            int length = tmp.Count;
            double[][] inputs;
            double[][] outputs;
            double[][] matrix;

            GetData(out inputs, out outputs, out matrix, tmp);

            // create multi-layer neural network
            this.ann = new ActivationNetwork(new BipolarSigmoidFunction(sigmoidAlphaValue),
                9, neuronsInFirstLayer, 1);

            if (useNguyenWidrow)
            {
                if (useSameWeights)
                    Accord.Math.Random.Generator.Seed = 1;

                NguyenWidrow initializer = new NguyenWidrow(ann);
                initializer.Randomize();
            }

            // create teacher
            LevenbergMarquardtLearning teacher = new LevenbergMarquardtLearning(ann, useRegularization);

            // set learning rate and momentum
            teacher.LearningRate = learningRate;

            // iterations
            iteration = 1;

            var ranges = matrix.GetRange(0);
            double[][] map = Matrix.Mesh(ranges[0], 200, ranges[1], 200);



          //  var sw = Stopwatch.StartNew();

            // loop
            while (true)
            {
                // run epoch of learning procedure
                error = teacher.RunEpoch(inputs, outputs) / length;

                var result = map.Apply(ann.Compute).GetColumn(0).Apply(Math.Sign);

                var graph = map.ToMatrix().InsertColumn(result.ToDouble());



                // increase current iteration
                iteration++;

                //elapsed = sw.Elapsed;

               // updateStatus();

                // check if we need to stop
                if ((iterations != 0) && (iteration > iterations))
                    break;
            }

            ANN_END = true;
         //   sw.Stop();
        }

        public bool classifierANN(data d)
        {
            int D = d.d;
            double[] input = new double[D];
            int output;
            for (int i = 0; i < D; ++i)
                input[i] = d.msg[i];

            ann.Compute(input);

            double []output_tmp = ann.Compute(input);

            output = System.Math.Sign(output_tmp[0]) > 0 ? 1 : 0;

            return output == 1;
        }
    }
}
