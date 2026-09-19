using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Exceptions
{
    public class InvalidTextException(string msg) : ChatException(msg)
    {
    }
}
