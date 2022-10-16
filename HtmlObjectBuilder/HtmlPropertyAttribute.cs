using System;

namespace HtmlObjectBuilder
{
    /// <summary>
    /// Represents an HTML Property. Used to identify a value from HTML based on it's classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HtmlPropertyAttribute : Attribute
    {
        /// <summary>
        /// Contains all the classes the HTML Attribute should have.
        /// </summary>
        public string[] Classes { get; set; }

        public HtmlPropertyAttribute(string hasClass)
        {
            Classes = new string[] { hasClass };
        }

        public HtmlPropertyAttribute(string[] classes)
        {
            Classes = classes;
        }
    }
}
