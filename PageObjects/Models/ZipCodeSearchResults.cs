using AngleSharp.Dom;
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
    public string ZipCodeUrl { get; set; }
    public string Longitude { get; set; }
    public string Latitude { get; set; }

    public ZipCodeSearchResults(string city = "", string state = "", string zipcode = "", string zipcodeurl = "", string longitude = "", string latitude = "")
    {
        City = city;
        State = state;
        ZipCode = zipcode;
        ZipCodeUrl = zipcodeurl;
        Longitude = longitude;
        Latitude = latitude;
    }
}
