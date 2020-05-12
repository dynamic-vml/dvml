//#define SAVE

using System.Net.Http;

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using AngleSharp.Io.Network;
using AngleSharp.Scripting;

using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Object;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

namespace Tests
{
    public class AddNewItemTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;
        private readonly IConfiguration config;

        public AddNewItemTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.client = this.factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
            });

            this.config = Configuration.Default
                .WithCss()
                .WithJs()
                .WithCookies()
                .WithEventLoop()
                .WithRequester(new HttpClientRequester(this.client))
                .WithDefaultLoader(new LoaderOptions
                {
                    IsResourceLoadingEnabled = true,
                    IsNavigationDisabled = false
                });
        }

        [Fact]
        public async void ListEditorFor_ScriptsShouldLoadCorrectly()
        {
            // Arrange
            var context = BrowsingContext.New(config);

            // Act
            var document = await context.OpenAsync("http://localhost:5000/Home/EditSimpleWithLayout");
            await document.WaitForReadyAsync();

            // Assert
            JsScriptingService js = (JsScriptingService)context.GetJsScripting();
            ObjectInstance jquery = (ObjectInstance)js.EvaluateScript(document, "jQuery");
            Assert.NotNull(jquery);
            ObjectInstance dmvl = (ObjectInstance)js.EvaluateScript(document, "dvml");
            Assert.NotNull(dmvl);
            JsValue add = dmvl.Get("add");
            Assert.NotNull(add);
            JsValue remove = dmvl.Get("remove");
            Assert.NotNull(remove);
        }

        [Fact]
        public async void ListEditorFor_Get_onClickShouldFindRightElement()
        {
            // Arrange
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync("http://localhost:5000/Home/EditSimpleWithLayout");
            await document.WaitForReadyAsync();
            JsScriptingService js = (JsScriptingService)context.GetJsScripting();

            // Act
            await document.WaitUntilAvailable();

            string actual = document.ToHtml();
            IHtmlAnchorElement link = (IHtmlAnchorElement)document.QuerySelector("a[name='dynamic-list-addnewitem']");

            string onclick = link.GetAttribute("onclick");
            Assert.Equal("dvml.add('I', 'AddSimpleItem/?" +
                "ContainerId=I" +
                "&ListTemplate=EditorTemplates%2fDynamicList" +
                "&ItemContainerTemplate=DynamicItemContainer" +
                "&ItemTemplate=SimpleItem" +
                "&Prefix=Items" +
                "&Mode=0');", onclick);

            ObjectInstance result = (ObjectInstance)js.EvaluateScript(document, onclick);
            ObjectInstance containerObj = result.Get("container").AsObject();
            IHtmlDivElement container = (IHtmlDivElement)containerObj.GetType().GetProperty("Value").GetValue(containerObj);

            ObjectInstance optionsObj = result.Get("options").AsObject();
            string url = optionsObj.Get("url").ToString();
            string type = optionsObj.Get("type").ToString();
            bool cache = optionsObj.Get("cache").AsBoolean();

            // Assert
            Assert.Equal("I", container.Id);
            Assert.Equal("AddSimpleItem/?" +
                "ContainerId=I" +
                "&ListTemplate=EditorTemplates%2fDynamicList" +
                "&ItemContainerTemplate=DynamicItemContainer" +
                "&ItemTemplate=SimpleItem" +
                "&Prefix=Items" +
                "&Mode=0", url);
            Assert.Equal("GET", type);
            Assert.False(cache);
        }

        [Fact]
        public async void ListEditorFor_Get_onClickShouldAddElementToRightPlace()
        {
            // Arrange
            var context = BrowsingContext.New(config);
            string controllerAddress = "http://localhost:5000/Home/";
            string viewAddress = controllerAddress + "EditSimpleWithLayout";
            var document = await context.OpenAsync(viewAddress);
            await document.WaitForReadyAsync();
            JsScriptingService js = (JsScriptingService)context.GetJsScripting();

            // Act
            await document.WaitUntilAvailable();

            string before = document.ToStandardizedHtml();
            IHtmlAnchorElement link = (IHtmlAnchorElement)document.QuerySelector("a[name='dynamic-list-addnewitem']");
            ObjectInstance result = (ObjectInstance)js.EvaluateScript(document, link.GetAttribute("onclick"));
            ObjectInstance containerObj = result.Get("container").AsObject();
            IHtmlDivElement container = (IHtmlDivElement)containerObj.GetType().GetProperty("Value").GetValue(containerObj);

            ObjectInstance optionsObj = result.Get("options").AsObject();
            string url = optionsObj.Get("url").ToString();
            ScriptFunctionInstance success = optionsObj.Get("success").As<ScriptFunctionInstance>();

            // Call the controller action manually
            var response = await client.GetAsync(controllerAddress + url);
            string newItemHtml = await response.Content.ReadAsStringAsync();

            ObjectInstance p = new ObjectInstance(result.Engine);
            p.FastAddProperty("success", new JsValue(true), true, false, false);
            p.FastAddProperty("html", new JsValue(newItemHtml), true, false, false);
            success.Call(null, new[] { new JsValue(p) });
            string after = document.ToStandardizedHtml();

            // Assert
            string beforePath = Helpers.GetResourcePath(@"AddNewItem\simple-before.html");
            string afterPath = Helpers.GetResourcePath(@"AddNewItem\simple-after.html");

            string expectedBefore = beforePath.ToHtml();
            string expectedAfter = afterPath.ToHtml();
#if SAVE
            after.ToFile(afterPath);
            before.ToFile(beforePath);
#endif
            Assert.Equal(expectedBefore, before);
            Assert.Equal(expectedAfter, after);
        }

    }
}
