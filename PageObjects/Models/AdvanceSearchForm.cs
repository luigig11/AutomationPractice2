using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPractice2.PageObjects.Models;

public class AdvanceSearchForm
{
    public string ZipCode { get; set; }
    public string Town_City { get; set; }
    public string State { get; set; }
    public string County { get; set; }
    public string AreaCode { get; set; }

    public AdvanceSearchForm(string zipCode = "", string town_city = "", string state = "", string county = "", string areaCode = "")
    {
        ZipCode = zipCode;
        Town_City = town_city;
        State = state;
        County = county;
        AreaCode = areaCode;
    }
}
