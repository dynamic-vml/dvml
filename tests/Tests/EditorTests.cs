//#define SAVE

using System.Net.Http;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

namespace Tests
{
    public class EditorTests : IClassFixture<CustomWAF<Startup>>
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;

        public EditorTests(CustomWAF<Startup> factory)
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
            string path = Helpers.GetResourcePath(@"Editor\simple.html");
            string expected = path.ToHtml();

            // Test
            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToHtml(minified: false);
#if SAVE
            actual.ToFile(path);
#endif

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void NestedListEditorFor_ShouldRenderNestedViewModels()
        {
            // Arrange
            string url = "/Home/EditNested";
            string path = Helpers.GetResourcePath(@"Editor\nested.html");
            string expected = path.ToHtml();

            // Test
            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToHtml(minified: false);
#if SAVE
            actual.ToFile(path);
#endif

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void RecursiveNestedListEditorFor_ShouldRenderNestedViewModels()
        {
            // Arrange
            string url = "/Home/EditNestedRecursive";
            string path = Helpers.GetResourcePath(@"Editor\recursive.html");
            string expected = path.ToHtml();

            // Test
            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToHtml(minified: false);
#if SAVE
            actual.ToFile(path);
#endif

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}
