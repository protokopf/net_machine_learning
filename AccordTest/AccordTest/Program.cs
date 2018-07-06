using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Accord.Controls;
using Accord.IO;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using ZedGraph;

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
            double[][] y = outputs.ToDouble().ToJagged();

            // Iterate until stop criteria is met
            double error = double.PositiveInfinity;
            double previous;

            Dictionary<int, double> epochError = new Dictionary<int, double>();

            int currentEpoch = 1;

            do
            {
                previous = error;

                // Compute one learning iteration
                error = teacher.RunEpoch(inputs, y);
                epochError.Add(currentEpoch++, error);

            } while (Math.Abs(previous - error) > 0.001);

            network.Save(networkName);


            // Classify the samples using the model
            double[] decimalAnswers = inputs.Apply(network.Compute).GetColumn(0); // can be used as probability.
            int[] answers = decimalAnswers.Apply(Math.Sign);

            // Plot the results
            ScatterplotBox.Show("Expected results", inputs, outputs);
            ScatterplotBox.Show("Network results", inputs, answers);

            PlotError("Training error", epochError);
        }

        private static void PlotError(string errorName, Dictionary<int, double> epochsErrors)
        {
            double[] epochs = epochsErrors.Keys.Select(e => (double)e).ToArray();
            double[] errors = epochsErrors.Values.ToArray();

            ScatterplotView spv = new ScatterplotView
            {
                Dock = DockStyle.Fill,
                LinesVisible = true
            };

            spv.Graph.GraphPane.XAxis.Title.Text = "Epoch";
            spv.Graph.GraphPane.YAxis.Title.Text = "Error";
            spv.Graph.GraphPane.Title.Text = "Error changing through epochs";

            spv.Graph.GraphPane.AddCurve(errorName, epochs, errors, Color.Green, SymbolType.None);

            spv.Graph.GraphPane.AxisChange();

            Form f1 = new Form
            {
                Width = 600,
                Height = 400
            };
            f1.Controls.Add(spv);
            f1.ShowDialog();
        }

        private static void EraseForRule(string ruleName)
        {
            //1. Remove network for rule.
        }

        private static void TrainForRule(string ruleName)
        {
            //1. Find network for this rule. Use it if exists. Create new if not. => Network object

            //2. Request all interactions, that is related to the provided rule. => Interaction[]

            //3. Extract contents from each interaction. 
            //   If multiple interaction's content - combine to one string. => string[]

            //4. Preprocess each content => string[]
            //  4.1. Lowercase it
            //  4.2. Replace all spaces with one space.

            //5. Retrieve surrounding sentences for each rule match inside content Concatenate them. => string[]

            //6. Extract words from contents. => map (word:count)[] for each interaction

            //7. Find word list for this rule. Use it if exists. Create new if not => string[];
            //   If not:
            //      - Combine 'map (word:count)[]' from all interactions.
            //      - Sort them by count descending
            //      - Get first WORDS_SIZE words.
            //      - Save to the file for the rule.

            //8. Map each 'map (word:count)[]' to features (double[WORDS_SIZE])
            //   where each double number of occurrence of each word. 

            //9. Combine features of all interaction => (double[INTERACTIONS_COUNT][WORDS_SIZE])

            //10. For each interaction find out whether rule for it was rejected or confirmed => double[INTERACTIONS_COUNT]

            //11. Train network for prepared data. (M times)

            //12. Save trained network.

        }

        private static double PredictForRule(string ruleName, string interactionIdenifier)
        {
            //1. Find network for this rule. Use it if exists. If now - throw exception.

            //2. Extract interaction. => Interaction

            //3. Extract contents for interaction => string;
            //   If multiple interaction's content - combine to one string.

            //4. Preprocess each content => string
            //  4.1. Lowercase it
            //  4.2. Replace all spaces with one space.

            //5. Retrieve surrounding sentences for each rule match inside content Concatenate them. => string

            //6. Extract words from contents. => map (word:count)[] for interaction

            //7. Map 'map (word:count)[]' to features (double[WORDS_SIZE])
            //   where each double number of occurrence of each word. 

            //8. Use network to predict value.

            return 0;
        }
    }
}
