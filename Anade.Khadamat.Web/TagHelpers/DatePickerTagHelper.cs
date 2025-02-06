using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anade.Khadamat.Web.TagHelpers
{
    [HtmlTargetElement("datepicker")]
    public class DatePickerTagHelper : TagHelper
    {
        private const string FormatAttributeName = "asp-format";
        private const string ForAttributeName = "asp-for";
        private const string DefaultInputCssClassName = "form-control datepicker";

        // Mapping from <input/> element's type to RFC 3339 date and time formats.
        private static readonly IDictionary<string, string> _rfc3339Formats =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                { "date", @"dd/mm/yy" },
                //{ "datetime", @"DD-MM-YYYY HH:mm" },
                //{ "datetime-local", @"YYYY-MM-DDTHH:mm" },
                //{ "time", @"HH:mm" },
            };

        public DatePickerTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
            Format = "date";
        }

        public override int Order { get; }
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }
        [HtmlAttributeName(FormatAttributeName)]
        public string Format { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        protected IHtmlGenerator Generator { get; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var metadata = For?.Metadata;
            var modelExplorer = For?.ModelExplorer;
            
            //if (metadata == null)
            //{
            //    throw new InvalidOperationException($"Model not provided {ForAttributeName}");
            //}

            IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();

            if (Name != null)
            {
                htmlAttributes.Add(nameof(Name), context);
            }

            if (Value != null)
            {
                htmlAttributes.Add(nameof(Value), context);
            }

            foreach (var attribute in output.Attributes)
            {
                htmlAttributes.Add(attribute.Name, attribute.Value);
            }

            if (!htmlAttributes.ContainsKey("class"))         
            {
                htmlAttributes.Add("class", DefaultInputCssClassName);
            }
            else
            {
                htmlAttributes["class"] = htmlAttributes["class"] + " datepicker";
            }

            TagBuilder textInputTagBuilder = For!=null? Generator.GenerateTextBox(
                 ViewContext,
                 modelExplorer,
                 For?.Name,
                 modelExplorer?.Model,
                 GetFormat(modelExplorer, null, "text"),
                 htmlAttributes):new TagBuilder("input");

            if(For!=null)
            {
                textInputTagBuilder = Generator.GenerateTextBox(
                 ViewContext,
                 modelExplorer,
                 For?.Name,
                 modelExplorer?.Model,
                 GetFormat(modelExplorer, null, "text"),
                 htmlAttributes);
            }
            else
            {
                textInputTagBuilder = new TagBuilder("input");
                foreach (var attr in htmlAttributes)
                {
                    textInputTagBuilder.Attributes.Add(attr.Key, attr.Value.ToString());
                }
            }

            if (!textInputTagBuilder.Attributes.ContainsKey("type"))
                textInputTagBuilder.Attributes.Add("type", "text");

            textInputTagBuilder.Attributes.Add(FormatAttributeName.Replace("asp", "data-date"), _rfc3339Formats.ContainsKey(Format) ? _rfc3339Formats[Format] : Format);

            var spanIconTagBuilder = new TagBuilder("span");
            spanIconTagBuilder.AddCssClass("input-group-text");

            var iconTagBuilder = new TagBuilder("i");
            iconTagBuilder.AddCssClass("fa fa-calendar");
            spanIconTagBuilder.InnerHtml.AppendHtml(iconTagBuilder);

            var spanIconOuterDivTagBuilder = new TagBuilder("div");
            spanIconOuterDivTagBuilder.AddCssClass("input-group-append");
            spanIconOuterDivTagBuilder.InnerHtml.AppendHtml(spanIconTagBuilder);

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            
            output.Attributes.Clear();
            output.Attributes.Add("class", "input-group");
            output.Content.AppendHtml(textInputTagBuilder);
            output.Content.AppendHtml(spanIconOuterDivTagBuilder);
        }
        private string GetFormat(ModelExplorer modelExplorer, string inputTypeHint, string inputType)
        {
            string format="";

            if (modelExplorer == null)
                return format;

            if (string.Equals("month", inputType, StringComparison.OrdinalIgnoreCase))
            {
                // "month" is a new HTML5 input type that only will be rendered in Rfc3339 mode
                format = "{0:yyyy-MM}";
            }
            else if (ViewContext.Html5DateRenderingMode == Html5DateRenderingMode.Rfc3339 &&
                !modelExplorer.Metadata.HasNonDefaultEditFormat &&
                (typeof(DateTime) == modelExplorer.Metadata.UnderlyingOrModelType ||
                 typeof(DateTimeOffset) == modelExplorer.Metadata.UnderlyingOrModelType))
            {
                // Rfc3339 mode _may_ override EditFormatString in a limited number of cases. Happens only when
                // EditFormatString has a default format i.e. came from a [DataType] attribute.
                if (string.Equals("text", inputType) &&
                    string.Equals(nameof(DateTimeOffset), inputTypeHint, StringComparison.OrdinalIgnoreCase))
                {
                    // Auto-select a format that round-trips Offset and sub-Second values in a DateTimeOffset. Not
                    // done if user chose the "text" type in .cshtml file or with data annotations i.e. when
                    // inputTypeHint==null or "text".
                    format = _rfc3339Formats["datetime"];
                }
                else if (_rfc3339Formats.TryGetValue(inputType, out var rfc3339Format))
                {
                    format = rfc3339Format;
                }
                else
                {
                    // Otherwise use default EditFormatString.
                    format = modelExplorer.Metadata.EditFormatString;
                }
            }
            else
            {
                // Otherwise use EditFormatString, if any.
                format = modelExplorer.Metadata.EditFormatString;
            }

            return format;
        }
    }
}
