using AutoMapper;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.WEB.Models;
using ImmigrantBlog.WEB.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImmigrantBlog.WEB
{
    public class AutoMapperConfig
    {
        //.ForMember(p => p.CategoryName, opt => opt.MapFrom(c => c.Category.Name));
        //.ForMember(p => p.TagsName, opt => opt.MapFrom(t => t.Tags.Select(y => y.Name).ToList()))
        //.ForMember(p => p.Tags, opt => opt.Ignore())

        public static void RegisterMappings()
        {
            // DTO to Model
            Mapper.CreateMap<PostDTO, PostModel>();
            Mapper.CreateMap<TagDTO, TagModel>();
            Mapper.CreateMap<CategoryDTO, CategoryModel>();
            Mapper.CreateMap<CommentDTO, CommentModel>();
            Mapper.CreateMap<PostCountHitDTO, PostCountHitModel>();
            Mapper.CreateMap<UserDTO, UserModel>();
            
            // Model to DTO
            Mapper.CreateMap<PostModel, PostDTO>();
            Mapper.CreateMap<TagModel, TagDTO>();
            Mapper.CreateMap<CategoryModel, CategoryDTO>();
            Mapper.CreateMap<CommentModel, CommentDTO>();
            Mapper.CreateMap<UserModel, UserDTO>();
        }
    }
}