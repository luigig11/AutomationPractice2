using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPractice2.TDM;

public static class BellatrixTDM
{
    public static IEnumerable<TestCaseData> BellatrixData()
    {
        yield return new TestCaseData( "Falcon 9", "50.00", "1" , "50.00", "50.00", "10.00", "60.00", "Direct bank transfer");
        yield return new TestCaseData("Saturn V", "120.00", "3", "360.00", "360.00", "0.00", "360.00", "Direct bank transfer");
    }
}