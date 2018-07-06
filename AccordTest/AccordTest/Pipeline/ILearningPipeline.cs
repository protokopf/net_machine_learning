namespace AccordTest.Pipeline
{
    /// <summary>
    /// Interface to pipeline.
    /// </summary>
    public interface ILearningPipeline
    {
        /// <summary>
        /// Trains model for specific rule.
        /// </summary>
        /// <param name="ruleName"></param>
        void Train(string ruleName);

        /// <summary>
        /// Predict value for specific rule.
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="interactionId"></param>
        /// <returns></returns>
        double Predict(string ruleName, string interactionId);

        /// <summary>
        /// Removes model for specific rule.
        /// </summary>
        /// <param name="ruleName"></param>
        void Forget(string ruleName);
    }
}
