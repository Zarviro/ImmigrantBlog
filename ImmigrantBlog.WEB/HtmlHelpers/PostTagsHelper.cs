using ImmigrantBlog.WEB.Models;
using ImmigrantBlog.WEB.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ImmigrantBlog.WEB.HtmlHelpers
{
    public static class PostTagsHelper
    {
        // Previous page
        public static MvcHtmlString PostTags(this HtmlHelper html, PostEditModelView post)
        {
            //var li = $("<li>").attr("id", "tag_id_" + tagId).attr("class", "search-choice collective");
            //li.append($("<span>").attr("class", "text").append(tag));
            //li.append($("<a>").attr("href", "#").attr("class", "search-choice-close").attr("rel", tagId).append("| x"));
            //li.append($("<input />", { type: 'checkbox', value: tagId, name: "selectedTags", checked: "checked"}).attr("style", "display: none"));
            

            StringBuilder result = new StringBuilder();
            foreach(var postTag in post.Post.Tags)
            {
                // li
                TagBuilder tagLi = new TagBuilder("li");
                tagLi.MergeAttribute("id", "tag_id_" + postTag.Id);
                tagLi.AddCssClass("search-choice collective");
                // span
                TagBuilder tag = new TagBuilder("span");
                tag.AddCssClass("text");
                tag.InnerHtml = postTag.Name;
                tagLi.InnerHtml += tag.ToString();
                // a
                tag = new TagBuilder("a");
                tag.MergeAttribute("href", "#");
                tag.MergeAttribute("rel", postTag.Id.ToString());
                tag.AddCssClass("search-choice-close");
                tag.InnerHtml = "| x";
                tagLi.InnerHtml += tag.ToString();
                // checkbox
                tag = new TagBuilder("input");
                tag.MergeAttribute("type", "checkbox");
                tag.MergeAttribute("value", postTag.Id.ToString());
                tag.MergeAttribute("name", "selectedTags");
                tag.MergeAttribute("checked", "checked");
                tag.MergeAttribute("style", "display: none");
                tagLi.InnerHtml += tag.ToString();

                var tagRemove = post.Tags.FirstOrDefault(t => t.Value == postTag.Id.ToString());
                post.Tags.Remove(tagRemove);
                result.Append(tagLi.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}