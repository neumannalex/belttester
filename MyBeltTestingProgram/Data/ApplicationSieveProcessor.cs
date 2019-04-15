using Microsoft.Extensions.Options;
using MyBeltTestingProgram.Data.Models;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(IOptions<SieveOptions> options, ISieveCustomSortMethods customSortMethods, ISieveCustomFilterMethods customFilterMethods) : base(options, customSortMethods, customFilterMethods)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<Motion>(p => p.Move.Name)
                .CanFilter();

            mapper.Property<Motion>(p => p.Move.Symbol)
                .CanFilter();

            mapper.Property<Motion>(p => p.Stance.Name)
                .CanFilter();

            mapper.Property<Motion>(p => p.Stance.Symbol)
                .CanFilter();

            mapper.Property<Motion>(p => p.Technique.Name)
                .CanFilter();

            return mapper;
        }
    }
}
