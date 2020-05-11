using System.IO;
using System.Linq;
using System.Net.Http;

using AngleSharp.Html.Dom;

using DynamicVML;
using DynamicVML.Internals;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

namespace Tests
{
    public class ControllerTests : IClassFixture<CustomWAF<Startup>>
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;

        public ControllerTests(CustomWAF<Startup> factory)
        {
            this.factory = factory;
            this.client = this.factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async void AddNewItem_ShouldRenderHtmlWithPrefix()
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
                actionUrl: "/Home/AddBook",
                addNewItemText: "Add new bookName",
                additionalViewData: null,
                method: NewItemMethod.Get);

            string url = e.GetActionContent();
            Assert.Equal("/Home/AddBook/" +
                "?ContainerId=4TUAPqX4s0SDU9fjH3oaFA" +
                "&ListTemplate=EditorTemplates%2fDynamicList" +
                "&ItemContainerTemplate=DynamicItemContainer" +
                "&ItemTemplate=Book" +
                "&Prefix=Namespace1.Namespace2.Namespace3" +
                "&Mode=0",
                url);

            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToHtml(minified: false);

            // Assert
            string htmlPrefix = prefix.Replace(".", "_");
            string itemId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_Index")).Value;
            Assert.NotEmpty(itemId);
            string actualId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_ContainerId")).Value;
            Assert.Equal(containerId, actualId);

            Assert.NotEqual(itemId, containerId);
            Assert.True(itemId.Length >= 20 && itemId.Length < 25, $"itemId: {itemId}");
        }


    }
}
