using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IWordListRetriever _wordListRetriever;
        private readonly IFeaturesRetriever _featuresRetriever;

        public TestPipeline(
            IRuleLabelsRetriever labelsRetriever,
            IRuleQueriesRetriever queriesRetriever,
            IContentRetriever contentRetriever, 
            IContentContextRetriever contextRetriever, 
            IContentPreprocessor preprocessor, 
            IWordsRetriever wordsRetriever, 
            IWordListRetriever wordListRetriever, 
            IFeaturesRetriever featuresRetriever)
        {
            _labelsRetriever = labelsRetriever;
            _queriesRetriever = queriesRetriever;
            _contentRetriever = contentRetriever;
            _contextRetriever = contextRetriever;
            _preprocessor = preprocessor;
            _wordsRetriever = wordsRetriever;
            _wordListRetriever = wordListRetriever;
            _featuresRetriever = featuresRetriever;
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

                    Dictionary<int, string>[] interactionsWords = _wordsRetriever.RetrieveWords(preprocessedContents);

                    // Word list should be savedfor rule as well as network.
                    string[] wordList = null; //Try retrieve  word list for rule from file system.

                    if (wordList == null)
                    {
                        wordList = _wordListRetriever.RetrieveWordList(interactionsWords);
                        //Save word list for this rule.
                    }

                    double[][] features = _featuresRetriever.RetrieveFeatures(interactionsWords, wordList);
                    double[] labels = _labelsRetriever.RetrieveRuleLabes(ruleName);



                    //Train here
                }
            }
        }

        public double Predict(Policy policy, string interactionId)
        {
            throw new NotImplementedException();
        }

        public void Forget(Policy policy)
        {
            throw new NotImplementedException();
        }
    }
}
