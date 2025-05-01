using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.WebApi.Models.DTO.PagingDTO;

namespace TodoListApp.WebApp.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model-post")]
    public class PageLinkTagHelperPost : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelperPost(IUrlHelperFactory helperFactory)
        {
            this.urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        [HtmlAttributeName("page-model-post")]
        public TaskPaging? PageModelPost { get; set; }

        public TaskPaging? PageFilter { get; set; }

        public string PageAction { get; set; } = "GetByFilter";

        public string PageController { get; set; } = "Task";

        public bool PageClassesEnabled { get; set; }

        public string PageClass { get; set; } = "btn";

        public string PageClassNormal { get; set; } = "btn-outline-dark";

        public string PageClassSelected { get; set; } = "btn-primary";

        public string? PageRoute { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-post-")]
        public Dictionary<string, object> PagePostValues { get; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output);
            if (this.ViewContext != null && this.PageModelPost != null)
            {
                IUrlHelper urlHelper = this.urlHelperFactory.GetUrlHelper(this.ViewContext);
                TagBuilder result = new TagBuilder("div");
                for (int i = 1; i <= this.PageModelPost.TotalCount; i++)
                {
                    TagBuilder form = new TagBuilder("form");
                    form.Attributes["method"] = "post";
                    form.Attributes["action"] = form.Attributes["action"] = "/" + this.PageController + "/" + this.PageAction;

                    form.AddCssClass("d-inline");

                    TagBuilder pageNumber = new TagBuilder("input");
                    pageNumber.Attributes["type"] = "hidden";
                    pageNumber.Attributes["name"] = "PageNumber";
                    pageNumber.Attributes["value"] = i.ToString();
                    _ = form.InnerHtml.AppendHtml(pageNumber);

                    foreach (var pair in this.PagePostValues)
                    {
                        TagBuilder input = new TagBuilder("input");
                        input.Attributes["type"] = "hidden";
                        input.Attributes["name"] = pair.Key;
                        input.Attributes["value"] = pair.Value?.ToString();
                        _ = form.InnerHtml.AppendHtml(input);
                    }

                    TagBuilder button = new TagBuilder("button");
                    button.Attributes["type"] = "submit";
                    _ = button.InnerHtml.Append(i.ToString());
                    form.InnerHtml.AppendHtml(button);

                    //this.PageUrlValues[key: "page"] = i;

                    _ = result.InnerHtml.AppendHtml(form);
                    if (this.PageClassesEnabled)
                    {
                        form.AddCssClass(this.PageClass);
                        form.AddCssClass(i == this.PageModelPost.CurrentPage
                            ? this.PageClassSelected : this.PageClassNormal);
                    }
                }

                _ = output.Content.AppendHtml(result.InnerHtml);
            }
        }
    }
}
