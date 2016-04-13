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
    public class CategoryService : IService<CategoryDTO>
    {
        IUnitOfWork Database { get; set; }

        public CategoryService(IUnitOfWork uow)
        {
            Database = uow;
            AutoMapperConfig.RegisterMappings();
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            List<Category> categoriesDb = Database.Categories.GetAll().ToList();
            var categories = Mapper.Map<IEnumerable<Category>, List<CategoryDTO>>(categoriesDb);
            return categories;
        }

        public CategoryDTO Get(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Не установлен id категории", "");
            }
            var category = Database.Categories.Get(id.Value);
            if (category == null)
            {
                throw new ValidationException("Категория не найдена", "");
            }
            Mapper.CreateMap<Category, CategoryDTO>();
            return Mapper.Map<Category, CategoryDTO>(category);
        }

        public void Add(CategoryDTO categoryDto)
        {
            Category category = Mapper.Map<CategoryDTO, Category>(categoryDto);
            if (category == null || category.Name == null || category.Name == "")
            {
                throw new ValidationException("*Категирия не задана...", "");
            }
            category.UrlSlug = StringHelper.ConvertToUrlSlug(categoryDto.Name);
            if (Database.Categories.Find(c => c.UrlSlug == category.UrlSlug).Count() > 0)
            {
                throw new ValidationException("*Категирия: " + category.Name + " уже сучествует", "");
            }
            Database.Categories.Create(category);
            Database.Save();
        }

        public void Update(CategoryDTO categoryDto)
        {
            Category category = Database.Categories.Get(categoryDto.Id);
            if (category == null || categoryDto.Name == null || categoryDto.Name == "")
            {
                throw new ValidationException("*Категирия не задана...", "");
            }
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            string urlSlug = StringHelper.ConvertToUrlSlug(categoryDto.Name);
            if (Database.Categories.Find(c => c.UrlSlug == urlSlug).Count() > 0)
            {
                throw new ValidationException("*Категирия: " + category.Name + " уже сучествует", "");
            }
            category.UrlSlug = urlSlug;
            Database.Categories.Update(category);
            Database.Save();
        }

        public void Delete(int id)
        {
            Database.Categories.Delete(id);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
