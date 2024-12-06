using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Identity;
public class Address
{
    public int id {  get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Streat { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string ApplicationUserId {  get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}
