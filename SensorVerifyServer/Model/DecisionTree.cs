using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.Math;
using Accord.Statistics.Filters;
using System.IO;

namespace SensorVerifyServer
{

    public partial class MainWindow : Window
    {
        public class Iris
        {
            public double[] attributes;
            public int type;
            public int attribute_count;
            public Iris()
            {
                attributes = new double[attribute_count];
            } 
            public Iris(string []ss,int count)
            {
                attribute_count = count-1;
                attributes = new double[attribute_count];
                for (int i = 0; i < attribute_count; ++i)
                    attributes[i] = double.Parse(ss[i]);
                switch(ss[attribute_count])
                {
                    case "Iris-setosa":type = 0;break;
                    case "Iris-versicolor":type = 1;break;
                    case "Iris-virginica":type = 2;break;
                    default:MessageBox.Show("error in class name");break;
                }
                Console.WriteLine(ss[attribute_count] + ":" + type);
            }
        }


        public DecisionTree tree;
        List<Iris> irislist;

        public bool classifierDT(data d)
        {
            int D = d.d;
            double[] input = new double[D];
            int output;
            for (int i = 0; i < D; ++i)
                input[i] = d.msg[i];

            output = tree.Decide(input);

            return output == 1;
        }

        public void DecisionTreeBuild(List<train> datalist)
        {
            int length = datalist.Count;
            int d = datalist[0].d;
            double[][] inputs;
            int[] outputs;
            GetData(out inputs, out outputs, datalist);

            DecisionVariable[] variables = new DecisionVariable[d];
            for (int i = 0; i < d; ++i)
                variables[i] = new DecisionVariable("attribute" + (i + 1), DecisionVariableKind.Continuous);

            // Create the C4.5 learning algorithm
            var c45 = new C45Learning(variables);
         
            // Learn the decision tree using C4.5
            tree = c45.Learn(inputs, outputs);
            

            log("The decision tree model has been trained");
        }

      
        public void DecisionTreeBuild(string Filename)
        {
            StreamReader sr = new StreamReader(Filename);
            string str;

            irislist = new List<Iris>();
            
            do
            {
                str = sr.ReadLine();
                if (str == null || str == "")
                    break;
                string[] ss = str.Split(',');
                int ss_length = ss.Length;
                for (int i = 0; i < ss_length; ++i)
                    ss[i] = ss[i].Trim();
                Iris iris = new Iris(ss,ss_length);
                irislist.Add(iris);
              
            }
            while (str != null && str != "");

            sr.Close();

            double[][] inputs;int[] outputs;
            GetData(out inputs, out outputs);
            
            // Specify the input variables
            DecisionVariable[] variables =
            {
                new DecisionVariable("SepalLength", DecisionVariableKind.Continuous),
                new DecisionVariable("SepalWidth", DecisionVariableKind.Continuous),
                new DecisionVariable("PetalLength", DecisionVariableKind.Continuous),
                new DecisionVariable("PetalWidth", DecisionVariableKind.Continuous),
            };

            // Create the C4.5 learning algorithm
            var c45 = new C45Learning(variables);

            // Learn the decision tree using C4.5
            tree = c45.Learn(inputs, outputs);

            
        }

        public void TrainAndTestByIrisData()
        {
            DecisionTreeBuild("iris.data");
            int length = irislist.Count;
            int attribute_number = irislist[0].attribute_count;
            int right = 0;
            for(int i= 135; i<length;++i)
            {
                double[] input = new double[attribute_number];
                for (int j = 0; j < attribute_number; ++j)
                    input[j] = irislist[i].attributes[j];
                int output = tree.Decide(input);
                Console.WriteLine(output);
                if (output == irislist[i].type)
                    right++;
            }
            MessageBox.Show("正确率 ： " + right / ((double)length-135) * 100 + " %");
        }
    }
}
