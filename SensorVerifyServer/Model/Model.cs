using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SensorVerifyServer
{
    public partial class MainWindow : Window
    {
        public String filename = @"..\..\File\1.csv";
        public String filename2 = @"..\..\File\2.txt";

        public static char interval = '&';
        public Evaluation eva;

        public String[] metrics = new String[8] { "TN", "TP ", "FN", "FP", "Accuracy", "Precision", "Recall_rate", "F1_score" };


        public class Evaluation
        {
            public List<List<bool>> flag;
            public List<bool> flagR;
            public int length;

            public List<Classifier> classifier;
            public String[] name = new String[8] { "SVM", "Decision Tree", "Logistic Regression", "Random Forest" , "Kernal-SVM", "Resilient Back-propagation", "Back-Propagation", "Levenberg-Marquardt" };
            public Evaluation()
            {
                flag = new List<List<bool>>();
                flagR = new List<bool>();
                length = 0;

                classifier = new List<Classifier>();
            }

            public void Add(bool flagR, List<bool> flag)
            {
                this.flagR.Add(flagR);
                this.flag.Add(flag);
                this.length++;
            }

            public void Analyst()
            {
                int D = flag[0].Count();
                for (int i = 0; i < D; ++i)
                {
                    List<bool> fr = new List<bool>();
                    List<bool> f = new List<bool>();
                    for (int j = 0; j < length; ++j)
                    {
                        fr.Add(this.flagR[j]);
                        f.Add(this.flag[j][i]);
                    }
                    this.classifier.Add(new Classifier(fr, f));
                }
            }

            public List<String> Output()
            {
                List<String> msg = new List<string>();
                int classifier_len = classifier.Count();
                for (int i = 0; i < classifier_len; ++i)
                {
                    msg.Add(name[i] + interval + classifier[i].output());
                }
                return msg;
            }

            public List<String> Output2()
            {
                List<String> msg = new List<string>();
                int classifier_len = classifier.Count();
                for (int i = 0; i < classifier_len; ++i)
                {
                    msg.Add(name[i] + interval + classifier[i].output2());
                }
                return msg;
            }
        }

        public class Classifier
        {
            public int TN, TP, FN, FP;
            public double Accuracy, Precision, Recall_rate, F1_score;

            public List<bool> flagR;
            public List<bool> flag;

            public Classifier(List<bool> flagR, List<bool> flag)
            {
                this.flagR = flagR;
                this.flag = flag;
                Analyst();
            }

            public void Analyst()
            {
                TN = TP = FN = FP = 0;
                int length = this.flag.Count();
                for (int i = 0; i < length; ++i)
                {
                    if (flagR[i] == true)
                        if (flag[i] == true)
                            TP++;
                        else
                            FP++;
                    else
                        if (flag[i] == true)
                        FN++;
                    else
                        TN++;
                }

                Accuracy = ((double)TN + TP) / (TN + FP + FN + TP);
                Precision = (double)TP / (TP + FP);
                Recall_rate = (double)TP / (TP + FN);
                F1_score = 2.0 * TP / (2 * TP + FP + FN);
            }

            public string output()
            {
                return TN + interval + TP + interval + FN + interval + FP + interval + Accuracy .ToString("f4")+ interval + Precision.ToString("f4") + interval + Recall_rate.ToString("f4") + interval + F1_score.ToString("f4");
            }

            public string output2()
            {
                return Accuracy.ToString("f4") + interval + Precision.ToString("f4") + interval + Recall_rate.ToString("f4") + interval + F1_score.ToString("f4")+" \\cr";
            }
        }
        
        public void TrianModelList(List<train> train_set)
        {
            DecisionTreeBuild(train_set);
            BuildSVM(train_set);
            BuildLR(train_set);
            RandomForestBuild(train_set);
            KSVMBuild(train_set);
            RBPBuild(train_set);
            BPBuild(train_set);
            LMBuild(train_set);
        }
        
        public List<bool> ClassifierList(data d)
        {
            List<bool> result = new List<bool>();
            result.Add(classifierSVM(d));
            result.Add(classifierDT(d));
            result.Add(classifierLR(d));
            result.Add(classifierforest(d));
            result.Add(classifierKSVM(d));
            result.Add(classifierRBP(d));
            result.Add(classifierBP(d));
            result.Add(classifierLM(d));
            return result;
        }

        public void AnalystValidationData(List<train> validation_set)
        {
            eva = new Evaluation();

            int length = validation_set.Count;

            for (int i = 0; i < length; ++i)
            {
                string output = "";
                data tmp = new data(validation_set[i].d, validation_set[i].msg1, validation_set[i].msg2);
                List<bool> result = ClassifierList(tmp);

                eva.Add(validation_set[i].flag, result);

                #region log

                output += i + " th data: flag = " + validation_set[i].flag + " : ";
                for (int j = 0; j < result.Count; ++j)
                    output += result[j] + ",";
                output += "\n";

                log(output);
                #endregion
            }
            
            eva.Analyst();

            #region  log
            String title = "classifier";
            int metrics_len = metrics.Count();
            for(int i=0;i< metrics_len;++i)
            {
                title += interval + metrics[i]  ;
            }

            List<String> msg = eva.Output();
            List<String> msg2 = eva.Output2();

            int msg_len = msg.Count();

            log(title);
            for(int i=0;i<msg_len; ++i)
            {
                log(msg[i]);
            }
            #endregion

            WriteToFile(filename,title,msg);
            WriteToFile(filename2, title, msg2);
        }
      
        public void GetData(out double[][] inputs, out int[] outputs)
        {
            int length = irislist.Count;
            int attribute_number = irislist[0].attribute_count;

            inputs = new double[length][];
            for (int i = 0; i < length; ++i)
            {
                inputs[i] = new double[attribute_number];
                for (int j = 0; j < attribute_number; ++j)
                    inputs[i][j] = irislist[i].attributes[j];
            }

            // Get only the output labels (last column)
            outputs = new int[length];
            for (int i = 0; i < length; ++i)
                outputs[i] = irislist[i].type;
        }

        public void GetData(out double[][] inputs, out int[] outputs, List<train> datalist)
        {
            int length = datalist.Count;
            int d = datalist[0].d;

            inputs = new double[length][];
            for (int i = 0; i < length; ++i)
            {
                inputs[i] = new double[d];
                for (int j = 0; j < d; ++j)
                    inputs[i][j] = Math.Abs(datalist[i].msg1[j] - datalist[i].msg2[j]);
            }

            // Get only the output labels (last column)
            outputs = new int[length];
            for (int i = 0; i < length; ++i)
                outputs[i] = datalist[i].flag ? 1 : 0;
        }

        public void GetData(out double[][] inputs, out double[][] outputs, out double[][]matrix,List<train> datalist)
        {
            int length = datalist.Count;
            int d = datalist[0].d;

            inputs = new double[length][];
            matrix = new double[length][];
            outputs = new double[length][];

            for (int i = 0; i < length; ++i)
            {
                matrix[i] = new double[d + 1];
                inputs[i] = new double[d];
                outputs[i] = new double[1];
                for (int j = 0; j < d; ++j)
                {
                    inputs[i][j] = Math.Abs(datalist[i].msg1[j] - datalist[i].msg2[j]);
                    matrix[i][j] = inputs[i][j];
                }
                outputs[i][0] = datalist[i].flag ? 1 : -1;
                matrix[i][d] = outputs[i][0];
            }
        }

        public void GetData(out double[][] inputs, out int[] outputs, List<train> datalist,int n,int k,out int []indexs)
        {
            int length = datalist.Count;
            int d = datalist[0].d;

            List<train> dataset = new List<train>();
            Random random = new Random();
            for (int i=0;i<n;++i)
            {
                int index = random.Next(length);
                dataset.Add(datalist[index]);
            }

            List<int> indexlist = new List<int>();
            for (int i = 0; i < d; ++i)
                indexlist.Add(i);

            List<int> finalindex = new List<int>();
            for(int i=0;i<k;++i)
            {
                int index = random.Next(indexlist.Count);
                finalindex.Add(indexlist[index]);
                indexlist.RemoveAt(index);
            }

            finalindex.Sort();
            

            inputs = new double[n][];
            for (int i = 0; i < n; ++i)
            {
                inputs[i] = new double[k];
                for (int j = 0; j < k; ++j)
                    inputs[i][j] = Math.Abs(dataset[i].msg1[finalindex[j]] - dataset[i].msg2[finalindex[j]]);
            }

            // Get only the output labels (last column)
            outputs = new int[n];
            for (int i = 0; i < n; ++i)
                outputs[i] = dataset[i].flag ? 1 : 0;

            indexs = new int[k];
            for (int i = 0; i < k; ++i)
                indexs[i] = finalindex[i];
        }
        
        public void WriteToFile(String filename,String title,List<String> msg)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine(title);
            int length = msg.Count();
            for (int i = 0; i < length; ++i)
                sw.WriteLine(msg[i]);

            sw.Close();
        }

        public void CrossValidation(out List<train> train_set, out List<train> validacation_set,List<train> dataset)
        {
            int length = dataset.Count();
            int quarter = length / 4;
            train_set = new List<train>();
            validacation_set = new List<train>();
            List<train> tmp = new List<train>();
            for (int i = 0; i < length; ++i)
                tmp.Add(dataset[i]);

            Random rand = new Random();

            for(int i=0;i<quarter;++i)
            {
                int index = rand.Next(length - i);
                validacation_set.Add(tmp[index]);
                tmp.RemoveAt(index);
            }

            for (int i = 0; i < length - quarter + 1; ++i)
                train_set.Add(dataset[i]);
        }
    }
}
