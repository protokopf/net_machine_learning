namespace AccordTest.Modules
{
    /// <summary>
    /// Used to sanitize content (remove redundant spaces, lowercase all letters and so on)
    /// </summary>
    public interface IContentPreprocessor
    {
        string[] PreprocessContents(string[] contents);
    }
}
