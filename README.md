# BlazorScopedCss
BlazorScopedCss is a library to fill the css gap in current Blazor version.
This is how it works:
![This is how it works](https://user-images.githubusercontent.com/729956/63616234-7d1c2c80-c5bd-11e9-9e88-3c01e8fcad5a.gif)




# Getting started
1. Dependency Injection time: go to `Startup.cs` `ConfigureServices` and add the line bellow:
`services.AddBlazorScopedCss(Assembly.GetExecutingAssembly());`

2. Go to the main cshtml or html file and add the `<style id="scopedCss"></style` tag inside html->head, this is the place where BlazorScopedCss will render the styles

3. In the same file, add the BlazorScopedCss js: `<script src="/_content/BlazorScopedCss/jsInterop.js"></script>`

4. Time to add the css! In the folder where your component or page is located, add the css file, `FetchData.razor.css` for example (Visual Studio will nest the file inside .razor, awesome!)

5. Inside de CSS, add some classes:
```
.myFirstScopedComponent-scopeId {
    background-color: red;
}

.mySecondScopedComponent-scopeId {
    background-color: blue;
}
```

This library will replace `-scopeId` by the componentId, so...if you don't put this keyword it will not work!

6. Select the css in solution explorer and press f4 (properties), change the Build Action to `Embedded resource`

7. Ok, now go to your .razor page or component, and add BazorScopedCss:
```
<BlazorScopedCss.ScopedStyle EmbeddedStylePath="SampleApp.Pages.FetchData.razor.css"
                             AfterInit="@(() => StateHasChanged())"
                             @ref="scopedStyle" />

<h1 class="@scopedStyle?.CssScopedClasses(scopedCssClasses: "myFirstScopedComponent")">Weather forecast</h1>

<p class="@scopedStyle?.CssClassesMixed(nonScopedCssClasses: "display-1", scopedCssClasses: "mySecondScopedComponent")">This component demonstrates fetching data from a service.</p>
```

That's it! Watch HTML `<style id="scopedCss">` tag to see the magic :)
