using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
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
        List<DecisionTree> forest;

        public void RandomForestBuild(List<train> datalist)
        {
            int length = datalist.Count;
            int d = datalist[0].d;

            forest = new List<DecisionTree>();

            int n = datalist.Count;
            int k = d; //(int)Math.Sqrt(d);
            int m = 100;

            for (int i = 0; i < m; ++i)
            {
                double[][] inputs;
                int[] outputs;
                int[] indexs;
                GetData(out inputs, out outputs, datalist,n,k,out indexs);

                DecisionVariable[] variables = new DecisionVariable[k];
                for (int j = 0; j < k; ++j)
                    variables[j] = new DecisionVariable("attribute" + (indexs[j]+1), DecisionVariableKind.Continuous);

                // Create the C4.5 learning algorithm
                var c45 = new C45Learning(variables);

                // Learn the decision tree using C4.5
                DecisionTree dtmp = c45.Learn(inputs, outputs);
                forest.Add(dtmp);
            }
            log("The random forest model has been trained");
        }

        public bool classifierforest(data d)
        {
            int D = d.d;
            double[] input = new double[D];
           
            for (int i = 0; i < D; ++i)
                input[i] = d.msg[i];
            int output_true = 0;
            int length = forest.Count;
            for(int i=0;i<length;++i)
            {
                int len = forest[i].Attributes.Count;
                double[] intput = new double[len];
                for (int j = 0; j < len; ++j)
                {
                    string tmp = forest[i].Attributes[j].Name.Remove(0, "attribute".Count());
                    int index = int.Parse(tmp)-1;
                    input[j] = d.msg[index];
                }

                if(forest[i].Decide(input)==1)
                    ++output_true;
            }


            return output_true>length/2;
        }

    }
}
