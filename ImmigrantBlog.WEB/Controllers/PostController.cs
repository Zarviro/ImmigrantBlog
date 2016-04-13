using AutoMapper;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Enums;
using ImmigrantBlog.BLL.Infrastructure;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.WEB.Filters;
using ImmigrantBlog.WEB.Models;
using ImmigrantBlog.WEB.ModelViews;
using ImmigrantBlog.WEB.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.Controllers
{
    public class PostController : Controller
    {
        private IPostService postService;
        private IService<CategoryDTO> categoryService;
        private IService<TagDTO> tagService;
        private IUserService userService;
        private readonly int pageSize = 2;

        public PostController(IPostService postService, IService<CategoryDTO> categoryService, IService<TagDTO> tagService, IUserService userService)
        {
            this.postService = postService;
            this.categoryService = categoryService;
            this.tagService = tagService;
            this.userService = userService;
        }

        // GET: category5/5
        [LastDateUserVisit]
        public ActionResult List(string category, int page = 1)
        {
            List<PostModel> posts = Mapper.Map<IEnumerable<PostDTO>, List<PostModel>>(postService.GetPosts(category, page, pageSize));
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = postService.TotalItems
            };
            PostListModelView model = new PostListModelView()
            {
                Posts = posts,
                PagingInfo = pagingInfo,
                CurrentSearch = category
            };
            return View(model);
        }

        // GET: tag/tag5/5
        public ActionResult PostsForTag(string tag, int page = 1)
        {
            List<PostModel> posts = Mapper.Map<IEnumerable<PostDTO>, List<PostModel>>(postService.SearchPosts(tag, SearchType.Tag, page, pageSize).OrderByDescending(p => p.PostedOn));
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = postService.TotalItems
            };
            PostListModelView model = new PostListModelView()
            {
                Posts = posts,
                PagingInfo = pagingInfo,
                CurrentSearch = tag
            };
            return View("List", model);
        }

        // GET: user/login5/5
        public ActionResult PostsForUserModal(string id, int page = 1)
        {
            int userPageSize = 10;
            List<PostModel> posts = Mapper.Map<IEnumerable<PostDTO>, List<PostModel>>(postService.SearchPosts(id, SearchType.User, page, userPageSize).OrderByDescending(p => p.PostedOn));
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = userPageSize,
                TotalItems = postService.TotalItems
            };
            PostListModelView model = new PostListModelView()
            {
                Posts = posts,
                PagingInfo = pagingInfo,
                CurrentSearch = id
            };
            return View(model);
        }

        // GET: search/post5/page5
        public ActionResult Search(string search, int page = 1)
        {
            List<PostModel> posts = Mapper.Map<IEnumerable<PostDTO>, List<PostModel>>(postService.SearchPosts(search, SearchType.TitleAndCategory, page, pageSize));
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = postService.TotalItems
            };
            PostListModelView model = new PostListModelView()
            {
                Posts = posts,
                PagingInfo = pagingInfo,
                CurrentSearch = search
            };
            return View("List", model);
        }

        // GET: Post/Show/5
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostModel post = Mapper.Map<PostDTO, PostModel>(postService.GetPost(id));
            if (post == null)
            {
                return HttpNotFound();
            }
            if (NewUserHit(id))
                postService.RaiseCountHit(Convert.ToInt16(id));
            return View(post);
        }

        // GET: Post/ShowModal/5
        public ActionResult ShowModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostModel postViewModel = Mapper.Map<PostDTO, PostModel>(postService.GetPost(id));
            if (postViewModel == null)
            {
                return HttpNotFound();
            }
            return PartialView(postViewModel);
        } 

        // GET: Post/Create
        [Authorize]
        [AuthorizeClaims(IsBlocked = false)]
        public ActionResult Create()
        {
            PostEditModelView post = new PostEditModelView()
            {
                Post = new PostModel(),
                Categories = GetCategories(),
                Tags = GetTags(),
            };
            return View(post);
        }

        // GET: Post/CreateModal
        [AuthorizeRoles(Role.Admin, Role.Moderator)]
        [AuthorizeClaims(IsBlocked = false)]
        public ActionResult CreateModal()
        {
            PostEditModelView post = new PostEditModelView()
            {
                Post = new PostModel(),
                Categories = GetCategories(),
                Tags = GetTags(),
            };
            ViewBag.DialogTitle = "Новый пост";
            return PartialView(post);
        }

        // POST: Post/Create
        [Authorize]
        [HttpPost, ValidateInput(false)]
        [AuthorizeClaims(IsBlocked = false)]
        public ActionResult Create(PostEditModelView postEditModel, int[] selectedTags)
        {
            if (selectedTags != null) 
            {
                postEditModel.Post.Tags = Mapper.Map<IEnumerable<TagDTO>, List<TagModel>>(tagService.GetAll().Where(t => selectedTags.Contains(t.Id)).ToList());
                ModelState["Post.Tags"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var postDto = Mapper.Map<PostModel, PostDTO>(postEditModel.Post);
                    UserDTO userDto = userService.FindUserByLogin(User.Identity.Name);
                    postDto.User = userDto;
                    if (User.IsInRole(Role.Admin) || User.IsInRole(Role.Moderator))
                        postDto.IsPublished = true;
                    else
                        TempData["message"] = string.Format("Спасибо за публикацию! Ваш пост \"{0}\" был отправлен модератору.", postEditModel.Post.Title);
                    postService.AddPost(postDto);
                    return RedirectToAction("List");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);
                }
            }
            postEditModel.Categories = GetCategories();
            postEditModel.Tags = GetTags();
            return View(postEditModel);
        }

        // POST: Post/Create
        [Authorize]
        [HttpPost, ValidateInput(false)]
        [AuthorizeClaims(IsBlocked = false)]
        public ActionResult CreateModal(PostEditModelView postEditModel, int[] selectedTags)
        {
            if (selectedTags != null)
            {
                postEditModel.Post.Tags = Mapper.Map<IEnumerable<TagDTO>, List<TagModel>>(tagService.GetAll().Where(t => selectedTags.Contains(t.Id)).ToList());
                ModelState["Post.Tags"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var postDto = Mapper.Map<PostModel, PostDTO>(postEditModel.Post);
                    UserDTO userDto = userService.FindUserByLogin(User.Identity.Name);
                    postDto.User = userDto;
                    if (User.IsInRole(Role.Admin) || User.IsInRole(Role.Moderator))
                        postDto.IsPublished = true;
                    postService.AddPost(postDto);
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);
                }
            }
            postEditModel.Categories = GetCategories();
            postEditModel.Tags = GetTags();
            ViewBag.DialogTitle =  "Новый пост";
            return PartialView("CreateModal", postEditModel);
        }

        // GET: Post/Edit/5
        [Authorize]
        [AuthorizeClaims(IsBlocked = false)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostModel postViewModel = Mapper.Map<PostDTO, PostModel>(postService.GetPost(id));
            if (postViewModel == null)
            {
                return HttpNotFound();
            }
            if (postViewModel.User.Login == User.Identity.Name || User.IsInRole(Role.Admin) || User.IsInRole(Role.Moderator))
            {
                PostEditModelView post = new PostEditModelView()
                {
                    Post = postViewModel,
                    Categories = GetCategories(),
                    Tags = GetTags(),
                };
                return View("Create", post);
            }
            else
            {
                return RedirectToAction("Show", id);
            }
        }

        // GET: Post/EditModal/5
        [Authorize]
        [AuthorizeClaims(IsBlocked = false)]
        public ActionResult EditModal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostModel postViewModel = Mapper.Map<PostDTO, PostModel>(postService.GetPost(id));
            if (postViewModel == null)
            {
                return HttpNotFound();
            }
            if (postViewModel.User.Login == User.Identity.Name || User.IsInRole(Role.Admin) || User.IsInRole(Role.Moderator))
            {
                PostEditModelView post = new PostEditModelView()
                {
                    Post = postViewModel,
                    Categories = GetCategories(),
                    Tags = GetTags(),
                };
                ViewBag.DialogTitle = "Редактирование поста";
                return PartialView("CreateModal", post);
            }
            else
            {
                return null;
            }
        }

        // POST: Post/Edit/5
        [Authorize]
        [HttpPost, ValidateInput(false)]
        [AuthorizeClaims(IsBlocked = false)]
        public ActionResult Edit(PostEditModelView postEditModel, int[] selectedTags)
        {
            if (selectedTags != null) 
            {
                postEditModel.Post.Tags = Mapper.Map<IEnumerable<TagDTO>, List<TagModel>>(tagService.GetAll().Where(t => selectedTags.Contains(t.Id)).ToList());
                ModelState["Post.Tags"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var postDto = Mapper.Map<PostModel, PostDTO>(postEditModel.Post);
                    if (User.IsInRole(Role.Admin) || User.IsInRole(Role.Moderator))
                    {
                        postDto.IsPublished = true;
                    }
                    else
                    {
                        postDto.IsPublished = false;
                        TempData["message"] = string.Format("Ваш пост \"{0}\" был отредактирован и отправлен модератору.", postEditModel.Post.Title);
                    }
                    postService.UpdatePost(postDto);
                    return RedirectToAction("List");
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);
                }
            }
            postEditModel.Categories = GetCategories();
            postEditModel.Tags = GetTags();
            return View("Create", postEditModel);
        }

        // POST: Post/EditModal/5
        [Authorize]
        [HttpPost, ValidateInput(false)]
        [AuthorizeClaims(IsBlocked = false)]
        public ActionResult EditModal(PostEditModelView postEditModel, int[] selectedTags)
        {
            if (selectedTags != null)
            {
                postEditModel.Post.Tags = Mapper.Map<IEnumerable<TagDTO>, List<TagModel>>(tagService.GetAll().Where(t => selectedTags.Contains(t.Id)).ToList());
                ModelState["Post.Tags"].Errors.Clear();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var postDto = Mapper.Map<PostModel, PostDTO>(postEditModel.Post);
                    if (User.IsInRole(Role.Admin) || User.IsInRole(Role.Moderator))
                        postDto.IsPublished = true;
                    postService.UpdatePost(postDto);
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);
                }
            }
            postEditModel.Categories = GetCategories();
            postEditModel.Tags = GetTags();
            ViewBag.DialogTitle = "Редактирование поста";
            return PartialView("CreateModal", postEditModel);
        }

        // GET: Post/Delete/5
        [AuthorizeClaims(IsBlocked = false)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mapper.CreateMap<PostDTO, PostModel>();
            PostModel postModel = Mapper.Map<PostDTO, PostModel>(postService.GetPost(id));
            if (postModel == null)
            {
                return HttpNotFound();
            }
            return View(postModel);
        }

        // POST: Post/Delete
        [HttpPost, ActionName("Delete")]
        [AuthorizeClaims(IsBlocked = false)]
        public ActionResult DeleteConfirmed(int id)
        {
            postService.DeletePost(id);
            return RedirectToAction("Index");
        }

        // GET: Post/TagPosts/5
        [HttpGet]
        public ActionResult TagPosts(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var tagPostsDot = postService.GetPosts().ToList().Select(p => p).Where(t => t. == id).ToList();
            var tagPostsDot = from post in postService.GetPosts()
                              from tag in post.Tags
                              where tag.Id == id
                              select post;
            var posts = Mapper.Map<IEnumerable<PostDTO>, List<PostModel>>(tagPostsDot);
            return View(posts);
        }


        protected override void Dispose(bool disposing)
        {
            postService.Dispose();
            base.Dispose(disposing);
        }

        //// autocomplete
        //public ActionResult AutocompleteSearch(string term)
        //{
        //    List<TagDTO> tags = tagService.GetAll().ToList();
        //    var models = tags.Where(a => a.Name.ToLower().Contains(term.ToLower()))
        //                    .Select(a => new { value = a.Name })
        //                    .Distinct();
        //    return Json(models, JsonRequestBehavior.AllowGet);
        //}

        private bool NewUserHit(int? id)
        {
            string userIp = Request.UserHostAddress;
            if ((Dictionary<int?, List<string>>)Session["HistoryIP"] == null)
            {
                Session["HistoryIP"] = new Dictionary<int?, List<string>>();
            }
            if ((Session["HistoryIP"] as Dictionary<int?, List<string>>).ContainsKey(id))
            {
                List<string> ips = (Session["HistoryIP"] as Dictionary<int?, List<string>>)[id];
                if (!ips.Contains(userIp))
                {
                    ips.Add(userIp);
                    return true;
                }
            }
            else
            {
                (Session["HistoryIP"] as Dictionary<int?, List<string>>).Add(id, new List<string> { userIp });
                return true;
            }
            return false;
        }

        private SelectList GetCategories()
        {
            List<CategoryModel> categories = Mapper.Map<IEnumerable<CategoryDTO>, List<CategoryModel>>(categoryService.GetAll());
            SelectList categoriesList = new SelectList(categories, "Id", "Name");
            return categoriesList;
        }

        private List<SelectListItem> GetTags()
        {
            List<TagModel> tags = Mapper.Map<IEnumerable<TagDTO>, List<TagModel>>(tagService.GetAll());
            List<SelectListItem> tagsList = tags.OrderBy(t => t.Name).Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList();
            return tagsList;
        }
    }
}
