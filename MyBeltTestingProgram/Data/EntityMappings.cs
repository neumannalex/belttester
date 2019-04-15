using AutoMapper;
using MyBeltTestingProgram.Data.Models;
using MyBeltTestingProgram.Entities.Combination;
using MyBeltTestingProgram.Entities.Move;
using MyBeltTestingProgram.Entities.Stance;
using MyBeltTestingProgram.Entities.Technique;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeltTestingProgram.Data
{
    public class EntityMappings : Profile
    {
        public EntityMappings()
        {
            CreateMap<Stance, StanceDTO>().ReverseMap();
            CreateMap<Stance, StanceDTOForCreation>().ReverseMap();
            CreateMap<Stance, StanceDTOForUpdate>().ReverseMap();

            CreateMap<Move, MoveDTO>().ReverseMap();
            CreateMap<Move, MoveDTOForCreation>().ReverseMap();
            CreateMap<Move, MoveDTOForUpdate>().ReverseMap();

            CreateMap<Technique, TechniqueDTO>().ReverseMap();
            CreateMap<Technique, TechniqueDTOForCreation>().ReverseMap();
            CreateMap<Technique, TechniqueDTOForUpdate>().ReverseMap();

            CreateMap<Combination, CombinationDTO>().ReverseMap();
        }
    }
}
