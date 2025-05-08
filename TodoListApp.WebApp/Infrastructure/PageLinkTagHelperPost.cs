using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TodoListApp.WebApi.Models.DTO.PagingDTO;

namespace TodoListApp.WebApp.Infrastructure
{
    /// <summary>
    /// A <see cref="TagHelper"/> that generates pagination controls using POST requests for server-side filtering and paging.
    /// </summary>
    /// <remarks>
    /// This helper renders a series of HTML <c>&lt;form&gt;</c> elements with hidden inputs and submit buttons,
    /// allowing pagination to be performed via POST instead of GET.
    /// </remarks>
    [HtmlTargetElement("div", Attributes = "page-model-post")]
    public class PageLinkTagHelperPost : TagHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageLinkTagHelperPost"/> class.
        /// </summary>
        public PageLinkTagHelperPost()
        {
        }

        /// <summary>
        /// Gets or sets the current view context. Automatically injected by the Razor engine.
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }

        /// <summary>
        /// Gets or sets the paging model used to determine total page count and the current page.
        /// </summary>
        [HtmlAttributeName("page-model-post")]
        public TaskPaging? PageModelPost { get; set; }

        /// <summary>
        /// Gets or sets the name of the controller action to which the pagination POST requests will be sent.
        /// </summary>
        public string PageAction { get; set; } = "GetByFilter";

        /// <summary>
        /// Gets or sets the name of the controller handling the pagination POST action.
        /// </summary>
        public string PageController { get; set; } = "Task";

        /// <summary>
        /// Gets or sets a value indicating whether CSS classes should be applied to the pagination buttons.
        /// </summary>
        public bool PageClassesEnabled { get; set; }

        /// <summary>
        /// Gets or sets the base CSS class applied to all pagination buttons.
        /// </summary>
        public string PageClass { get; set; } = "btn";

        /// <summary>
        /// Gets or sets the CSS class applied to non-selected page buttons.
        /// </summary>
        public string PageClassNormal { get; set; } = "btn-outline-dark";

        /// <summary>
        /// Gets or sets the CSS class applied to the currently active (selected) page button.
        /// </summary>
        public string PageClassSelected { get; set; } = "btn-primary";

        /// <summary>
        /// Gets the collection of additional hidden POST parameters to include in each pagination form.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = "page-post-")]
        public Dictionary<string, object> PagePostValues { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Builds the HTML output for the tag helper, rendering a form-based pagination UI using POST requests.
        /// </summary>
        /// <param name="context">The context for the current tag helper.</param>
        /// <param name="output">The output object used to write the generated HTML.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="output"/> is <c>null</c>.</exception>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output);

            if (this.ViewContext != null && this.PageModelPost != null)
            {
                TagBuilder result = new TagBuilder("div");

                for (int i = 1; i <= this.PageModelPost.TotalCount; i++)
                {
                    TagBuilder form = new TagBuilder("form");
                    form.Attributes["method"] = "post";
                    form.Attributes["action"] = "/" + this.PageController + "/" + this.PageAction;
                    form.AddCssClass("d-inline");

                    TagBuilder pageNumber = new TagBuilder("input");
                    pageNumber.Attributes["type"] = "hidden";
                    pageNumber.Attributes["name"] = "PageNumber";
                    pageNumber.Attributes["value"] = i.ToString(CultureInfo.InvariantCulture);
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
                    _ = button.InnerHtml.Append(i.ToString(CultureInfo.InvariantCulture));

                    if (this.PageClassesEnabled)
                    {
                        button.AddCssClass(this.PageClass);
                        button.AddCssClass(i == this.PageModelPost.CurrentPage
                            ? this.PageClassSelected : this.PageClassNormal);
                    }

                    _ = form.InnerHtml.AppendHtml(button);
                    _ = result.InnerHtml.AppendHtml(form);
                }

                _ = output.Content.AppendHtml(result.InnerHtml);
            }
        }
    }
}
