using BeltTester.Data.Entities;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Services
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(IOptions<SieveOptions> options, ISieveCustomSortMethods customSortMethods, ISieveCustomFilterMethods customFilterMethods) : base(options, customSortMethods, customFilterMethods)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            #region Motion
            mapper.Property<Motion>(p => p.Move.Name)
                .HasName("MoveName")
                .CanFilter();

            mapper.Property<Motion>(p => p.Move.Symbol)
                .HasName("MoveSymbol")
                .CanFilter();

            mapper.Property<Motion>(p => p.Stance.Name)
                .HasName("StanceName")
                .CanFilter();

            mapper.Property<Motion>(p => p.Stance.Symbol)
                .HasName("StanceSymbol")
                .CanFilter();

            mapper.Property<Motion>(p => p.Technique.Name)
                .HasName("TechniqueName")
                .CanFilter();
            #endregion

            return mapper;
        }
    }
}
