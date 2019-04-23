using BeltTester.Data.Entities;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Services
{
    public class SieveCustomFilterMethods : ISieveCustomFilterMethods
    {
        public IQueryable<Motion> TechniqueWeapon(IQueryable<Motion> source, string op, string[] values)
        {
            if (values.Length <= 0)
                return source;

            string value = values[0];

            switch (op)
            {
                case "==":
                    return source.Where(x => x.Technique.Weapon.ToString() == value);
                case "==*":
                    return source.Where(x => x.Technique.Weapon.ToString().ToLowerInvariant() == value.ToLowerInvariant());
                case "!=":
                    return source.Where(x => x.Technique.Weapon.ToString() != value);
                case "@=":
                    return source.Where(x => x.Technique.Weapon.ToString().Contains(value));
                case "_=":
                    return source.Where(x => x.Technique.Weapon.ToString().StartsWith(value));
                case "!@=":
                    return source.Where(x => !x.Technique.Weapon.ToString().Contains(value));
                case "!_=":
                    return source.Where(x => !x.Technique.Weapon.ToString().StartsWith(value));
                case "@=*":
                    return source.Where(x => x.Technique.Weapon.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
                case "_=*":
                    return source.Where(x => x.Technique.Weapon.ToString().ToLowerInvariant().StartsWith(value.ToLowerInvariant()));
                case "!@=*":
                    return source.Where(x => x.Technique.Weapon.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
                case "!_=*":
                    return source.Where(x => !x.Technique.Weapon.ToString().ToLowerInvariant().StartsWith(value.ToLowerInvariant()));
                default:
                    return source.Where(x => x.Technique.Weapon.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
            }
        }

        public IQueryable<Motion> TechniquePurpose(IQueryable<Motion> source, string op, string[] values)
        {
            if (values.Length <= 0)
                return source;

            string value = values[0];

            switch (op)
            {
                case "==":
                    return source.Where(x => x.Technique.Purpose.ToString() == value);
                case "==*":
                    return source.Where(x => x.Technique.Purpose.ToString().ToLowerInvariant() == value.ToLowerInvariant());
                case "!=":
                    return source.Where(x => x.Technique.Purpose.ToString() != value);
                case "@=":
                    return source.Where(x => x.Technique.Purpose.ToString().Contains(value));
                case "_=":
                    return source.Where(x => x.Technique.Purpose.ToString().StartsWith(value));
                case "!@=":
                    return source.Where(x => !x.Technique.Purpose.ToString().Contains(value));
                case "!_=":
                    return source.Where(x => !x.Technique.Purpose.ToString().StartsWith(value));
                case "@=*":
                    return source.Where(x => x.Technique.Purpose.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
                case "_=*":
                    return source.Where(x => x.Technique.Purpose.ToString().ToLowerInvariant().StartsWith(value.ToLowerInvariant()));
                case "!@=*":
                    return source.Where(x => x.Technique.Purpose.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
                case "!_=*":
                    return source.Where(x => !x.Technique.Purpose.ToString().ToLowerInvariant().StartsWith(value.ToLowerInvariant()));
                default:
                    return source.Where(x => x.Technique.Purpose.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
            }
        }

        public IQueryable<Technique> Weapon(IQueryable<Technique> source, string op, string[] values)
        {
            if (values.Length <= 0)
                return source;

            string value = values[0];

            switch (op)
            {
                case "==":
                    return source.Where(x => x.Weapon.ToString() == value);
                case "==*":
                    return source.Where(x => x.Weapon.ToString().ToLowerInvariant() == value.ToLowerInvariant());
                case "!=":
                    return source.Where(x => x.Weapon.ToString() != value);
                case "@=":
                    return source.Where(x => x.Weapon.ToString().Contains(value));
                case "_=":
                    return source.Where(x => x.Weapon.ToString().StartsWith(value));
                case "!@=":
                    return source.Where(x => !x.Weapon.ToString().Contains(value));
                case "!_=":
                    return source.Where(x => !x.Weapon.ToString().StartsWith(value));
                case "@=*":
                    return source.Where(x => x.Weapon.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
                case "_=*":
                    return source.Where(x => x.Weapon.ToString().ToLowerInvariant().StartsWith(value.ToLowerInvariant()));
                case "!@=*":
                    return source.Where(x => x.Weapon.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
                case "!_=*":
                    return source.Where(x => !x.Weapon.ToString().ToLowerInvariant().StartsWith(value.ToLowerInvariant()));
                default:
                    return source.Where(x => x.Weapon.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
            }
        }

        public IQueryable<Technique> Purpose(IQueryable<Technique> source, string op, string[] values)
        {
            if (values.Length <= 0)
                return source;

            string value = values[0];

            switch (op)
            {
                case "==":
                    return source.Where(x => x.Purpose.ToString() == value);
                case "==*":
                    return source.Where(x => x.Purpose.ToString().ToLowerInvariant() == value.ToLowerInvariant());
                case "!=":
                    return source.Where(x => x.Purpose.ToString() != value);
                case "@=":
                    return source.Where(x => x.Purpose.ToString().Contains(value));
                case "_=":
                    return source.Where(x => x.Purpose.ToString().StartsWith(value));
                case "!@=":
                    return source.Where(x => !x.Purpose.ToString().Contains(value));
                case "!_=":
                    return source.Where(x => !x.Purpose.ToString().StartsWith(value));
                case "@=*":
                    return source.Where(x => x.Purpose.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
                case "_=*":
                    return source.Where(x => x.Purpose.ToString().ToLowerInvariant().StartsWith(value.ToLowerInvariant()));
                case "!@=*":
                    return source.Where(x => x.Purpose.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
                case "!_=*":
                    return source.Where(x => !x.Purpose.ToString().ToLowerInvariant().StartsWith(value.ToLowerInvariant()));
                default:
                    return source.Where(x => x.Purpose.ToString().ToLowerInvariant().Contains(value.ToLowerInvariant()));
            }
        }
    }
}
