using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ImmigrantBlog.WEB.Models;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Infrastructure;
using System.Collections.Generic;
using ImmigrantBlog.WEB.ModelViews;
using AutoMapper;
using System.Net;

namespace ImmigrantBlog.WEB.Controllers
{
    public class AccountController : Controller
    {
        private IUserService userService;
        //
        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login()
        {
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModelView model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Login = model.Login, Password = model.Password};
                ClaimsIdentity claim = await userService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("List", "Post");
                }
            }
            return View(model);
        }
 
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("List", "Post");
        }
 
        public ActionResult Register()
        {
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModelView model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Login = model.Login,
                    Password = model.Password,
                    Roles = new List<string>(){"user"},
                    Name = model.Name,
                    Surname = model.Surname,
                    Country = model.Country,
                    City = model.City,
                    LastIP = Request.UserHostAddress,
                    DateRegister = DateTime.Now,
                    DateLastVisit = DateTime.Now
                };
                OperationDetails operationDetails = await userService.CreateAsync(userDto);
                if (operationDetails.Succedeed)
                    return View("SuccessRegister");
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        public async Task<JsonResult> CheckLogin(string login)
        {
            UserDTO user = await userService.FindUserByLoginAsync(login);
            bool result = user == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> CheckEmail(string email)
        {
            UserDTO user = await userService.FindUserByEmailAsync(email);
            bool result = user == null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowInfoModal(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModel user = Mapper.Map<UserDTO, UserModel>(userService.GetUser(id));
            return View(user);
        }

        public ActionResult EditRoles(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModel userModel = Mapper.Map<UserDTO, UserModel>(userService.GetUser(id));
            List<string> allRoles = userService.GetAllRoles().OrderBy(r => r).ToList();
            IEnumerable<SelectListItem> roles = allRoles.Select(x => new SelectListItem()
            {
                Selected = userModel.Roles.Contains(x),
                Text = x,
                Value = x
            });
            UserEditModelView userEditModelView = new UserEditModelView()
            {
                User = userModel,
                Roles = roles
            };
            return View(userEditModelView);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UserEdit(UserEditModelView userViewModel, string[] selectedRoles)
        {
            UserDTO userDto = userService.GetUser(userViewModel.User.Id);
            if (selectedRoles != null)
                userDto.Roles = selectedRoles.ToList();
            OperationDetails operationDetails =  userService.UpdateUser(userDto);
            if (operationDetails.Succedeed)
                return View("ShowInfoModal", Mapper.Map<UserDTO, UserModel>(userDto));
            else
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        //// GET: Admin/UserEdit/string
        //public ActionResult UserEdit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    UserModel userModel = Mapper.Map<UserDTO, UserModel>(userService.GetUser(id));
        //    List<string> allRoles = userService.GetAllRoles().OrderBy(r => r).ToList();
        //    IEnumerable<SelectListItem> roles = allRoles.Select(x => new SelectListItem()
        //        {
        //            Selected = userModel.Roles.Contains(x),
        //            Text = x,
        //            Value = x
        //        });
        //    UserEditModelView userEditModelView = new UserEditModelView()
        //    {
        //        User = userModel,
        //        Roles = roles
        //    };
        //    return View(userEditModelView);
        //}

        //// POST: Admin/UserEdit
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> UserEdit(UserEditModelView userViewModel, string[] selectedRoles)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        UserDTO userDto = userService.GetUser(userViewModel.User.Id);
        //        userDto.Name = userViewModel.User.Name;
        //        userDto.Surname = userViewModel.User.Surname;
        //        userDto.Country = userViewModel.User.Country;
        //        userDto.City = userViewModel.User.City;
        //        userDto.IsBlocked = userViewModel.User.IsBlocked;
        //        if (selectedRoles != null)
        //            userDto.Roles = selectedRoles.ToList();
        //        OperationDetails operationDetails = await userService.UpdateUserAsync(userDto);
        //        if (operationDetails.Succedeed)
        //            return RedirectToAction("UserList");
        //        else
        //            ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
        //    }
        //    return RedirectToAction("UserList");
        //}
    }
}