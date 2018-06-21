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
using System.Data;
using Accord.Neuro;
using Accord.Neuro.Learning;
using System.IO;

namespace AccordTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read the Excel worksheet into a DataTable
            DataTable table = new ExcelReader("examples.xls").GetWorksheet("Classification - Yin Yang");

            // Convert the DataTable to input and output vectors
            double[][] inputs = table.ToJagged<double>("X", "Y");
            int[] outputs = table.Columns["G"].ToArray<int>();


            // Since we would like to learn binary outputs in the form
            // [-1,+1], we can use a bipolar sigmoid activation function
            IActivationFunction function = new BipolarSigmoidFunction();

            Console.Write("Enter network name: ");
            string networkName = Console.ReadLine();

            Network network = null;

            if (File.Exists(networkName))
            {
                network = Network.Load(networkName);
            }
            else
            {
                network = new ActivationNetwork(function,
                    inputsCount: 2, neuronsCount: new[] { 10, 5, 1 });
            }


            // Create a Levenberg-Marquardt algorithm
            var teacher = new LevenbergMarquardtLearning((ActivationNetwork)network)
            {
                UseRegularization = true
            };

            // Because the network is expecting multiple outputs,
            // we have to convert our single variable into arrays
            //
            double[][] y = outputs.ToDouble().ToJagged();

            // Iterate until stop criteria is met
            double error = double.PositiveInfinity;
            double previous;

            do
            {
                previous = error;

                // Compute one learning iteration
                error = teacher.RunEpoch(inputs, y);

            } while (Math.Abs(previous - error) > 1);

            network.Save(networkName);


            // Classify the samples using the model
            int[] answers = inputs.Apply(network.Compute).GetColumn(0).Apply(Math.Sign);

            // Plot the results
            ScatterplotBox.Show("Expected results", inputs, outputs);
            ScatterplotBox.Show("Network results", inputs, answers)
            .Hold();
        }
    }
}
