using AutoMapper;
using BeltTester.Data.Entities;
using BeltTester.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeltTester.Services
{
    public class EntityMappings : Profile
    {

        public EntityMappings()
        {
            //CreateMap<Stance, StanceDTO>().ReverseMap();
            //CreateMap<Stance, StanceDTOForCreation>().ReverseMap();
            //CreateMap<Stance, StanceDTOForUpdate>().ReverseMap();

            //CreateMap<Move, MoveDTO>().ReverseMap();
            //CreateMap<Move, MoveDTOForCreation>().ReverseMap();
            //CreateMap<Move, MoveDTOForUpdate>().ReverseMap();

            CreateMap<Technique, TechniqueDTO>().ReverseMap();
            CreateMap<Technique, TechniqueDTOForCreation>().ReverseMap();
            CreateMap<Technique, TechniqueDTOForUpdate>().ReverseMap();

            //CreateMap<Motion, MotionDTO>().ReverseMap();

            //CreateMap<Combination, CombinationDTO>().ReverseMap();
        }
    }
}
