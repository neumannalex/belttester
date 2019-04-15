using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data.Repositories
{
    public class RepositoryItemAlreadyExistsException : Exception
    {
    }

    public class RepositoryItemNotFoundException : Exception
    {
    }

    public class RepositoryItemMismatchException : Exception
    {
    }
}
