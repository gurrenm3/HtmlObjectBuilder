using HtmlAgilityPack;
using HtmlObjectBuilder.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HtmlObjectBuilder
{
    /// <summary>
    /// Used to instantiate an instance of a class from an HTML Node.
    /// </summary>
    public abstract class HtmlObject
    {
        private readonly HtmlNode _objectNode;

        public HtmlObject(HtmlNode objectNode)
        {
            this._objectNode = objectNode;
            var properties = GetHtmlProperties();
            SetHtmlProperties(properties);
        }

        /// <summary>
        /// Returns the matching node that contains all of the provided classes.
        /// </summary>
        /// <param name="classes"></param>
        /// <returns></returns>
        private HtmlNode GetNodeWithClasses(params string[] classes)
        {
            return _objectNode?.Descendants()?.FirstOrDefault(d => d.HasClasses(classes))!;
        }

        /// <summary>
        /// Returns all of the properties and their corresponding <see cref="HtmlPropertyAttribute"/>.
        /// </summary>
        /// <returns></returns>
        private Dictionary<PropertyInfo, HtmlPropertyAttribute> GetHtmlProperties()
        {
            var results = new Dictionary<PropertyInfo, HtmlPropertyAttribute>();

            var properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var prop in properties)
            {
                HtmlPropertyAttribute attr = prop.GetCustomAttribute<HtmlPropertyAttribute>()!;
                if (attr == null)
                    continue;

                results.Add(prop, attr);
            }

            return results;
        }

        /// <summary>
        /// Sets the value of each property with it's corresponding <see cref="HtmlPropertyAttribute"/>.
        /// </summary>
        /// <param name="properties"></param>
        private void SetHtmlProperties(Dictionary<PropertyInfo, HtmlPropertyAttribute> properties)
        {
            foreach (var item in properties)
            {
                var node = GetNodeWithClasses(item.Value.Classes);
                if (node == null)
                    continue;

                var htmlValue = node.InnerHtml?.Trim();

                if (item.Key.PropertyType == typeof(string))
                {
                    item.Key.SetValue(this, htmlValue);
                    continue;
                }

                // it's not a string, try converting it to it's real type.
                try
                {
                    var type = item.Key.PropertyType;

                    // it has a decimal but will be stored as a whole number.
                    if (htmlValue.Contains(".") && type == typeof(int) || type == typeof(long) || type == typeof(byte))
                    {
                        var split = htmlValue.Split('.');
                        htmlValue = split[split.Length - 2];
                    }

                    // it's an int but will be stored as a bool.
                    else if (type == typeof(bool) && htmlValue == "1")
                        htmlValue = "true";

                    var value = Convert.ChangeType(htmlValue, item.Key.PropertyType);
                    item.Key.SetValue(this, value);
                }
                catch (Exception) { }
            }
        }
    }
}
