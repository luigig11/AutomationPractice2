using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationPractice2.PageObjects.Models;

public class ZipCodeSearchResults
{
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Longitude { get; set; }
    public string Latitude { get; set; }

    public ZipCodeSearchResults(string city = "", string state = "", string zipcode = "", string longitude = "", string latitude = "")
    {
        string City = city;
        string State = state;
        string ZipCode = zipcode;
        string Longitude = longitude;
        string Latitude = latitude;
    }
}
