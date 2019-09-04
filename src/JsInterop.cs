using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorScopedCss
{
    internal class JsInterop
    {
        readonly IJSRuntime _jsRuntime;
        const string jsName = "blazorScopedCss";

        public JsInterop(IJSRuntime jsRuntime)
            => _jsRuntime = jsRuntime;

        internal async ValueTask InnerHTML(string elementId, string value)
            => await _jsRuntime.InvokeVoidAsync($"{jsName}.innerHTML", elementId, value);
    }
}
