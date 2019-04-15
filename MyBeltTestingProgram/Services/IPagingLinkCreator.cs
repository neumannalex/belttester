using Sieve.Models;

namespace MyBeltTestingProgram.Services
{
    public interface IPagingLinkCreator
    {
        string CreateNextPageLink(string operationName, SieveModel sieve);
        string CreatePreviousPageLink(string operationName, SieveModel sieve);
        string CreateSamePageLink(string operationName, SieveModel sieve);
    }
}