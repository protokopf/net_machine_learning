﻿namespace AccordTest.Modules
{
    /// <summary>
    /// Retrieves all contents of all interactions related to the rule.
    /// </summary>
    public interface IContentRetriever
    {
        string[] RetrieveContents(string[] queries, string interactionId = null);
    }
}
