using Microsoft.Extensions.Configuration;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Services
{
    public class SieveModelPreparer : ISieveModelPreparer
    {
        private readonly IConfiguration _configuration;

        private readonly int _defaultPageSize = 10;
        private readonly int _defaultPage = 1;

        public SieveModelPreparer(IConfiguration configuration)
        {
            _configuration = configuration;

            IConfigurationSection section = _configuration.GetSection("Sieve");
            if (section.Exists())
            {
                if (section.GetValue<int>("DefaultPageSize") > 0)
                    _defaultPageSize = section.GetValue<int>("DefaultPageSize");
            }
        }

        public void SetMissingValues(ref SieveModel model)
        {
            if (!model.PageSize.HasValue)
                model.PageSize = _defaultPageSize;

            if (!model.Page.HasValue)
                model.Page = _defaultPage;
        }
    }
}
