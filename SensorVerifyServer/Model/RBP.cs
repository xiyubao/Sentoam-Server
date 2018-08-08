using Accord.Neuro;
using Accord.Neuro.Learning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SensorVerifyServer
{
    public partial class MainWindow : Window
    {
        ActivationNetwork network_rbp;
        ResilientBackpropagationLearning teacher_rbp;

        public void RBPBuild(List<train> datalist)
        {
            double[][] inputs;
            double[][] outputs;
            double[][] matrix;

            GetData(out inputs, out outputs, out matrix, datalist);

            // create neural network
            network_rbp = new ActivationNetwork(
                new BipolarSigmoidFunction(),
                9, // two inputs in the network
                3, // two neurons in the first layer
                1//ont neuron in the second layer
                );
            // Randomly initialize the network
            new NguyenWidrow(network_rbp).Randomize();

            // create teacher
            teacher_rbp = new ResilientBackpropagationLearning(network_rbp);

            int times = 0;

            // loop
            while (times++ < 50)
            {
                // run epoch of learning procedure
                double error = teacher_rbp.RunEpoch(inputs, outputs);
                // check error value to see if we need to stop
                // ...
            }


            // Checks if the network has learned
          /*  for (int i = 0; i < inputs.Length; i++)
            {
                double[] answer = network.Compute(inputs[i]);

                log(answer[0].ToString()) ;

                // actual should be equal to expected
            }*/

        }

        public bool classifierRBP(data d)
        {
            int D = d.d;
            double[] input = new double[D];

            for (int i = 0; i < D; ++i)
                input[i] = d.msg[i];

            double[] answer = network_rbp.Compute(input);
            double actual = answer.Max();

            log(actual.ToString());

            return actual > 0;
        }
    }
}
