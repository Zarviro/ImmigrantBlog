using ImmigrantBlog.BLL.BusinessModel;
using ImmigrantBlog.BLL.DTO;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.BLL.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.Util
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IPostService>().To<PostService>();
            kernel.Bind<IService<CategoryDTO>>().To<CategoryService>();
            kernel.Bind<IService<TagDTO>>().To<TagService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<ICommentService>().To<CommentService>();
            kernel.Bind<IStatistics>().To<Statistics>();
        }
    }
}