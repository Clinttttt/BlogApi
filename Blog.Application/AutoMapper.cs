using AutoMapper;
using BlogApi.Application.Dtos;
using BlogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Application
{
    public class AutomapperProfile : Profile
    {
      public AutomapperProfile()
        {
            CreateMap<Post, PostDto>();
            CreateMap<Tag, TagDto>();
            CreateMap<Category, CategoryDto>();
        }

    }
}
