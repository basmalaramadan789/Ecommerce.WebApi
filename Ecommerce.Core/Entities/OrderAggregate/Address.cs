using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities.OrderAggregate;
public class Address
{
    public Address()
    {
         
    }
    public Address(string firstName, string lastName, string streat, string city, string state, string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        Streat = streat;
        City = city;
        State = state;
        ZipCode = zipCode;

    }
    public int id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Streat { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}
