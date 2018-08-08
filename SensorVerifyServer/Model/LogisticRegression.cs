using Accord.Statistics.Models.Regression;
using Accord.Statistics.Models.Regression.Fitting;
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

        LogisticRegression regression;

        public void BuildLR(List<train> datalist)
        {
            double[][] input;
            int[] output;
            GetData(out input, out output, datalist);

            // To verify this hypothesis, we are going to create a logistic
            // regression model for those two inputs (age and smoking), learned
            // using a method called "Iteratively Reweighted Least Squares":

            var learner = new IterativeReweightedLeastSquares<LogisticRegression>()
            {
                Tolerance = 1e-4,  // Let's set some convergence parameters
                Iterations = 100,  // maximum number of iterations to perform
                Regularization = 0
            };

            // Now, we can use the learner to finally estimate our model:
            regression = learner.Learn(input, output);

            // At this point, we can compute the odds ratio of our variables.
            // In the model, the variable at 0 is always the intercept term, 
            // with the other following in the sequence. Index 1 is the age
            // and index 2 is whether the patient smokes or not.

            // For the age variable, we have that individuals with
            //   higher age have 1.021 greater odds of getting lung
            //   cancer controlling for cigarette smoking.
            double ageOdds = regression.GetOddsRatio(1); // 1.0208597028836701

            // For the smoking/non smoking category variable, however, we
            //   have that individuals who smoke have 5.858 greater odds
            //   of developing lung cancer compared to those who do not 
            //   smoke, controlling for age (remember, this is completely
            //   fictional and for demonstration purposes only).
            double smokeOdds = regression.GetOddsRatio(2); // 5.8584748789881331

            // If we would like to use the model to predict a probability for
            // each patient regarding whether they are at risk of cancer or not,
            // we can use the Probability function:

            double[] scores = regression.Probability(input);

            // Finally, if we would like to arrive at a conclusion regarding
            // each patient, we can use the Decide method, which will transform
            // the probabilities (from 0 to 1) into actual true/false values:


            log("The LogisticRegression model has been trained");
        }

        public bool classifierLR(data d)
        {
            int D = d.d;
            double[] input = new double[D];
            bool output;
            for (int i = 0; i < D; ++i)
                input[i] = d.msg[i];

            output = regression.Decide(input);

            return output;
        }

        
    }
}
