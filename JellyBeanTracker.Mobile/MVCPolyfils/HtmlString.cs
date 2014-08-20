using System;

namespace MVCPolyfils
{
    public class HtmlString : IHtmlString {
        string value;

        public HtmlString (string value)
        {
            this.value = value;
        }

        public string ToHtmlString ()
        {
            return value;
        }

        public override string ToString ()
        {
            return value;
        }
    }
}

