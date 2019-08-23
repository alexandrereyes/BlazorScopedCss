using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorScopedCss
{
    /// <summary>
    /// Class to store all css embedded on dll
    /// </summary>
    internal class CssBag
    {
        /// <summary>
        /// All styles embedded in the assembly
        /// </summary>
        internal Dictionary<string, string> Styles { get; private set; }

        /// <summary>
        /// Initialize the css bag
        /// </summary>
        /// <param name="embeddedCssAssembly"></param>
        internal CssBag(Assembly embeddedCssAssembly)
        {
            if (embeddedCssAssembly is null)
            {
                throw new ArgumentNullException(nameof(embeddedCssAssembly));
            }

            // initalize the styles bag
            Styles = embeddedCssAssembly.GetManifestResourceNames()
                .Select(s =>
                {
                    using Stream stream = embeddedCssAssembly.GetManifestResourceStream(s);
                    using StreamReader reader = new StreamReader(stream);
                    var css = reader.ReadToEnd();

                    return new
                    {
                        Name = s,
                        Resource = css
                    };

                })
                .Where(r => r.Name.ToLower().EndsWith(".css"))
                .ToDictionary(s => s.Name, s => s.Resource);
        }
    }
}
