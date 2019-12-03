using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorScopedCss
{
    public class ScopedStyle : ComponentBase, IDisposable
    {
        #region Vars

        string _id;

        #endregion

        #region Injections

        [Inject]
        internal State State { get; set; }

        [Inject]
        internal Configuration Configuration { get; set; }

        #endregion

        #region Parameters

        /// <summary>
        /// Your css Embedded Path, for example: MyProject.MyNamespace.MyFile.css
        /// Do not forget to set your css as "Embedded Resource" on Visual Studio build action.
        /// If you don't want to fill the entire namespace, for example only "MyFile.css", just provide the Parent parameter
        /// </summary>
        [Parameter]
        public string EmbeddedStylePath { get; set; }

        /// <summary>
        /// After component initialization, this callback is called
        /// </summary>
        [Parameter]
        public EventCallback AfterInit { get; set; }

        /// <summary>
        /// If you provide the parent, it'll be possible to provide EmbeddedStylePath without the entire namespace
        /// </summary>
        [Parameter]
        public ComponentBase Parent { get; set; }

        /// <summary>
        /// If true (default), CSS scopeId will be replaced by Parent (required) GetType().Fullname
        /// </summary>
        [Parameter]
        public bool ReuseCss { get; set; } = true;

        #endregion

        #region Props

        /// <summary>
        /// This component unique id
        /// </summary>
        public string Id
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_id))
                {
                    if (ReuseCss && Parent != null)
                    {
                        _id = "-" + Parent.GetType().FullName.Replace(".", "-");
                        var indexOf = _id.IndexOf("`");
                        if (indexOf > -1) _id = _id.Substring(0, indexOf);
                    }
                    else
                    {
                        _id = Guid.NewGuid().ToString();
                    }
                }

                return _id;
            }
        }

        /// <summary>
        /// If BlazorScopedCss finished rendering the style
        /// </summary>
        public bool IsComplete { get; set; }

        #endregion

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (string.IsNullOrWhiteSpace(EmbeddedStylePath))
                {
                    throw new ArgumentException(nameof(EmbeddedStylePath));
                }

                await State.InitializeComponent(this, EmbeddedStylePath, Parent);
                IsComplete = true;
                if (AfterInit.HasDelegate) await AfterInit.InvokeAsync(null);
            }
        }

        /// <summary>
        /// Returns the css string to the class tag
        /// </summary>
        /// <param name="nonScopedCssClasses">css classes to concat before scoped css classes, example: btn btn-lg</param>
        /// <param name="scopedCssClasses">array of classes do return with scoped id, example: myclass-#id</param>
        /// <returns></returns>
        public string CssClassesMixed(string nonScopedCssClasses, params string[] scopedCssClasses)
        {
            var cssClass = new List<string>();

            if (!string.IsNullOrWhiteSpace(nonScopedCssClasses))
                cssClass.Add(nonScopedCssClasses.Trim());

            cssClass.AddRange(
                scopedCssClasses
                    .Where(w => !string.IsNullOrWhiteSpace(w))
                    .Select(c => $"{c.Trim()}{Configuration.CssSelectorToReplace}".Replace(Configuration.CssSelectorToReplace, Id.ToString()))
            );

            return cssClass.Aggregate((a, b) => $"{a} {b}");
        }

        /// <summary>
        /// Returns the css string to the class tag
        /// </summary>
        /// <param name="scopedCssClasses">array of classes do return with scoped id, example: myclass-#id</param>
        /// <returns></returns>
        public string CssScopedClasses(params string[] scopedCssClasses)
            => CssClassesMixed(null, scopedCssClasses);

        void IDisposable.Dispose()
            => State.DisposeComponent(this);
    }
}