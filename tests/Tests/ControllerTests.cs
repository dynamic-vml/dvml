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
            var e = new EditorParams(
                itemTemplate: "Book",
                itemContainerTemplate: $"{Constants.DefaultItemContainerTemplate}",
                listTemplate: $"EditorTemplates/{Constants.DefaultListTemplate}",
                actionUrl: "/Home/AddBook",
                addNewItemText: "Add new bookName",
                prefix: prefix,
                additionalViewData: null,
                method: NewItemMethod.Get,
                mode: ListRenderMode.ViewModelOnly);

            string url = e.GetActionContent(containerId);
            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);

            // Assert
            string htmlPrefix = prefix.Replace(".", "_");
            string itemId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_Index")).Value;
            Assert.NotEmpty(itemId);
            string actualId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_ContainerId")).Value;
            Assert.Equal(containerId, actualId);

            Assert.NotEqual(itemId, containerId);
            Assert.True(itemId.Length > 20 && itemId.Length < 25);
        }


    }
}
