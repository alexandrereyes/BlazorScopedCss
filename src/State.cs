using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorScopedCss
{
    internal class State
    {
        public State(JsInterop jsInterop, Configuration configuration, CssBag cssBag)
        {
            _jsInterop = jsInterop;
            _configuration = configuration;
            _cssBag = cssBag;
            _renderedStyles = new Dictionary<string, string>();
        }

        #region Vars

        readonly JsInterop _jsInterop;
        readonly Configuration _configuration;
        readonly CssBag _cssBag;
        readonly Dictionary<string, string> _renderedStyles;

        #endregion

        /// <summary>
        /// Initialize the scoped component
        /// </summary>
        /// <param name="componentId">Id of the component</param>
        /// <param name="embeddedCssPath">Embedded css path inside the assembly</param>
        /// <returns>Returns if has changed style tag</returns>
        internal async Task<bool> InitializeComponent(ScopedStyle component, string embeddedCssPath, ComponentBase parent)
        {
            if (component.ReuseCss && component.Parent != null)
            {
                if (_renderedStyles.ContainsKey(component.Id))
                    return false;
            }

            if (!_cssBag.Styles.TryGetValue(embeddedCssPath, out string css))
            {
                if (parent != null)
                    embeddedCssPath = $"{parent.GetType().Namespace}.{embeddedCssPath}";

                if (!_cssBag.Styles.TryGetValue(embeddedCssPath, out css))
                {
                    throw new ArgumentException($"Embedded css path {embeddedCssPath} not found. Did you set the build action of the file as EmbeddedResource on Visual Studio?");
                }
            }

            _renderedStyles.Add(component.Id, css);

            await _jsInterop.InnerHTML(
                _configuration.StyleHtmlTagName,
                _renderedStyles
                    .Select(s => s.Value.Replace(_configuration.CssSelectorToReplace, s.Key.ToString()))
                    .Aggregate((a, b) => $"{a} {b}")
            );

            return true;
        }

        /// <summary>
        /// Dispose the component
        /// </summary>
        /// <param name="componentId"></param>
        internal void DisposeComponent(ScopedStyle component)
        {
            if (!component.ReuseCss || component.Parent == null)
            {
                _renderedStyles.Remove(component.Id);
            }
        }
    }
}
