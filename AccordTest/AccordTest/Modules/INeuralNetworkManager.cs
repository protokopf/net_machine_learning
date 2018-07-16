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
        void Save(string policyName, string ruleName);

        ActivationNetwork Get(string policyName, string ruleName);
    }
}
