using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlazorScopedCss
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Initialize ScopedCss lib
        /// </summary>
        /// <param name="serviceCollection">AspNet service collection referes</param>
        /// <param name="embeddedCssAssembly">Assembly where css embedded files are located</param>
        /// <param name="styleHtmlTagName">HTML tag where css will be render</param>
        /// <param name="cssSelectorToReplace">Css selector where this lib will replace by component id, for example: in css I have .myClass-scopeId... so, the CssSelectorToReplace is -scopeId</param>
        /// <returns>serviceCollection</returns>
        public static IServiceCollection AddBlazorScopedCss(this IServiceCollection serviceCollection, Assembly embeddedCssAssembly, string styleHtmlTagName = "scopedCss", string cssSelectorToReplace = "-scopeId")
        {
            serviceCollection.AddSingleton(new CssBag(embeddedCssAssembly));
            serviceCollection.AddSingleton(new Configuration(styleHtmlTagName, cssSelectorToReplace));
            serviceCollection.AddScoped<JsInterop>();
            serviceCollection.AddScoped<State>();
            return serviceCollection;
        }

    }
}
