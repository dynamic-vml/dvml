//#define SAVE

using System.Net.Http;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

namespace Tests.MVC
{
    public class BasicDisplayTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;

        public BasicDisplayTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            this.client = this.factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async void ListDisplayFor_ShouldRenderViewModels()
        {
            // Arrange
            string url = "/Home/GetSimple";
            string path = Helpers.GetResourcePath(@"Display\mvc-simple.html");

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
        public async void NestedListDisplayFor_ShouldRenderNestedViewModels()
        {
            // Arrange
            string url = "/Home/GetNested";
            string path = Helpers.GetResourcePath(@"Display\mvc-nested.html");

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
        public async void RecursiveNestedListDisplayFor_ShouldRenderNestedViewModels()
        {
            // Arrange
            string url = "/Home/GetNestedRecursive";
            string path = Helpers.GetResourcePath(@"Display\mvc-recursive.html");

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
