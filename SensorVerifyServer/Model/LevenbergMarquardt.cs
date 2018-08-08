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
        ActivationNetwork network_lm;
        LevenbergMarquardtLearning teacher_lm;

        public void LMBuild(List<train> datalist)
        {
            double[][] inputs;
            double[][] outputs;
            double[][] matrix;

            GetData(out inputs, out outputs, out matrix, datalist);

            // create neural network
            network_lm = new ActivationNetwork(
                new BipolarSigmoidFunction(),
                9, // two inputs in the network
                3, // two neurons in the first layer
                1//ont neuron in the second layer
                );
            // Randomly initialize the network
            new NguyenWidrow(network_lm).Randomize();

            // create teacher
            teacher_lm = new LevenbergMarquardtLearning(network_lm);

            int times = 0;

            // loop
            while (times++ < 50)
            {
                // run epoch of learning procedure
                double error = teacher_lm.RunEpoch(inputs, outputs);
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

        public bool classifierLM(data d)
        {
            int D = d.d;
            double[] input = new double[D];

            for (int i = 0; i < D; ++i)
                input[i] = d.msg[i];

            double[] answer = network_lm.Compute(input);
            double actual = answer.Max();

            log(actual.ToString());

            return actual > 0;
        }
    }
}
