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
    public class DisplayTests : IClassFixture<CustomWAF<Startup>>
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;

        public DisplayTests(CustomWAF<Startup> factory)
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
            string path = @"Resources\Display\simple.html";
            string expected = path.ToHtml();

            // Test
            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToHtml(minified: false);
            //actual.ToFile(path);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void NestedListDisplayFor_ShouldRenderNestedViewModels()
        {
            // Arrange
            string url = "/Home/GetNested";
            string path = @"Resources\Display\nested.html";
            string expected = path.ToHtml();

            // Test
            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToHtml(minified: false);
            //actual.ToFile(path);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void RecursiveNestedListDisplayFor_ShouldRenderNestedViewModels()
        {
            // Arrange
            string url = "/Home/GetNestedRecursive";
            string path = @"Resources\Display\recursive.html";
            string expected = path.ToHtml();

            // Test
            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToHtml(minified: false);
            //actual.ToFile(path);

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}
