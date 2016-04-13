using ImmigrantBlog.BLL.Enums;
using ImmigrantBlog.BLL.Infrastructure;
using ImmigrantBlog.BLL.Interfaces;
using ImmigrantBlog.DAL.Entities;
using ImmigrantBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmigrantBlog.BLL.BusinessModel
{
    public class Statistics : IStatistics
    {
        IUnitOfWork Database { get; set; }

        public Statistics(IUnitOfWork uow)
        {
            Database = uow;
            AutoMapperConfig.RegisterMappings();
        }

        public int TotalPosts 
        {
            get { return Database.Posts.GetAll().Count(); }
        }

        public int PublishedPosts
        {
            get { return Database.Posts.Find(p => p.IsPublished == true).Count(); }
        }

        public int NotPublishedPosts 
        {
            get { return Database.Posts.Find(p => p.IsPublished == false).Count(); }
        }

        public int TotalComments 
        {
            get { return Database.Comments.GetAll().Count(); }
        }

        public int PublishedComments 
        {
            get { return Database.Comments.Find(c => c.IsPublished == true).Count(); }
        }

        public int NotPublishedComments 
        {
            get { return Database.Comments.Find(c => c.IsPublished == false).Count(); }
        }

        public int TotalUsers 
        {
            get { return Database.UserManager.GetAll().Count(); }
        }

        public int NotBlockedUsers
        {
            get { return Database.UserManager.Find(u => u.IsBlocked == false).Count(); }
        }

        public int BlockedUsers
        {
            get { return Database.UserManager.Find(u => u.IsBlocked == true).Count(); }
        }


        public IEnumerable<ChartItem> GetPostsCountHit(Period period, DateTime startDate, DateTime endDate)
        {
            if (startDate == null || endDate == null || startDate > endDate)
                return null;
            List<ChartItem> items = new List<ChartItem>();
            List<PostCountHit> c = Database.PostCountHits.Find(h => h.Date.Date >= startDate.Date && h.Date.Date <= endDate.Date).ToList();
            DateTime currentStartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            DateTime currentEndDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            currentEndDate = DateAddPeriod(period, currentEndDate).AddDays(-1);
            while (currentStartDate <= endDate)
            {
                int count = c.Where(h => h.Date.Date >= currentStartDate.Date && h.Date.Date <= currentEndDate.Date).Select(h => h.Count).Sum();
                string title = currentStartDate.ToString("dd.MM.yy") + "-" + (currentEndDate <= endDate ? currentEndDate.ToString("dd.MM.yy") : endDate.ToString("dd.MM.yy"));
                items.Add(new ChartItem { Title = title, Value = count });
                currentStartDate = DateAddPeriod(period, currentStartDate);
                currentEndDate = DateAddPeriod(period, currentEndDate);
            }
            return items;
        }

        private DateTime DateAddPeriod(Period period, DateTime date)
        {
            switch(period)
            {
                case Period.Days:
                    date.AddDays(1);
                    break;
                case Period.Weeks:
                    date = date.AddDays(7);
                     break;
                case Period.Months:
                    date.AddMonths(1);
                    break;
                case Period.Years:
                    date.AddYears(1);
                    break;
                default:
                    break;
            };
            return date;
        }



        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
