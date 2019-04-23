using BeltTester.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Data
{
    public class BeltTesterDBContext : IdentityDbContext<AppUser>
    {
        public BeltTesterDBContext(DbContextOptions<BeltTesterDBContext> options) : base(options)
        {
        }

        public DbSet<BeltTestProgram> BeltTestPrograms { get; set; }
        public DbSet<Combination> Combinations { get; set; }
        public DbSet<Motion> Motions { get; set; }
        public DbSet<Stance> Stances { get; set; }
        public DbSet<Move> Moves { get; set; }
        public DbSet<Technique> Techniques { get; set; }
    }
}
