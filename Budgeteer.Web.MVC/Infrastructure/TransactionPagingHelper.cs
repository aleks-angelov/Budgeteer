using System;
using System.Text;
using System.Web.Mvc;
using Budgeteer.Web.MVC.Models;

namespace Budgeteer.Web.MVC.Infrastructure
{
    public static class TransactionPagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, TransactionPagingInfo pagingInfo,
            Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.MergeAttribute("style", "margin-left: 2px");
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag);
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}