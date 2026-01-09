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
            CreateMap<Post, PostDto>()
                .ForMember(d => d.CommentCount, opt => opt.MapFrom(s => s.Comments.Count))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Category.Name))
                .ForMember(d => d.Author, opt => opt.MapFrom(s =>
                    s.User.UserInfo != null && !string.IsNullOrEmpty(s.User.UserInfo.FullName)
                        ? s.User.UserInfo.FullName
                        : s.User.UserName))
                .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.PostTags.Select(pt => new TagDto
                {
                    Id = pt.TagId,
                    Name = pt.tag!.Name,
                    PostId = pt.PostId,
                    TagCount = pt.tag.PostTags.Count()
                })))
                .ForMember(d => d.IsBookMark, opt => opt.MapFrom(s => s.BookMarks.Any()))
                .ForMember(d => d.PostLike, opt => opt.MapFrom(s => s.PostLikes.Count()))
              
                .ForMember(d => d.PhotoIsliked, opt => opt.Ignore())
                .ForMember(d => d.Preview, opt => opt.Ignore());


            CreateMap<Tag, TagDto>();
            CreateMap<Category, CategoryDto>();

        }

    }
}
