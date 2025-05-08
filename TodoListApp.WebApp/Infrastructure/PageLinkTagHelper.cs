using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.WebApi.Models.DTO.PagingDTO;

namespace TodoListApp.WebApp.Infrastructure
{
    /// <summary>
    /// Generates page navigation links based on a <see cref="TaskPaging"/> model.
    /// </summary>
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageLinkTagHelper"/> class.
        /// </summary>
        /// <param name="helperFactory">The factory used to create URL helpers.</param>
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            this.urlHelperFactory = helperFactory;
        }

        /// <summary>
        /// Gets or sets provides access to the current view context. This is set automatically by the framework.
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        /// <summary>
        /// Gets or sets the paging model containing pagination details such as current page and total count.
        /// </summary>
        public TaskPaging? PageModel { get; set; }

        /// <summary>
        /// Gets or sets the action method name used for generating page URLs.
        /// </summary>
        public string? PageAction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether indicates whether CSS classes should be applied to page links.
        /// </summary>
        public bool PageClassesEnabled { get; set; }

        /// <summary>
        /// Gets or sets base CSS class applied to all page links.
        /// </summary>
        public string PageClass { get; set; } = "btn";

        /// <summary>
        /// Gets or sets cSS class applied to non-selected page links.
        /// </summary>
        public string PageClassNormal { get; set; } = "btn-outline-dark";

        /// <summary>
        /// Gets or sets cSS class applied to the currently selected page link.
        /// </summary>
        public string PageClassSelected { get; set; } = "btn-primary";

        /// <summary>
        /// Gets or sets optional route name used for link generation.
        /// </summary>
        public string? PageRoute { get; set; }

        /// <summary>
        /// Gets dictionary of additional URL parameters passed to the action.
        /// Can be populated using attributes with the prefix <c>page-url-</c>.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Builds and injects pagination anchor tags into the output HTML.
        /// </summary>
        /// <param name="context">The tag helper context.</param>
        /// <param name="output">The tag helper output.</param>
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
