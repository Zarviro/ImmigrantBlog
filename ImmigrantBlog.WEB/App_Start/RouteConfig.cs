using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ImmigrantBlog.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Comments/OnlyUser",
                url: "comments/user/{id}",
                defaults: new { controller = "Comment", action = "CommentsForUserModal", page = 1 }
                );

            routes.MapRoute(
                name: "Comments/UserAndPage",
                url: "comments/user/{id}/page{page}",
                defaults: new { controller = "Comment", action = "CommentsForUserModal" },
                constraints: new { page = @"\d+" }
                );

            routes.MapRoute(
                name: "FirstRequest",
                url: "",
                defaults: new { controller = "Post", action = "List", category = (string)null, page = 1 }
                );
            routes.MapRoute(
                name: "OnlyPage",
                url: "page{page}",
                defaults: new { controller = "Post", action = "List", category = (string)null },
                constraints: new { page = @"\d+" }
                );

            routes.MapRoute(
                name: "OnlyCategory",
                url: "{category}",
                defaults: new { controller = "Post", action = "List", page = 1}
                );

            routes.MapRoute(
                name: "CategoryAndPage",
                url: "{category}/page{page}",
                defaults: new { controller = "Post", action = "List" },
                constraints: new { page = @"\d+" }
                );

            routes.MapRoute(
                name: "OnlySearch",
                url: "search/{search}",
                defaults: new { controller = "Post", action = "Search", page = 1 }
                );

            routes.MapRoute(
                name: "SearchAndPage",
                url: "search/{search}/page{page}",
                defaults: new { controller = "Post", action = "Search" },
                constraints: new { page = @"\d+" }
                );

            routes.MapRoute(
                name: "OnlyTag",
                url: "tag/{tag}",
                defaults: new { controller = "Post", action = "PostsForTag", page = 1 }
                );

            routes.MapRoute(
                name: "TagAndPage",
                url: "tag/{tag}/page{page}",
                defaults: new { controller = "Post", action = "PostsForTag" },
                constraints: new { page = @"\d+" }
                );

            routes.MapRoute(
                name: "OnlyUser",
                url: "user/{login}",
                defaults: new { controller = "Post", action = "PostsForUserModal", page = 1 }
                );

            routes.MapRoute(
                name: "UserAndPage",
                url: "user/{login}/page{page}",
                defaults: new { controller = "Post", action = "PostsForUserModal" },
                constraints: new { page = @"\d+" }
                );

            routes.MapRoute(
                name: "Post",
                url: "post/{id}",
                defaults: new { controller = "Post", action = "Show" },
                constraints: new { id = @"\d+" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Post", action = "Details", id = UrlParameter.Optional }
                );
        }
    }
}
