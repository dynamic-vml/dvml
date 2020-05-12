using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Io;

namespace Tests
{
    public static class Helpers
    {
        public static async Task<IHtmlDocument> GetDocumentAsync(this HttpResponseMessage response,
            IBrowsingContext context = null)
        {
            if (context == null)
                context = BrowsingContext.New();

            var content = await response.Content.ReadAsStringAsync();
            var document = await context.OpenAsync(ResponseFactory, CancellationToken.None);
            return (IHtmlDocument)document;

            void ResponseFactory(VirtualResponse htmlResponse)
            {
                htmlResponse
                    .Address(response.RequestMessage.RequestUri)
                    .Status(response.StatusCode);

                MapHeaders(response.Headers);
                MapHeaders(response.Content.Headers);

                htmlResponse.Content(content);

                void MapHeaders(HttpHeaders headers)
                {
                    foreach (var header in headers)
                        foreach (var value in header.Value)
                            htmlResponse.Header(header.Key, value);
                }
            }
        }

        public static string GetResourcePath(string filePath)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            return Path.Combine(projectDirectory, "Resources", filePath);
        }

        public static string ToHtml(this string filePath)
        {
            using var reader = new StreamReader(filePath, Encoding.ASCII);
            string html = reader.ReadToEnd();
            html = html.Replace("\r\n", "\n");
            return html;
        }

        public static void ToFile(this string html, string filePath)
        {
            using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(file, Encoding.ASCII);
            html = html.Replace("\r\n", "\n");
            writer.Write(html);
        }

        public static string ToStandardizedHtml(this IDocument content, bool minified = false)
        {
            if (minified)
            {
                var writer = new StringWriter();
                content.ToHtml(writer, new MinifyMarkupFormatter()
                {
                    ShouldKeepAttributeQuotes = true,
                    ShouldKeepComments = false,
                    ShouldKeepImpliedEndTag = true,
                    ShouldKeepStandardElements = true,
                    ShouldKeepEmptyAttributes = false
                });
                return writer.ToString();
            }
            else
            {
                string min = ToStandardizedHtml(content, minified: true);
                var parser = new HtmlParser();
                IHtmlDocument document = parser.ParseDocument(min);
                var writer = new StringWriter();
                document.ToHtml(writer, new PrettyMarkupFormatter()
                {
                    Indentation = "  ",
                    NewLine = "\n"
                });
                return writer.ToString();
            }
        }
    }
}
