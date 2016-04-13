using AutoMapper;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Enums;
using ImmigrantBlog.BLL.Infrastructure;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.WEB.Filters;
using ImmigrantBlog.WEB.Models;
using ImmigrantBlog.WEB.ModelViews;
using ImmigrantBlog.WEB.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        ICommentService commentService;
        IPostService postService;
        IUserService userService;
        private readonly int pageSize = 2;

        public CommentController(ICommentService commentService, IUserService userService, IPostService postService)
        {
            this.commentService = commentService;
            this.userService = userService;
            this.postService = postService;
        }

        // GET: Comment/ShowModal/5
        public ActionResult ShowModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentModel comment = Mapper.Map<CommentDTO, CommentModel>(commentService.Get(id));
            List<CommentModel> chainOfResponse = new List<CommentModel>();

            var comments = Mapper.Map<IEnumerable<CommentDTO>, List<CommentModel>>(commentService.GetAll()).GroupBy(c => c.ResponsToCommentId);
            foreach(var group in comments)
            {
                if (group.Key == comment.ResponsToCommentId)
                {
                    foreach(var c in group)
                        chainOfResponse.Add(c);
                }
            }

            CommentEditModelView model = new CommentEditModelView()
            {
                Comment = comment,
                Responses = chainOfResponse
            };
            return View(model);
        }

        // GET: comments/user/login5/5
        public ActionResult CommentsForUserModal(string id, int page = 1)
        {
            int userPageSize = 10;
            List<CommentModel> comments = Mapper.Map<IEnumerable<CommentDTO>, List<CommentModel>>(commentService.SearchComments(id, SearchType.User, page, userPageSize).OrderByDescending(p => p.PostedOn));
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = userPageSize,
                TotalItems = commentService.TotalItems
            };
            CommentListModelView model = new CommentListModelView()
            {
                Comments = comments,
                PagingInfo = pagingInfo,
                CurrentSearch = id
            };
            return View(model);
        }

        // GET: Comment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Comment/Create
        [AuthorizeClaimsAttribute(IsBlocked = false)]
        public PartialViewResult Create(int id)
        {
            return PartialView(new CommentModel() { PostId = id });
        }

        // GET: Comment/Reply
        [AuthorizeClaimsAttribute(IsBlocked = false)]
        public PartialViewResult Reply(int postId, int responseCommentId)
        {
            return PartialView("Create", new CommentModel() { PostId = postId, ResponsToCommentId = responseCommentId });
        }

        // POST: Comment/Create
        [HttpPost]
        [AuthorizeClaimsAttribute(IsBlocked = false)]
        public ActionResult Create(CommentModel comment)
        {
            try
            {
                var commentDto = Mapper.Map<CommentModel, CommentDTO>(comment);
                UserDTO userDto = userService.FindUserByLogin(User.Identity.Name);
                commentDto.User = userDto;
                if(User.IsInRole(Role.Admin) || User.IsInRole(Role.Moderator))
                    commentDto.IsPublished = true;
                else
                    TempData["message"] = string.Format("Ваш комментарий успешно добавлен и отправлен модератору.");
                commentService.Add(commentDto);
                //return RedirectToAction("Show", "Post", comment.PostId);
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
            }
            return RedirectToAction("Show", "Post", new { id = comment.PostId });
        }

        // POST: Comment/Create
        [HttpPost]
        [AuthorizeClaimsAttribute(IsBlocked = false)]
        public void CreateModal(CommentModel comment, int postId)
        {
            var commentDto = Mapper.Map<CommentModel, CommentDTO>(comment);
            commentDto.PostedOn = DateTime.Now;
            UserDTO userDto = userService.FindUserByLogin(User.Identity.Name);
            commentDto.User = userDto;
            commentDto.PostId = postId;
            commentDto.IsPublished = (User.IsInRole(Role.Admin) || User.IsInRole(Role.Moderator)) ? true : false;
            commentService.Add(commentDto);
        }

        // GET: Comment/ReplyModal/
        public ActionResult ReplyModal(int? id)
        {
            int? responseId;
            CommentDTO commentDto = commentService.Get(id);
            if(commentDto == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (commentDto.ResponsToCommentId == null)
            {
                commentDto.ResponsToCommentId = id;
                commentService.Update(commentDto);
                responseId = id;
            }
            else
            {
                responseId = commentDto.ResponsToCommentId;
            }
            CommentModel comment = new CommentModel() { PostId = commentDto.PostId, ResponsToCommentId = responseId };
            return View(comment);
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // GET: Comment/EditModal/5
        public ActionResult EditModal(int? id)
        {
            CommentDTO commentDto = commentService.Get(id);
            if (commentDto == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            CommentModel comment = Mapper.Map<CommentDTO, CommentModel>(commentDto);
            return View(comment);
        }

        // POST: Comment/EditModal/5
        [HttpPost]
        public ActionResult EditModal(CommentModel comment)
        {
            try
            { 
                CommentDTO commentDto = Mapper.Map<CommentModel, CommentDTO>(comment);
                commentService.Update(commentDto);
            }
            catch(Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // POST: Comment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
