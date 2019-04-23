using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Services
{
    public interface IPagingLinkCreator
    {
        string CreateNextPageLink(string operationName, SieveModel sieve);
        string CreatePreviousPageLink(string operationName, SieveModel sieve);
        string CreateSamePageLink(string operationName, SieveModel sieve);
    }

    public class PagingLinkCreator : IPagingLinkCreator
    {
        private readonly IUrlHelper _urlHelper;

        public PagingLinkCreator(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public string CreatePreviousPageLink(string operationName, SieveModel sieve)
        {
            var newSieve = new SieveModel
            {
                Filters = sieve.Filters,
                Sorts = sieve.Sorts,
                PageSize = sieve.PageSize,
                Page = sieve.Page - 1
            };

            return _urlHelper.Link(operationName, newSieve);
        }

        public string CreateSamePageLink(string operationName, SieveModel sieve)
        {
            var newSieve = new SieveModel
            {
                Filters = sieve.Filters,
                Sorts = sieve.Sorts,
                PageSize = sieve.PageSize,
                Page = sieve.Page
            };

            return _urlHelper.Link(operationName, newSieve);
        }

        public string CreateNextPageLink(string operationName, SieveModel sieve)
        {
            var newSieve = new SieveModel
            {
                Filters = sieve.Filters,
                Sorts = sieve.Sorts,
                PageSize = sieve.PageSize,
                Page = sieve.Page + 1
            };

            return _urlHelper.Link(operationName, newSieve);
        }
    }
}
