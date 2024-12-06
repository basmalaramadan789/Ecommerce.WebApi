using Ecommerce.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces;
public interface ITokenService
{
    string CreateToken (ApplicationUser user);
}
