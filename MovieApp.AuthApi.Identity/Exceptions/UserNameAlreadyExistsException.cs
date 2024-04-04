using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.AuthApi.Identity.Exceptions;
public class UserNameAlreadyExistsException : Exception
{
    public UserNameAlreadyExistsException(): base("Username already exists.")
    {
        
    }
}
