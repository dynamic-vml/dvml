//#define SAVE

using System.Net.Http;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

namespace Tests.MVC
{
    public class BasicEditorTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;

        public BasicEditorTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.client = this.factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async void ListEditorFor_ShouldRenderViewModels()
        {
            // Arrange
            string url = "/Home/EditSimple";
            string path = Helpers.GetResourcePath(@"Editor\mvc-simple.html");

            // Test
            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToStandardizedHtml(minified: false);
#if SAVE
            actual.ToFile(path);
#endif
            string expected = path.ToHtml();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void NestedListEditorFor_ShouldRenderNestedViewModels()
        {
            // Arrange
            string url = "/Home/EditNested";
            string path = Helpers.GetResourcePath(@"Editor\mvc-nested.html");

            // Test
            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToStandardizedHtml(minified: false);
#if SAVE
            actual.ToFile(path);
#endif
            string expected = path.ToHtml();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void RecursiveNestedListEditorFor_ShouldRenderNestedViewModels()
        {
            // Arrange
            string url = "/Home/EditNestedRecursive";
            string path = Helpers.GetResourcePath(@"Editor\mvc-recursive.html");

            // Test
            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToStandardizedHtml(minified: false);
#if SAVE
            actual.ToFile(path);
#endif
            string expected = path.ToHtml();

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}
