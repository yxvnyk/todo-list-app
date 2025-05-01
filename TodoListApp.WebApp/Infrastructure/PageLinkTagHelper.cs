using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.WebApi.Models.DTO.PagingDTO;

namespace TodoListApp.WebApp.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            this.urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        public TaskPaging? PageModel { get; set; }

        public string? PageAction { get; set; }

        public bool PageClassesEnabled { get; set; }

        public string PageClass { get; set; } = "btn";

        public string PageClassNormal { get; set; } = "btn-outline-dark";

        public string PageClassSelected { get; set; } = "btn-primary";

        public string? PageRoute { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output);
            if (this.ViewContext != null && this.PageModel != null)
            {
                IUrlHelper urlHelper = this.urlHelperFactory.GetUrlHelper(this.ViewContext);
                TagBuilder result = new TagBuilder("div");
                for (int i = 1; i <= this.PageModel.TotalCount; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    this.PageUrlValues[key: "page"] = i;
                    tag.Attributes[key: "href"] = urlHelper.Action(this.PageAction, this.PageUrlValues);

                    _ = tag.InnerHtml.Append(i.ToString());
                    _ = result.InnerHtml.AppendHtml(tag);
                    if (this.PageClassesEnabled)
                    {
                        tag.AddCssClass(this.PageClass);
                        tag.AddCssClass(i == this.PageModel.CurrentPage
                            ? this.PageClassSelected : this.PageClassNormal);
                    }
                }

                _ = output.Content.AppendHtml(result.InnerHtml);
            }
        }
    }
}
