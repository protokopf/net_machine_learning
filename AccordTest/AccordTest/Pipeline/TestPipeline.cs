using System;
using System.Collections.Generic;
using Accord.Math;
using Accord.Neuro;
using Accord.Neuro.Learning;
using AccordTest.Entities;
using AccordTest.Modules;

namespace AccordTest.Pipeline
{
    /// <summary>
    /// Test pipeline.
    /// </summary>
    public class TestPipeline : ILearningPipeline
    {
        private readonly IRuleLabelsRetriever _labelsRetriever;

        private readonly IRuleQueriesRetriever _queriesRetriever;
        private readonly IContentRetriever _contentRetriever;
        private readonly IContentContextRetriever _contextRetriever;
        private readonly IContentPreprocessor _preprocessor;
        private readonly IWordsRetriever _wordsRetriever;
        private readonly IWordListManager _wordListManager;
        private readonly IWordListRetriever _wordListRetriever;
        private readonly IFeaturesRetriever _featuresRetriever;
        private readonly INeuralNetworkManager _nnManager;

        public TestPipeline(
            IRuleLabelsRetriever labelsRetriever,
            IRuleQueriesRetriever queriesRetriever,
            IContentRetriever contentRetriever, 
            IContentContextRetriever contextRetriever, 
            IContentPreprocessor preprocessor, 
            IWordsRetriever wordsRetriever,
            IWordListManager wordListManager,
            IWordListRetriever wordListRetriever, 
            IFeaturesRetriever featuresRetriever,
            INeuralNetworkManager nnManager)
        {
            _labelsRetriever = labelsRetriever;
            _queriesRetriever = queriesRetriever;
            _contentRetriever = contentRetriever;
            _contextRetriever = contextRetriever;
            _preprocessor = preprocessor;
            _wordsRetriever = wordsRetriever;
            _wordListRetriever = wordListRetriever;
            _wordListManager = wordListManager;
            _featuresRetriever = featuresRetriever;
            _nnManager = nnManager;
        }

        public void Train(Policy[] policies)
        {
            // Feature retrieving

            foreach (Policy policy in policies)
            {
                foreach (string ruleName in policy.RuleNames)
                {
                    string[] queries = _queriesRetriever.RetrieveQueriesFor(policy.Name, ruleName);

                    string[] rawContents = _contentRetriever.RetrieveContents(queries);

                    string[] rawContentsContext = _contextRetriever.RetrieveContentContext(rawContents);

                    string[] preprocessedContents = _preprocessor.PreprocessContents(rawContentsContext);

                    CountedWord[] interactionsWords = _wordsRetriever.RetrieveWords(preprocessedContents);

                    string[] wordList = _wordListManager.Get(policy.Name, ruleName);

                    if (wordList == null)
                    {
                        wordList = _wordListRetriever.RetrieveWordList(interactionsWords);
                        _wordListManager.Save(policy.Name, ruleName, wordList);
                    }

                    double[][] inputs = _featuresRetriever.RetrieveFeatures(interactionsWords, wordList);
                    double[] outputs = _labelsRetriever.RetrieveRuleLabes(policy.Name, ruleName);

                    ActivationNetwork network = _nnManager.Get(policy.Name, ruleName);


                    // Create a Levenberg-Marquardt algorithm
                    var teacher = new LevenbergMarquardtLearning(network)
                    {
                        UseRegularization = true
                    };

                    // Because the network is expecting multiple outputs,
                    // we have to convert our single variable into arrays

                    //We should make sure outputs are [0...1]
                    double[][] y = outputs.ToJagged();

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

                    _nnManager.Save(network, policy.Name, ruleName);

                    // Classify the samples using the model
                    double[] decimalAnswers = inputs.Apply(network.Compute).GetColumn(0); // can be used as probability.
                }
            }
        }

        public double Predict(Policy policy, string interactionId)
        {
            // Feature retrieving
            string policyName = policy.Name;
            string ruleName = policy.RuleNames[0];

            string[] queries = _queriesRetriever.RetrieveQueriesFor(policyName, ruleName);

            string[] rawContents = _contentRetriever.RetrieveContents(queries, interactionId);

            string[] rawContentsContext = _contextRetriever.RetrieveContentContext(rawContents);

            string[] preprocessedContents = _preprocessor.PreprocessContents(rawContentsContext);

            CountedWord[] interactionsWords = _wordsRetriever.RetrieveWords(preprocessedContents);

            string[] wordList = _wordListManager.Get(policyName, ruleName);

            if (wordList == null)
            {
                wordList = _wordListRetriever.RetrieveWordList(interactionsWords);
                _wordListManager.Save(policyName, ruleName, wordList);
            }


            double[][] inputs = _featuresRetriever.RetrieveFeatures(interactionsWords, wordList);


            ActivationNetwork network = _nnManager.Get(policy.Name, ruleName);


            // Classify the samples using the model
            //We should make sure decimalAnswers are [0...1]
            double[] decimalAnswers = inputs.Apply(network.Compute).GetColumn(0); // can be used as probability.


            return decimalAnswers[0];
        }

        public void Forget(Policy policy)
        {
            foreach(string ruleName in policy.RuleNames)
            {
                _nnManager.Delete(policy.Name, ruleName);
            }
        }
    }
}
