using System.Collections.Generic;
using AccordTest.Entities;

namespace AccordTest.Modules
{
    /// <summary>
    /// Retrieves features from interaction words based on word list.
    /// </summary>
    public interface IFeaturesRetriever
    {
        double[][] RetrieveFeatures(CountedWord[] interactionsWords, string[] wordList);
    }
}
