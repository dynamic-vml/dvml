//#define SAVE

using System.Net.Http;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

namespace Tests.RazorPages
{
    public class BasicEditorPageTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;

        public BasicEditorPageTests(WebApplicationFactory<Startup> factory)
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
            string url = "/EditSimple";
            string path = Helpers.GetResourcePath(@"Editor\razor-simple.html");

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
