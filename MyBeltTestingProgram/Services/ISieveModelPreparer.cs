using Sieve.Models;

namespace MyBeltTestingProgram.Services
{
    public interface ISieveModelPreparer
    {
        void SetMissingValues(ref SieveModel model);
    }
}