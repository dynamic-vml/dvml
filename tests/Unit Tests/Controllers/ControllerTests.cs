using System.Net.Http;
using System.Text;

using AngleSharp.Html.Dom;

using DynamicVML;
using DynamicVML.Internals;

using Microsoft.AspNetCore.Mvc.Testing;

using Newtonsoft.Json;

using Xunit;

namespace Tests.MVC
{
    public class ControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;
        private readonly WebApplicationFactory<Startup> factory;

        public ControllerTests(WebApplicationFactory<Startup> factory)
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
                actionUrl: "/Home/AddBook",
                addNewItemText: "Add new bookName",
                additionalViewData: null,
                method: NewItemMethod.Get);

            string url = e.GetActionInfo();
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
                actionUrl: "/Home/AddBookWithParameterByGet?integerParameter=42&stringParameter=test",
                addNewItemText: "Add new bookName",
                additionalViewData: null,
                method: NewItemMethod.Get);

            string url = e.GetActionInfo();
            Assert.Equal("/Home/AddBookWithParameterByGet" +
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

        [Fact]
        public async void AddNewItem_ShouldIncludeControllerParameters_Post()
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
                actionUrl: "/Home/AddBookWithParameterByPost?integerParameter=42&stringParameter=test",
                addNewItemText: "Add new bookName",
                additionalViewData: new { extraData = "myData" },
                method: NewItemMethod.Post);

            string url = e.GetActionInfo();
            Assert.Equal("POST" +
                "|" +
                "/Home/AddBookWithParameterByPost" +
                "?integerParameter=42" +
                "&stringParameter=test" +
                "|" +
                "{\"ContainerId\":\"4TUAPqX4s0SDU9fjH3oaFA\"," +
                "\"ItemTemplate\":\"Book\"," +
                "\"ItemContainerTemplate\":\"DynamicItemContainer\"," +
                "\"ListTemplate\":\"EditorTemplates/DynamicList\"," +
                "\"Prefix\":\"Namespace1.Namespace2.Namespace3\"," +
                "\"Mode\":0," +
                "\"AdditionalViewData\":\"eyJleHRyYURhdGEiOiJteURhdGEifQ==\"}",
                url);

            // Call the controller action manually
            var parts = url.Split("|");
            var response = await client.PostAsync(parts[1],
                new StringContent(parts[2], Encoding.UTF8, "application/json"));

            var jsonResponse = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), new
            {
                success = false,
                html = ""
            });

            Assert.True(jsonResponse.success);

            var content = await Helpers.GetDocumentAsync(jsonResponse.html);
            var actual = content.ToStandardizedHtml(minified: false);

            // Assert
            Assert.Contains("New book via POST with parameters: 42 test", actual);

            string htmlPrefix = prefix.Replace(".", "_");
            string itemId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_Index")).Value;
            Assert.NotEmpty(itemId);
            string actualId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_ContainerId")).Value;
            Assert.Equal(containerId, actualId);

            Assert.NotEqual(itemId, containerId);
        }

        [Fact]
        public async void AddNewItemWithCustomItemTemplate_ShouldIncludeControllerParameters_Post()
        {
            string prefix = "Namespace1.Namespace2.Namespace3";
            string containerId = "4TUAPqX4s0SDU9fjH3oaFA";

            // Act
            var e = new ListEditorParameters(
                parameters: new ListParameters(
                                containerId: containerId,
                                itemTemplate: "Book",
                                itemContainerTemplate: "CustomItemTemplateWithTestOptions",
                                listTemplate: "EditorTemplates/" + Constants.DefaultListTemplate,
                                prefix: prefix,
                                mode: ListRenderMode.ViewModelOnly),
                actionUrl: "/Home/AddBookWithOptionsAndParameterByPost?integerParameter=42&stringParameter=test",
                addNewItemText: "Add new bookName",
                additionalViewData: new { extraData = "testData" },
                method: NewItemMethod.Post);

            string url = e.GetActionInfo();
            Assert.Equal("POST" +
                "|" +
                "/Home/AddBookWithOptionsAndParameterByPost" +
                "?integerParameter=42" +
                "&stringParameter=test" +
                "|" +
                "{\"ContainerId\":\"4TUAPqX4s0SDU9fjH3oaFA\"," +
                "\"ItemTemplate\":\"Book\"," +
                "\"ItemContainerTemplate\":\"CustomItemTemplateWithTestOptions\"," +
                "\"ListTemplate\":\"EditorTemplates/DynamicList\"," +
                "\"Prefix\":\"Namespace1.Namespace2.Namespace3\"," +
                "\"Mode\":0," +
                "\"AdditionalViewData\":\"eyJleHRyYURhdGEiOiJ0ZXN0RGF0YSJ9\"}",
                url);

            // Call the controller action manually
            var parts = url.Split("|");
            var response = await client.PostAsync(parts[1],
                new StringContent(parts[2], Encoding.UTF8, "application/json"));

            var jsonResponse = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), new
            {
                success = false,
                html = ""
            });

            Assert.True(jsonResponse.success);

            var content = await Helpers.GetDocumentAsync(jsonResponse.html);
            var actual = content.ToStandardizedHtml(minified: false);

            // Assert
            Assert.Contains("New book via POST with parameters: 42 test", actual);
            Assert.Contains("OptionsTestText with extra data: 'testData'", actual);

            string htmlPrefix = prefix.Replace(".", "_");
            string itemId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_Index")).Value;
            Assert.NotEmpty(itemId);
            string actualId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_ContainerId")).Value;
            Assert.Equal(containerId, actualId);

            Assert.NotEqual(itemId, containerId);
        }


        [Fact]
        public async void AddNewItemWithCustomItemTemplate_ShouldIncludeControllerParameters_Get()
        {
            string prefix = "Namespace1.Namespace2.Namespace3";
            string containerId = "4TUAPqX4s0SDU9fjH3oaFA";

            // Act
            var e = new ListEditorParameters(
                parameters: new ListParameters(
                                containerId: containerId,
                                itemTemplate: "Book",
                                itemContainerTemplate: "CustomItemTemplateWithTestOptions",
                                listTemplate: "EditorTemplates/" + Constants.DefaultListTemplate,
                                prefix: prefix,
                                mode: ListRenderMode.ViewModelOnly),
                actionUrl: "/Home/AddBookWithOptionsAndParameterByGet?integerParameter=42&stringParameter=test",
                addNewItemText: "Add new bookName",
                additionalViewData: new { extraData = "testData" },
                method: NewItemMethod.Get);

            string url = e.GetActionInfo();
            Assert.Equal("/Home/AddBookWithOptionsAndParameterByGet" +
                "?integerParameter=42" +
                "&stringParameter=test" +
                "&ContainerId=4TUAPqX4s0SDU9fjH3oaFA" +
                "&ListTemplate=EditorTemplates%2fDynamicList" +
                "&ItemContainerTemplate=CustomItemTemplateWithTestOptions" +
                "&ItemTemplate=Book" +
                "&Prefix=Namespace1.Namespace2.Namespace3" +
                "&Mode=0",
                url);


            var response = await client.GetAsync(url);
            var content = await Helpers.GetDocumentAsync(response);
            var actual = content.ToStandardizedHtml(minified: false);

            // Assert
            Assert.Contains("New book via GET with parameters: 42 test", actual);
            Assert.Contains("OptionsTestText with extra data: ''", actual);

            string htmlPrefix = prefix.Replace(".", "_");
            string itemId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_Index")).Value;
            Assert.NotEmpty(itemId);
            string actualId = ((IHtmlInputElement)content.QuerySelector($"#{htmlPrefix}_ContainerId")).Value;
            Assert.Equal(containerId, actualId);

            Assert.NotEqual(itemId, containerId);
        }

        [Fact]
        public async void AddNewItemWitWrongItemTemplate_ShouldThrow_Post()
        {
            string prefix = "Namespace1.Namespace2.Namespace3";
            string containerId = "4TUAPqX4s0SDU9fjH3oaFA";

            // Act
            var e = new ListEditorParameters(
                parameters: new ListParameters(
                                containerId: containerId,
                                itemTemplate: "Book",
                                itemContainerTemplate: "CustomItemTemplateWithWrongType",
                                listTemplate: "EditorTemplates/" + Constants.DefaultListTemplate,
                                prefix: prefix,
                                mode: ListRenderMode.ViewModelOnly),
                actionUrl: "/Home/AddBookWithOptionsAndParameterByPost?integerParameter=42&stringParameter=throw",
                addNewItemText: "Add new bookName",
                additionalViewData: new { extraData = "testData" },
                method: NewItemMethod.Post);

            string url = e.GetActionInfo();

            try
            {
                // Call the controller action manually
                var parts = url.Split("|");
                var response = await client.PostAsync(parts[1],
                    new StringContent(parts[2], Encoding.UTF8, "application/json"));

                var jsonResponse = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), new
                {
                    success = true,
                    html = ""
                });

                Assert.False(jsonResponse.success);

                var content = await Helpers.GetDocumentAsync(jsonResponse.html);
                var actual = content.ToStandardizedHtml(minified: false);

                // Assert
                Assert.Contains("Failed to render the dynamic list item. If you are using custom " +
                    "templates with options, make sure you have passed the options object when " +
                    "calling PartialViewAsync.", actual);
            }
            catch (DynamicListException ex)
            {
                Assert.Contains("Error rendering item container template 'CustomItemTemplateWithWrongType'", ex.Message);
            }
        }

        [Fact]
        public async void AddNewItemWitWrongItemTemplate_ShouldThrow_Get()
        {
            string prefix = "Namespace1.Namespace2.Namespace3";
            string containerId = "4TUAPqX4s0SDU9fjH3oaFA";

            // Act
            var e = new ListEditorParameters(
                parameters: new ListParameters(
                                containerId: containerId,
                                itemTemplate: "Book",
                                itemContainerTemplate: "CustomItemTemplateWithWrongType",
                                listTemplate: "EditorTemplates/" + Constants.DefaultListTemplate,
                                prefix: prefix,
                                mode: ListRenderMode.ViewModelOnly),
                actionUrl: "/Home/AddBookWithOptionsAndParameterByGet?integerParameter=42&stringParameter=throw",
                addNewItemText: "Add new bookName",
                additionalViewData: new { extraData = "testData" },
                method: NewItemMethod.Get);

            string url = e.GetActionInfo();

            try
            {
                var response = await client.GetAsync(url);
                var content = await Helpers.GetDocumentAsync(response);
                var actual = content.ToStandardizedHtml(minified: false);

                // Assert
                Assert.Contains("DynamicVML.DynamicListException: Error rendering item container template 'CustomItemTemplateWithWrongType'" +
                    " for edit. Check the inner exception and other properties of this exception for details. Message: The model item passed" +
                    " into the ViewDataDictionary is of type 'Tests.ViewModels.TestOptions`1[Tests.BookViewModel]'," +
                    " but this ViewDataDictionary instance requires a model item of type 'Tests.ViewModels.IWrongOptions'.", actual);
            }
            catch (DynamicListException ex)
            {
                Assert.Contains("Error rendering item container template 'CustomItemTemplateWithWrongType'", ex.Message);
            }
        }
    }
}
