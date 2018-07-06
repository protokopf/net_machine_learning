namespace AccordTest.Modules
{
    /// <summary>
    /// Retrieves all interactions for rule and check, whether rule is confirmed of rejected for each interaction.
    /// </summary>
    public interface IRuleLabelsRetriever
    {
        double[] RetrieveRuleLabes(string ruleName);
    }
}
