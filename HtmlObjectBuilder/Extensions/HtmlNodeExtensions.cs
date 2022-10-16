using HtmlAgilityPack;

namespace HtmlObjectBuilder.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="HtmlNode"/>.
    /// </summary>
    public static class HtmlNodeExtensions
    {
        /// <summary>
        /// Returns whether or not this HTML Node contains all of the provided classes.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="classes"></param>
        /// <returns></returns>
        public static bool HasClasses(this HtmlNode node, params string[] classes)
        {
            foreach (var c in classes)
            {
                if(!node.HasClass(c))
                    return false;
            }

            return true;
        }
    }
}
