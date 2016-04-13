using AutoMapper;
using ImmigrantBlog.BLL.BusinessModel;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Infrastructure;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.Services
{
    public class TagService : IService<TagDTO>
    {
        IUnitOfWork Database { get; set; }

        public TagService(IUnitOfWork uow)
        {
            Database = uow;
            AutoMapperConfig.RegisterMappings();
        }

        public IEnumerable<TagDTO> GetAll()
        {
            List<Tag> tagsDb = Database.Tags.GetAll().ToList();
            var tags = Mapper.Map<IEnumerable<Tag>, List<TagDTO>>(tagsDb);
            return tags;
        }

        public TagDTO Get(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Не установлен id тэга", "");
            }
            var tag = Database.Tags.Get(id.Value);
            if (tag == null)
            {
                throw new ValidationException("Тэг не найден", "");
            }
            Mapper.CreateMap<Tag, TagDTO>();
            return Mapper.Map<Tag, TagDTO>(tag);
        }

        public void Add(TagDTO tagDto)
        {
            Tag tag = Mapper.Map<TagDTO, Tag>(tagDto);
            if (tag == null || tag.Name == null || tag.Name == "")
            {
                throw new ValidationException("*Тэг не задан...", "");
            }
            tag.UrlSlug = StringHelper.ConvertToUrlSlug(tagDto.Name);
            if (Database.Tags.Find(c => c.UrlSlug == tag.UrlSlug).Count() > 0)
            {
                throw new ValidationException("*Тэг: " + tag.Name + " уже сучествует", "");
            }
            Database.Tags.Create(tag);
            Database.Save();
        }

        public void Update(TagDTO tagDto)
        {
            Tag tag = Database.Tags.Get(tagDto.Id);
            if (tag == null || tagDto.Name == null || tagDto.Name == "")
            {
                throw new ValidationException("*Тэг не задан...", "");
            }
            tag.Name = tagDto.Name;
            tag.Description = tagDto.Description;
            string urlSlug = StringHelper.ConvertToUrlSlug(tagDto.Name);
            if (Database.Tags.Find(c => c.UrlSlug == urlSlug).Count() > 0)
            {
                throw new ValidationException("*Тэг: " + tag.Name + " уже сучествует", "");
            }
            tag.UrlSlug = urlSlug;
            Database.Tags.Update(tag);
            Database.Save();
        }

        public void Delete(int id)
        {
            Database.Tags.Delete(id);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
