using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorScopedCss
{
    public class ScopedStyle : ComponentBase, IDisposable
    {
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

        #endregion

        #region Props

        /// <summary>
        /// This component unique id
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// If BlazorScopedCss finished rendering the style
        /// </summary>
        public bool IsComplete { get; set; }

        #endregion

        /// <summary>
        /// Initialize the component
        /// </summary>
        /// <returns></returns>
        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            if (string.IsNullOrWhiteSpace(EmbeddedStylePath))
            {
                throw new ArgumentException(nameof(EmbeddedStylePath));
            }

            await State.InitializeComponent(Id, EmbeddedStylePath, Parent);
            IsComplete = true;
            if (AfterInit.HasDelegate) await AfterInit.InvokeAsync(null);
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
        {
            IsComplete = false;
            State.DisposeComponent(Id);
        }
    }
}