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
        ActivationNetwork network_bp;
        BackPropagationLearning teacher_bp;

        public void BPBuild(List<train> datalist)
        {
            double[][] inputs;
            double[][] outputs;
            double[][] matrix;

            GetData(out inputs, out outputs, out matrix, datalist);

            // create neural network
            network_bp = new ActivationNetwork(
                new BipolarSigmoidFunction(),
                9, // two inputs in the network
                3, // two neurons in the first layer
                1//ont neuron in the second layer
                );
            // Randomly initialize the network
            new NguyenWidrow(network_bp).Randomize();

            // create teacher
            teacher_bp = new BackPropagationLearning(network_bp);

            int times = 0;

            // loop
            while (times++ < 50)
            {
                // run epoch of learning procedure
                double error = teacher_bp.RunEpoch(inputs, outputs);
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

        public bool classifierBP(data d)
        {
            int D = d.d;
            double[] input = new double[D];
            int output;
            for (int i = 0; i < D; ++i)
                input[i] = d.msg[i];

            double[] answer = network_bp.Compute(input);
            double actual = answer.Max();

            log(actual.ToString());

            return actual > 0;
        }
    }
}
