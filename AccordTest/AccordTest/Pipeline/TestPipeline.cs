using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccordTest.Modules;

namespace AccordTest.Pipeline
{
    /// <summary>
    /// Test pipeline.
    /// </summary>
    public class TestPipeline : ILearningPipeline
    {
        private readonly IRuleLabelsRetriever _labelsRetriever;

        private readonly IContentRetriever _contentRetriever;
        private readonly IContentContextRetriever _contextRetriever;
        private readonly IContentPreprocessor _preprocessor;
        private readonly IWordsRetriever _wordsRetriever;
        private readonly IWordListRetriever _wordListRetriever;
        private readonly IFeaturesRetriever _featuresRetriever;

        public TestPipeline(
            IRuleLabelsRetriever labelsRetriever, 
            IContentRetriever contentRetriever, 
            IContentContextRetriever contextRetriever, 
            IContentPreprocessor preprocessor, 
            IWordsRetriever wordsRetriever, 
            IWordListRetriever wordListRetriever, 
            IFeaturesRetriever featuresRetriever)
        {
            _labelsRetriever = labelsRetriever;
            _contentRetriever = contentRetriever;
            _contextRetriever = contextRetriever;
            _preprocessor = preprocessor;
            _wordsRetriever = wordsRetriever;
            _wordListRetriever = wordListRetriever;
            _featuresRetriever = featuresRetriever;
        }

        public void Train(string ruleName)
        {
            // Feature retrieving
             
            string[] rawContents = _contentRetriever.RetrieveContents(ruleName);

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

        public double Predict(string ruleName, string interactionId)
        {
            throw new NotImplementedException();
        }

        public void Forget(string ruleName)
        {
            throw new NotImplementedException();
        }
    }
}
