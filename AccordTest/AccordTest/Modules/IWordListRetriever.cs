using System.Collections.Generic;

namespace AccordTest.Modules
{
    /// <summary>
    /// Retrieves list of most used words. Should only be used when there are no word list created (like first training).
    /// </summary>
    public interface IWordListRetriever
    {
        string[] RetrieveWordList(Dictionary<int, string>[] interactionsWords);
    }
}
