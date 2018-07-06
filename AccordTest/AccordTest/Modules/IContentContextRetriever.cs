namespace AccordTest.Modules
{
    /// <summary>
    /// Retrieve the most important sentences from the content. (For example, sentences that surrounds highlights).
    /// </summary>
    public interface IContentContextRetriever
    {
        string[] RetrieveContentContext(string[] contents);
    }
}
