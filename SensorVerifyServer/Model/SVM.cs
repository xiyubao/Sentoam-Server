using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Controls;
using Accord.MachineLearning.VectorMachines;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Math;
using Accord.Statistics.Kernels;
using System.Windows;

namespace SensorVerifyServer
{
    public partial class MainWindow : Window
    {
        SupportVectorMachine svm;

        public void BuildSVM(List<train> datalist)
        {
            double[][] inputs;
            int[] outputs;
            GetData(out inputs, out outputs, datalist);

            // Now, we can create the sequential minimal optimization teacher
            var learn = new SequentialMinimalOptimization()
            {
                UseComplexityHeuristic = true,
                UseKernelEstimation = false
            };

            // And then we can obtain a trained SVM by calling its Learn method
             svm = learn.Learn(inputs, outputs);

            log("svm model has been trained");
        }

        public bool classifierSVM(data d)
        {
            int D = d.d;
            double[] input = new double[D];
            bool output;
            for (int i = 0; i < D; ++i)
                input[i] = d.msg[i];

            output = svm.Decide(input);

            return output;
        }

    }
}
