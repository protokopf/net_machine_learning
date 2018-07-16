using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccordTest.Entities;

namespace AccordTest.Modules
{
    public interface IRuleQueriesRetriever
    {
        /// <summary>
        /// Retrieve queries for policies.
        /// </summary>
        /// <param name="policyName"></param>
        /// <param name="ruleName"></param>
        /// <returns></returns>
        string[] RetrieveQueriesFor(string policyName, string ruleName);
    }
}
