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

        internal async Task InnerHTML(string elementId, string value)
            => await _jsRuntime.InvokeAsync<object>($"{jsName}.innerHTML", elementId, value);
    }
}
