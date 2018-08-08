using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Controls;
using Accord.IO;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using Accord.MachineLearning.Bayes;

using System.Windows;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Statistics.Kernels;

namespace SensorVerifyServer
{
    public partial class MainWindow : Window
    {
        Accord.MachineLearning.VectorMachines.SupportVectorMachine<Gaussian> ksvm;

        public void KSVMBuild(List<train> datalist)
        {
            double[][] inputs;
            int[] outputs;

            GetData(out inputs, out outputs, datalist);

            // Create a new Sequential Minimal Optimization (SMO) learning 
            // algorithm and estimate the complexity parameter C from data
            var teacher = new SequentialMinimalOptimization<Gaussian>()
            {
                UseComplexityHeuristic = true,
                UseKernelEstimation = true // estimate the kernel from the data
            };

            // Teach the vector machine
            ksvm = teacher.Learn(inputs, outputs);

            log("the ann model has been trained");
        }

        public bool classifierKSVM(data d)
        {
            int D = d.d;
            double[][] input = new double[1][];
            input[0] = new double[D];
            int output;
            for (int i = 0; i < D; ++i)
                input[0][i] = d.msg[i];

            // Classify the samples using the model
            // Classify the samples using the model
            bool[] answers = svm.Decide(input);

            return answers[0] ;
        }
    }
}
