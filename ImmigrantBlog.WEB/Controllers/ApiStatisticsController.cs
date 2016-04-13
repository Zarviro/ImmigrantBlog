using ImmigrantBlog.BLL.BusinessModel;
using ImmigrantBlog.BLL.Enums;
using ImmigrantBlog.BLL.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ImmigrantBlog.WEB.Controllers
{
    public class ApiStatisticsController : ApiController
    {
        private IStatistics statistics;

        [Inject]
        public IStatistics Statistics
        {
            get { return this.statistics; }
            set { this.statistics = value; }
        }

        // GET: api/ApiStatistics/SetPublished/5
        [Route("api/ApiStatistics/blog")]
        public List<ChartItem> Get(Period period, DateTime startDate, DateTime endDate)
        {
            return statistics.GetPostsCountHit(period, startDate, endDate).ToList();
        }



    }
}


