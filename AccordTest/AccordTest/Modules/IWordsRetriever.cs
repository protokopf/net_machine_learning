using System.Collections.Generic;
using AccordTest.Entities;

namespace AccordTest.Modules
{
    /// <summary>
    /// For each interaction creates dictionary where each value - word and number of occurance.
    /// </summary>
    public interface IWordsRetriever
    {
        CountedWord[] RetrieveWords(string[] contents);
    }
}
