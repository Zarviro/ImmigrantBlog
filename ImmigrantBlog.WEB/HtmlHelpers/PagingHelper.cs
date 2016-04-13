using ImmigrantBlog.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.HtmlHelpers
{
    public static class PagingHelper
    {
        // Previous page
        public static MvcHtmlString PrevPage(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder tag = new TagBuilder("a");
            tag.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
            tag.InnerHtml = "<";
            tag.AddCssClass("btn btn-default pagination");
            if (pagingInfo.CurrentPage <= 1)
            {
                tag.AddCssClass("disabled");
            }
            result.Append(tag.ToString());

            return MvcHtmlString.Create(result.ToString());
        }

        // Next page
        public static MvcHtmlString NextPage(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder tag = new TagBuilder("a");
            tag.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
            tag.InnerHtml = ">";
            tag.AddCssClass("btn btn-default pagination");
            if(pagingInfo.CurrentPage >= pagingInfo.TotalPages)
            {
                tag.AddCssClass("disabled");
            }
            result.Append(tag.ToString());

            return MvcHtmlString.Create(result.ToString());
        }

        // Page links
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            int pageRange = pagingInfo.TotalPages > 7 ? 7 : pagingInfo.TotalPages;
            int startPage;
            int endPage;   
            StringBuilder result = new StringBuilder();

            if(pagingInfo.CurrentPage != 1)
            {
                TagBuilder tagFirst = new TagBuilder("a");
                tagFirst.MergeAttribute("href", pageUrl(1));
                tagFirst.InnerHtml = "<<";
                tagFirst.AddCssClass("btn btn-small pagination");
                result.Append(tagFirst);
            }

            if (pagingInfo.CurrentPage <= pageRange / 2)
            {
                startPage = 1;
                endPage = pageRange; 
            }
            else if (pagingInfo.CurrentPage > (pagingInfo.TotalPages - pageRange / 2))
            {
                startPage = pagingInfo.TotalPages - (pageRange - 1);
                endPage = pagingInfo.TotalPages;
            }
            else
            {
                startPage = pagingInfo.CurrentPage - pageRange / 2 ;
                endPage = pagingInfo.CurrentPage + pageRange / 2 ;
            }
            for (int i = startPage; i <= endPage; i++)
            {
                TagBuilder tagPage = new TagBuilder("a");
                tagPage.MergeAttribute("href", pageUrl(i));
                tagPage.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                {
                    tagPage.AddCssClass("selected");
                    tagPage.AddCssClass("btn-primary");
                }
                tagPage.AddCssClass("btn btn-mini pagination");
                result.Append(tagPage.ToString());
            }

            if (pagingInfo.CurrentPage != pagingInfo.TotalPages)
            {
                TagBuilder tagLast = new TagBuilder("a");
                tagLast.MergeAttribute("href", pageUrl(pagingInfo.TotalPages));
                tagLast.InnerHtml = ">>";
                tagLast.AddCssClass("btn btn-small pagination");
                result.Append(tagLast);
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}