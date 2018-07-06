using System.Collections.Generic;

namespace AccordTest.Modules
{
    /// <summary>
    /// For each interaction creates dictionary where each value - word and number of occurance.
    /// </summary>
    public interface IWordsRetriever
    {
        Dictionary<int, string>[] RetrieveWords(string[] contents);
    }
}
