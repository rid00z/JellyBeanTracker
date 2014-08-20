using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using MVCPolyfils.Helper;

namespace MVCPolyfils
{
    public interface IHtmlString {
        string ToHtmlString ();
    }

    public partial class HtmlHelper {
        private TextWriter _writer;

        public HtmlHelper(TextWriter writer) {
            _writer = writer;
        }

        private string GenerateHtmlAttributes(object htmlAttributes) {
            var attrs = new StringBuilder ();
            if (htmlAttributes != null) {
                foreach (var property in htmlAttributes.GetType ().GetRuntimeProperties()) 
                    attrs.AppendFormat (@" {0}=""{1}""", property.Name.Replace('_', '-'), property.GetMethod.Invoke (htmlAttributes, null));
            }
            return attrs.ToString ();
        }

        public IHtmlString RenderPartial(string partialTypeName, object model = null, Dictionary<string, object> viewData = null) {
            return Partial(partialTypeName, model, viewData);
        }

        public IHtmlString Partial(string partialTypeName, object model, Dictionary<string, object> viewData = null) {
            var razorPageType = Type.GetType(partialTypeName);
            if (razorPageType == null)
                throw new Exception("Type " + partialTypeName + " not found");

            var razorPage = Activator.CreateInstance(razorPageType);

            if (model != null)
            {
                var modelProperty = ReflectionHelpers.GetProperty(razorPageType, "Model");
                if (modelProperty != null)
                    modelProperty.SetValue(razorPage, model);
            }

            if (viewData != null)
            {
                var viewDataMethod = ReflectionHelpers.GetProperty(razorPageType, "ViewData");
                if (viewDataMethod != null)
                    viewDataMethod.SetValue(razorPage, viewData);
            }

            var generateStringMethod = ReflectionHelpers.GetMethod(razorPageType, "GenerateString");

            return new HtmlString((string)generateStringMethod.Invoke(razorPage, null));
        }

        public IHtmlString Raw(string input)
        {
            return new HtmlString(input);
        }
    }
}

