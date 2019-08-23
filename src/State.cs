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
            _renderedStyles = new Dictionary<Guid, string>();
        }

        #region Vars

        readonly JsInterop _jsInterop;
        readonly Configuration _configuration;
        readonly CssBag _cssBag;
        readonly Dictionary<Guid, string> _renderedStyles;

        #endregion

        /// <summary>
        /// Initialize the scoped component
        /// </summary>
        /// <param name="componentId">Id of the component</param>
        /// <param name="embeddedCssPath">Embedded css path inside the assembly</param>
        /// <returns></returns>
        internal async Task InitializeComponent(Guid componentId, string embeddedCssPath)
        {
            if (!_cssBag.Styles.TryGetValue(embeddedCssPath, out string css))
                throw new ArgumentException($"Embedded css path {embeddedCssPath} not found. Did you set the build action of the file as EmbeddedResource on Visual Studio?");

            _renderedStyles.Add(componentId, css);

            await _jsInterop.InnerHTML(
                _configuration.StyleHtmlTagName,
                _renderedStyles
                    .Select(s => s.Value.Replace(_configuration.CssSelectorToReplace, s.Key.ToString()))
                    .Aggregate((a, b) => $"{a} {b}")
            );
        }

        /// <summary>
        /// Dispose the component
        /// </summary>
        /// <param name="componentId"></param>
        internal void DisposeComponent(Guid componentId)
            => _renderedStyles.Remove(componentId);
    }
}
