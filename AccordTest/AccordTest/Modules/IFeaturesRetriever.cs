using System.Collections.Generic;

namespace AccordTest.Modules
{
    /// <summary>
    /// Retrieves features from interaction words based on word list.
    /// </summary>
    public interface IFeaturesRetriever
    {
        double[][] RetrieveFeatures(Dictionary<int, string>[] interactionsWords, string[] wordList);
    }
}
