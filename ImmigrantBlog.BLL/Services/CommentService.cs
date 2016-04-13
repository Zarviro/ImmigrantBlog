using AutoMapper;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Enums;
using ImmigrantBlog.BLL.Infrastructure;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImmigrantBlog.BLL.Services
{
    public class CommentService : ICommentService
    {
        IUnitOfWork Database { get; set; }

        public CommentService(IUnitOfWork uow)
        {
            Database = uow;
            AutoMapperConfig.RegisterMappings();
        }

        public int TotalItems { get; private set; }

        public CommentDTO Get(int? id)
        {
            if (id == null)
            {
                throw new ValidationException("Не установлено id комментария", "");
            }
            var comment = Database.Comments.Get(id.Value);
            if (comment == null)
            {
                throw new ValidationException("Комментарий не найден", "");
            }
            return Mapper.Map<Comment, CommentDTO>(comment);
        }

        public IEnumerable<CommentDTO> GetAll()
        {
            List<Comment> commentsDb = Database.Comments.GetAll().ToList();
            List<CommentDTO> comments = Mapper.Map<IEnumerable<Comment>, List<CommentDTO>>(commentsDb);
            return comments;
        }

        public IEnumerable<CommentDTO> SearchComments(string search, SearchType searchType, int pageNo, int pageSize)
        {
            List<Comment> commentsDb = new List<Comment>();
            switch (searchType)
            {
                case SearchType.User:
                    commentsDb = Database.Comments.Find(c => c.UserId == search).ToList();
                    break;
                default:
                    break;
            }
            TotalItems = commentsDb.Count();
            commentsDb = commentsDb
                .OrderByDescending(c => c.PostedOn)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            List<CommentDTO> comments = Mapper.Map<IEnumerable<Comment>, List<CommentDTO>>(commentsDb);
            return comments;
        }

        public void Add(CommentDTO commentDto)
        {
            Comment comment = Mapper.Map<CommentDTO, Comment>(commentDto);
            comment.PostedOn = DateTime.Now;
            Database.Comments.Create(comment);
            Database.Save();
            if(commentDto.ResponsToCommentId == null)
            {
                Comment commentUpdate = Database.Comments.Find(c => c.Description == comment.Description && c.PostedOn == comment.PostedOn).LastOrDefault();
                commentUpdate.ResponsToCommentId = commentUpdate.Id;
                Database.Comments.Update(commentUpdate);
                Database.Save();
            }
        }

        public void Update(CommentDTO commentDTO)
        {
            Comment comment = Database.Comments.Get(commentDTO.Id);
            comment.Description = commentDTO.Description;
            comment.IsPublished = commentDTO.IsPublished;
            Database.Comments.Update(comment);
            Database.Save();
        }

        public void Delete(int id)
        {
            Database.Comments.Delete(id);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
