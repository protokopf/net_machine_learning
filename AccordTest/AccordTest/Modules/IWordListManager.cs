using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccordTest.Modules
{
    public interface IWordListManager
    {
        string[] Get(string policyName, string ruleName);

        void Save(string policyName, string ruleName, string[] words);
    }
}
