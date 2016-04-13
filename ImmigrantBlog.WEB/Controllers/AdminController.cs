using AutoMapper;
using ImmigrantBlog.BLL.DTO;
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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.Controllers
{
    [AuthorizeRoles(Role.Admin, Role.Moderator)]
    public class AdminController : Controller
    {
        IStatistics statistics;

        public AdminController(IStatistics statistics)
        {
            this.statistics = statistics;
        }

        // GET: Admin/Console
        public ActionResult Console()
        {
            return View();
        }


        // GET: Admin/PostList
        public ActionResult PostList()
        {
            return View("ConsolePostList");
        }

        // GET: Admin/CommentList
        public ActionResult CommentList()
        {
            return View("ConsoleCommentList");
        }

        // GET: Admin/CategoryList
        [AuthorizeRoles(Role.Admin)]
        public ActionResult CategoryList()
        {
            return View("ConsoleCategoryList");
        }

        // GET: Admin/TagList
        [AuthorizeRoles(Role.Admin)]
        public ActionResult TagList()
        {
            return View("ConsoleTagList");
        }

        // GET: Admin/Statistics
        [AuthorizeRoles(Role.Admin)]
        public ActionResult StatisticsList()
        {
            ViewBag.PostStatistics = string.Format("{0} ({1}/{2})", statistics.TotalPosts, statistics.PublishedPosts, statistics.NotPublishedPosts);
            ViewBag.CommentStatistics = string.Format("{0} ({1}/{2})", statistics.TotalComments, statistics.PublishedComments, statistics.NotPublishedComments);
            ViewBag.UserStatistics = string.Format("{0} ({1}/{2})", statistics.TotalUsers, statistics.NotBlockedUsers, statistics.BlockedUsers);
            return View("ConsoleStatisticsList");
        }

        // GET: Admin/UserList
        [AuthorizeRoles(Role.Admin)]
        public ActionResult UserList()
        {
            return View("ConsoleUserList");
        }

        // GET: Admin/Setings
        [AuthorizeRoles(Role.Admin)]
        public ActionResult Setings()
        {
            return View("ConsoleSetings");
        }


        // GET: Admin/CreateChart
        public ActionResult CreateChart()
        {
            var chart = new System.Web.Helpers.Chart(width: 700, height: 300)
                  .AddTitle("График посещений")
                  .AddSeries(
                         name: "Моя программа",
                         legend: "Моя программа",
                         chartType: "Line",
                         xValue: new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" },
                         yValues: new[] { "2", "6", "4", "5", "3" })
                  .Write();

            return null;
        }
    }
}