using System.Net.Http;
using System.Text;

using AngleSharp.Html.Dom;

using DynamicVML;
using DynamicVML.Internals;

using Microsoft.AspNetCore.Mvc.Testing;

using Newtonsoft.Json;

using Xunit;

namespace Tests.RazorPages
{
    public class PageModelTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;

        public PageModelTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.client = this.factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async void AddNewItem_ShouldRenderHtmlWithPrefix_Get()
        {
            string prefix = "Namespace1.Namespace2.Namespace3";
            string containerId = "4TUAPqX4s0SDU9fjH3oaFA";

            // Act
            var e = new ListEditorParameters(
                parameters: new ListParameters(
                                containerId: containerId,
                                itemTemplate: "Book",
                                itemContainerTemplate: Constants.DefaultItemContainerTemplate,
                                listTemplate: "EditorTemplates/" + Constants.DefaultListTemplate,
                                prefix: prefix,
                                mode: ListRenderMode.ViewModelOnly),
                actionUrl: "/SomePage/AddBook",
                addNewItemText: "Add new bookName",
                additionalViewData: null,
                method: NewItemMethod.Get);

            string url = e.GetActionInfo();
            Assert.Equal("/SomePage/AddBook/" +
                "?ContainerId=4TUAPqX4s0SDU9fjH3oaFA" +
                "&ListTemplate=EditorTemplates%2fDynamicList" +
                "&ItemContainerTemplate=DynamicItemContainer" +
                "&ItemTemplate=Book" +
                "&Prefix=Namespace1.Namespace2.Namespace3" +
                "&Mode=0",
                url);

            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToStandardizedHtml(minified: false);

            // Assert
            string htmlPrefix = prefix.Replace(".", "_");
            string itemId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_Index")).Value;
            Assert.NotEmpty(itemId);
            string actualId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_ContainerId")).Value;
            Assert.Equal(containerId, actualId);

            Assert.NotEqual(itemId, containerId);
            Assert.True(itemId.Length >= 19 && itemId.Length < 23, $"itemId: {itemId}");
        }


        [Fact]
        public async void AddNewItem_ShouldIncludeControllerParameters_Get()
        {
            string prefix = "Namespace1.Namespace2.Namespace3";
            string containerId = "4TUAPqX4s0SDU9fjH3oaFA";

            // Act
            var e = new ListEditorParameters(
                parameters: new ListParameters(
                                containerId: containerId,
                                itemTemplate: "Book",
                                itemContainerTemplate: Constants.DefaultItemContainerTemplate,
                                listTemplate: "EditorTemplates/" + Constants.DefaultListTemplate,
                                prefix: prefix,
                                mode: ListRenderMode.ViewModelOnly),
                actionUrl: "/SomePage/AddBookWithParameterByGet?integerParameter=42&stringParameter=test",
                addNewItemText: "Add new bookName",
                additionalViewData: null,
                method: NewItemMethod.Get);

            string url = e.GetActionInfo();
            Assert.Equal("/SomePage/AddBookWithParameterByGet" +
                "?integerParameter=42" +
                "&stringParameter=test" +
                "&ContainerId=4TUAPqX4s0SDU9fjH3oaFA" +
                "&ListTemplate=EditorTemplates%2fDynamicList" +
                "&ItemContainerTemplate=DynamicItemContainer" +
                "&ItemTemplate=Book" +
                "&Prefix=Namespace1.Namespace2.Namespace3" +
                "&Mode=0",
                url);

            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToStandardizedHtml(minified: false);

            // Assert
            Assert.Contains("New book via GET with parameters: 42 test", actual);
            string htmlPrefix = prefix.Replace(".", "_");
            string itemId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_Index")).Value;
            Assert.NotEmpty(itemId);
            string actualId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_ContainerId")).Value;
            Assert.Equal(containerId, actualId);
        }
    }
}
