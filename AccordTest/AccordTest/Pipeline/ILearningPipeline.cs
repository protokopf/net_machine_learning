using AccordTest.Entities;

namespace AccordTest.Pipeline
{
    /// <summary>
    /// Interface to pipeline.
    /// </summary>
    public interface ILearningPipeline
    {
        /// <summary>
        /// Trains model for policies
        /// </summary>
        /// <param name="policies"></param>
        void Train(Policy[] policies);

        /// <summary>
        /// Predict value for specific policy
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="interactionId"></param>
        /// <returns></returns>
        double Predict(Policy policy, string interactionId);

        /// <summary>
        /// Removes model for specific policy
        /// </summary>
        /// <param name="policy"></param>
        void Forget(Policy policy);
    }
}
