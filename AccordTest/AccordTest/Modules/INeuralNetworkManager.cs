using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Neuro;

namespace AccordTest.Modules
{
    public interface INeuralNetworkManager
    {
        void Save(ActivationNetwork network, string policyName, string ruleName);

        ActivationNetwork Get(string policyName, string ruleName);

        void Delete(string policyName, string ruleName);
    }
}
