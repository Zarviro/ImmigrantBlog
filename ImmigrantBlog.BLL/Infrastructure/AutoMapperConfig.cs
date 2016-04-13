using AutoMapper;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.Infrastructure
{
    class AutoMapperConfig
    {
        //.ForMember(p => p.CategoryName, opt => opt.MapFrom(c => c.Category.Name));
        //.ForMember(p => p.TagsName, opt => opt.MapFrom(t => t.Tags.Select(y => y.Name).ToList()))
        //.ForMember(p => p.Tags, opt => opt.Ignore())

        public static void RegisterMappings()
        {
            // DAL to DTO
            Mapper.CreateMap<Category, CategoryDTO>();
            Mapper.CreateMap<Tag, TagDTO>();
            Mapper.CreateMap<Post, PostDTO>();
            Mapper.CreateMap<Comment, CommentDTO>();
            Mapper.CreateMap<PostCountHit, PostCountHitDTO>();
            Mapper.CreateMap<User, UserDTO>()
                .ForMember(u => u.Login, opt => opt.MapFrom(u => u.ApplicationUser.UserName))
                .ForMember(u => u.Email, opt => opt.MapFrom(u => u.ApplicationUser.Email));

            // DTO to DAL
            Mapper.CreateMap<CategoryDTO, Category>();
            Mapper.CreateMap<TagDTO, Tag>();
            Mapper.CreateMap<PostDTO, Post>()
                .ForMember(c => c.User, opt => opt.Ignore())
                .ForMember(c => c.Category, opt => opt.Ignore())
                .ForMember(c => c.Tags, opt => opt.Ignore());
            Mapper.CreateMap<UserDTO, User>()
                .ForMember(u => u.ApplicationUser, opt => opt.Ignore());
            Mapper.CreateMap<CommentDTO, Comment>()
                .ForMember(c => c.User, opt => opt.Ignore())
                .ForMember(p => p.Post, opt => opt.Ignore());
            Mapper.CreateMap<PostCountHitDTO, PostCountHit>()
                .ForMember(p => p.Post, opt => opt.Ignore());
        }
    }
}
