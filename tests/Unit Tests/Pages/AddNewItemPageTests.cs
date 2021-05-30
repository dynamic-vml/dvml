//#define SAVE

using System.Net.Http;
using System.Text;
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
using Newtonsoft.Json;
using Xunit;

namespace Tests.RazorPages
{
    public class AddNewItemPageTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;
        private readonly IConfiguration config;

        public AddNewItemPageTests(WebApplicationFactory<Startup> factory)
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
            var document = await context.OpenAsync("http://localhost:5000/EditSimple");
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
            var document = await context.OpenAsync("http://localhost:5000/EditSimple");
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
                "&Prefix=Input.Items" +
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
                "&Prefix=Input.Items" +
                "&Mode=0", url);
            Assert.Equal("GET", type);
            Assert.False(cache);
        }

        [Fact]
        public async void ListEditorFor_Get_onClickShouldAddElementToRightPlace()
        {
            // Arrange
            var context = BrowsingContext.New(config);
            string pageAddress = "http://localhost:5000/EditSimple/";
            var document = await context.OpenAsync(pageAddress);
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
            var response = await client.GetAsync(pageAddress + url);
            string newItemHtml = await response.Content.ReadAsStringAsync();

            success.Call(null, new[] { new JsValue(newItemHtml) });
            string after = document.ToStandardizedHtml();

            // Assert
            string beforePath = Helpers.GetResourcePath(@"AddNewItem\razor-simple-before.html");
            string afterPath = Helpers.GetResourcePath(@"AddNewItem\razor-simple-after.html");

#if SAVE
            after.ToFile(afterPath);
            before.ToFile(beforePath);
#endif
            string expectedBefore = beforePath.ToHtml();
            string expectedAfter = afterPath.ToHtml();

            Assert.Equal(expectedBefore, before);
            Assert.Equal(expectedAfter, after);
        }

        [Fact]
        public async void ListEditorFor_Post_onClickShouldAddElementToRightPlace()
        {
            // Arrange
            var context = BrowsingContext.New(config);
            string pageAddress = "http://localhost:5000/EditSimpleByPost/";
            var document = await context.OpenAsync(pageAddress);
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
            string data = optionsObj.Get("data").ToString();
            string contentType = optionsObj.Get("contentType").ToString();
            string type = optionsObj.Get("type").ToString();
            bool cache = optionsObj.Get("cache").AsBoolean();
            ScriptFunctionInstance success = optionsObj.Get("success").As<ScriptFunctionInstance>();

            Assert.Equal("AddSimpleItemByPost", url);
            Assert.Equal("{\"ContainerId\":\"I\"," +
                "\"ItemTemplate\":\"SimpleItem\"," +
                "\"ItemContainerTemplate\":\"DynamicItemContainer\"," +
                "\"ListTemplate\":\"EditorTemplates/DynamicList\"," +
                "\"Prefix\":\"Input.Items\"," +
                "\"Mode\":0,\"" +
                "AdditionalViewData\":\"eyJEYXRhIjoibXlEYXRhIn0=\"}",
                data);
            Assert.Equal("application/json; charset=utf-8", contentType);
            Assert.Equal("POST", type);
            Assert.False(cache);

            // Call the controller action manually
            var response = await client.PostAsync(pageAddress + url,
                new StringContent(data, Encoding.UTF8, "application/json"));

            string json = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeAnonymousType(json, new
            {
                success = false,
                html = ""
            });

            Assert.True(content.success);
            Assert.NotEmpty(content.html);

            ObjectInstance p = new ObjectInstance(result.Engine);
            p.FastAddProperty("success", new JsValue(true), true, false, false);
            p.FastAddProperty("html", new JsValue(content.html), true, false, false);
            success.Call(null, new[] { new JsValue(p) });
            string after = document.ToStandardizedHtml();

            // Assert
            string beforePath = Helpers.GetResourcePath(@"AddNewItem\razor-simple-post-before.html");
            string afterPath = Helpers.GetResourcePath(@"AddNewItem\razor-simple-post-after.html");

#if SAVE
            after.ToFile(afterPath);
            before.ToFile(beforePath);
#endif
            
            string expectedBefore = beforePath.ToHtml();
            string expectedAfter = afterPath.ToHtml();

            Assert.Equal(expectedBefore, before);
            Assert.Equal(expectedAfter, after);
        }

        [Fact]
        public async void ListEditorFor_Post_WithAntiForgeryTokens_onClickShouldAddElementToRightPlace()
        {
            // Arrange
            var context = BrowsingContext.New(config);
            string pageAddress = "http://localhost:5000/EditSimpleByPostWithAntiForgeryTokens";
            IDocument document = await context.OpenAsync(pageAddress);
            await document.WaitForReadyAsync();
            JsScriptingService js = (JsScriptingService)context.GetJsScripting();

            // Act
            await document.WaitUntilAvailable();

            IHtmlInputElement antiForgeryField = (IHtmlInputElement)document.QuerySelector("input[name=\"__RequestVerificationToken\"]");
            Assert.NotNull(antiForgeryField);
            Assert.NotEmpty(antiForgeryField.Value);

            IHtmlAnchorElement link = (IHtmlAnchorElement)document.QuerySelector("a[name='dynamic-list-addnewitem']");
            ObjectInstance result = (ObjectInstance)js.EvaluateScript(document, link.GetAttribute("onclick"));
            ObjectInstance containerObj = result.Get("container").AsObject();
            IHtmlDivElement container = (IHtmlDivElement)containerObj.GetType().GetProperty("Value").GetValue(containerObj);

            ObjectInstance optionsObj = result.Get("options").AsObject();
            string url = optionsObj.Get("url").ToString();
            string data = optionsObj.Get("data").ToString();
            string contentType = optionsObj.Get("contentType").ToString();
            string type = optionsObj.Get("type").ToString();
            bool cache = optionsObj.Get("cache").AsBoolean();
            ScriptFunctionInstance success = optionsObj.Get("success").As<ScriptFunctionInstance>();

            Assert.Equal("?handler=AddSimpleItemByPost", url);
            Assert.Equal("{\"ContainerId\":\"I\"," +
                "\"ItemTemplate\":\"SimpleItem\"," +
                "\"ItemContainerTemplate\":\"DynamicItemContainer\"," +
                "\"ListTemplate\":\"EditorTemplates/DynamicList\"," +
                "\"Prefix\":\"Input.Items\"," +
                "\"Mode\":0,\"" +
                "AdditionalViewData\":\"eyJEYXRhIjoibXlEYXRhIn0=\"}",
                data);
            Assert.Equal("application/json; charset=utf-8", contentType);
            Assert.Equal("POST", type);
            Assert.False(cache);

            ObjectInstance headers = optionsObj.Get("headers").AsObject();
            string verificationToken = headers.Get("RequestVerificationToken").AsString();
            Assert.Equal(antiForgeryField.Value, verificationToken);

            // Call the controller action manually
            var contentToSend = new StringContent(data, Encoding.UTF8, "application/json");
            contentToSend.Headers.Add("RequestVerificationToken", verificationToken);
            var response = await client.PostAsync(pageAddress + url, contentToSend);

            string json = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeAnonymousType(json, new
            {
                success = false,
                html = ""
            });

            Assert.True(content.success);
            Assert.NotEmpty(content.html);

            ObjectInstance p = new ObjectInstance(result.Engine);
            p.FastAddProperty("success", new JsValue(true), true, false, false);
            p.FastAddProperty("html", new JsValue(content.html), true, false, false);
            success.Call(null, new[] { new JsValue(p) });
        }
    }
}
