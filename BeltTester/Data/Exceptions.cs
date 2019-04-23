using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Data
{
    public class RepositoryItemAlreadyExistsException : Exception
    {
        public RepositoryItemAlreadyExistsException(string message = null) : base(message)
        {
        }
    }

    public class RepositoryItemNotFoundException : Exception
    {
        public RepositoryItemNotFoundException(string message = null) : base(message)
        {
        }
    }

    public class RepositoryItemMismatchException : Exception
    {
        public RepositoryItemMismatchException(string message = null) : base(message)
        {
        }
    }

    public class RepositoryFilterException : Exception
    {
        public RepositoryFilterException(string message = null) : base(message)
        {
        }
    }
}
