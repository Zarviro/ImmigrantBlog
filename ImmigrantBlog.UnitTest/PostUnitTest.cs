using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ImmigrantBlog.BLL.Interfaces;
using System.Collections.Generic;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.WEB.Controllers;
using ImmigrantBlog.WEB.Models;
using ImmigrantBlog.DAL.Interfaces;
using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.BLL.Services;
using System.Web.Mvc;
using ImmigrantBlog.WEB.HtmlHelpers;

namespace ImmigrantBlog.UnitTest
{
    [TestClass]
    public class PostUnitTest
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IUnitOfWork> mock = new Mock<IUnitOfWork>();
            mock.Setup(m => m.Posts.GetAll()).Returns(new List<Post>
                {
                    new Post { Id = 1, Title = "Post1", PostedOn = Convert.ToDateTime("05.01.2000")},
                    new Post { Id = 2, Title = "Post2", PostedOn = Convert.ToDateTime("04.01.2000")},
                    new Post { Id = 3, Title = "Post3", PostedOn = Convert.ToDateTime("03.01.2000")},
                    new Post { Id = 4, Title = "Post4", PostedOn = Convert.ToDateTime("02.01.2000")},
                    new Post { Id = 5, Title = "Post5", PostedOn = Convert.ToDateTime("01.01.2000")}
                });
            PostService postService = new PostService(mock.Object);

            // Действие (act)
            List<PostDTO> result = (List<PostDTO>)postService.GetPosts(null, 2, 3);

            // Утверждение (assert)
            List<PostDTO> posts = result;
            Assert.IsTrue(posts.Count == 2);
            Assert.AreEqual(posts[0].Title, "Post2");
            Assert.AreEqual(posts[1].Title, "Post1");
        }

        [TestMethod]
        public void Can_Generate_Start_Page_Links()
        {
            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 1,
                TotalItems = 20,
                ItemsPerPage = 2
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(
                @"<a class=""btn btn-mini btn-primary selected"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-mini"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-mini"" href=""Page3"">3</a>"
                + @"<a class=""btn btn-mini"" href=""Page4"">4</a>"
                + @"<a class=""btn btn-mini"" href=""Page5"">5</a>"
                + @"<a class=""btn btn-small"" href=""Page10"">>></a>"
                ,
                result.ToString());
        }

        [TestMethod]
        public void Can_Generate_Prev_Page_Link()
        {
            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PrevPage(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1""><</a>",result.ToString());
        }

        [TestMethod]
        public void Can_Generate_Next_Page_Link()
        {
            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.NextPage(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page3"">></a>", result.ToString());
        }


    }
}
