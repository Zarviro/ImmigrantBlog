using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImmigrantBlog.BLL.Interfaces;
using Moq;
using ImmigrantBlog.BLL.DTO;
using System.Collections.Generic;
using ImmigrantBlog.WEB.Controllers;
using ImmigrantBlog.WEB.Models;

namespace ImmigratBlog.UnitTests
{
    [TestClass]
    public class PostUnitTest
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IPostService> mock = new Mock<IPostService>();
            mock.Setup(m => m.GetPosts()).Returns(new List<PostDTO>
                {
                    new PostDTO { Id = 1, Title = "Post1"},
                    new PostDTO { Id = 2, Title = "Post2"},
                    new PostDTO { Id = 3, Title = "Post3"},
                    new PostDTO { Id = 4, Title = "Post4"},
                    new PostDTO { Id = 5, Title = "Post5"}
                });
            PostController controller = new PostController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            List<PostViewModel> result = (List<PostViewModel>)controller.List(2).Model;

            // Утверждение (assert)
            List<PostViewModel> posts = result;
            Assert.IsTrue(posts.Count == 2);
        }
    }
}
