using ImmigrantBlog.BLL.BusinessModel;
using ImmigrantBlog.BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.Interfaces
{
    public interface IStatistics
    {
        int TotalPosts { get; }
        int PublishedPosts { get; }
        int NotPublishedPosts { get; }
        int TotalComments { get; }
        int PublishedComments { get; }
        int NotPublishedComments { get; }
        int TotalUsers { get; }
        int NotBlockedUsers { get; }
        int BlockedUsers { get; }
        IEnumerable<ChartItem> GetPostsCountHit(Period period, DateTime startDate, DateTime endDate);
    }
}
