using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.AuthApi.Identity.Exceptions;
public class EmailAlreadyExistsException : Exception
{
    public EmailAlreadyExistsException() : base("Email already exists.")
    {

    }
}
