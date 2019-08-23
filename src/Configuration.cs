namespace BlazorScopedCss
{
    internal class Configuration
    {
        /// <summary>
        /// HTML tag where css will be render
        /// </summary>
        internal string StyleHtmlTagName { get; }

        /// <summary>
        /// Css selector where this lib will replace by component id, for example: in css I have .myClass-scopeId... so, the CssSelectorToReplace is scopeId
        /// </summary>
        internal string CssSelectorToReplace { get; }

        /// <summary>
        /// Initialize the config
        /// </summary>
        /// <param name="styleHtmlTagName">HTML tag where css will be render</param>
        /// <param name="cssSelectorToReplace">Css selector where this lib will replace by component id, for example: in css I have .myClass-scopeId... so, the CssSelectorToReplace is scopeId</param>
        internal Configuration(string styleHtmlTagName, string cssSelectorToReplace)
        {
            StyleHtmlTagName = styleHtmlTagName;
            CssSelectorToReplace = cssSelectorToReplace;
        }
    }
}
