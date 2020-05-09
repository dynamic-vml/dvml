
# Sample applications

You can find sample applications on the project's [GitHub page](https://github.com/dynamic-vml/) which can help
demonstrate how this library can be used. Those are:



### Books and authors 

 - Browse the source: [dynamic-vml/sample-netcoreapp3.1-books-and-authors-simple](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-simple)
 - Download it as a .zip: [master.zip](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-simple/archive/master.zip)

This sample application [demonstrates](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-simple/blob/master/Controllers/AuthorsController.cs#L56) how to consume the DynamicVML RCL from a .NET Core App 3.1 application
running ASP.NET MVC for presentation and Entity Framework Core for persistence. It consists of a simple CRUD
example where you can list, add, edit, and delete book authors and include details about the books they have
written. The books are added and removed from the CRUD pages using DynamicVML.


### Books and authors (customized layouts)

 - Browse the source: [dynamic-vml/sample-netcoreapp3.1-books-and-authors-options](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-options)
 - Download it as a .zip: [master.zip](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-options/archive/master.zip)

This version of the sample application shows [how to customize](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-options/blob/master/Views/Authors/Create.cshtml#L30) 
the [layouts of the lists](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-options/blob/master/Views/Authors/EditorTemplates/CustomItemTemplateWithMyOptions.cshtml) and how to create [view 
models for display options](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-options/blob/master/ViewModels/MyOptions.cs) 
associated with each item in a list, e.g., titles and subtitles for [Bootstrap Cards](https://getbootstrap.com/docs/4.0/components/card/).


### Books and authors (parameterized templates) 

 - Browse the source: [dynamic-vml/sample-netcoreapp3.1-books-and-authors-parameters](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-parameters)
 - Download it as a .zip: [master.zip](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-parameters/archive/master.zip)

This version of the sample application shows how to [create parameterized templates](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-parameters/blob/master/Views/Authors/EditorTemplates/CustomItemTemplateWithMyOptions.cshtml) that can have their behaviour
customized depending on [additional view data](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-parameters/blob/master/Views/Authors/Create.cshtml#L38) specified inside the view (.cshtml). This sample also shows how to
make the user request new items to be added to the list [via POST](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-parameters/blob/master/Controllers/AuthorsController.cs#L79).



# Razor Class Libraries (RCL)

Those examples, together with the 
[library iself](https://github.com/dynamic-mvl/dvml), 
can also serve as an example on how to create a reusable 
[Razor Class Library](https://docs.microsoft.com/en-us/aspnet/core/blazor/class-libraries?view=aspnetcore-3.1&tabs=visual-studio) 
(RCL) that contain 
[static resources](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/static-files?view=aspnetcore-3.1)
and consume it from your applications in .NET Core 3.1. There is 
[currently](https://stackoverflow.com/questions/59431923/cant-reference-static-files-from-rcl) 
[some](https://stackoverflow.com/questions/51610513/can-razor-class-library-pack-static-files-js-css-etc-too?rq=1) 
[confusion](https://stackoverflow.com/questions/60040793/404-with-static-content-in-razor-class-library-rcl)
on how to achieve this due the significant amount of 
[outdated](https://dotnetstories.com/blog/How-to-Include-static-files-in-a-Razor-library-en-7156675136)
[tutorials](https://www.learnrazorpages.com/advanced/razor-class-library) and 
[Stackoverflow answers](https://stackoverflow.com/a/53241012/262032) out there.

 - Here is the [.csproj configuration for the library](https://github.com/dynamic-vml/dvml/blob/master/src/DynamicVML.csproj).
 - Here are the [Program.cs](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-simple/blob/master/Program.cs)
and [Startup.cs](https://github.com/dynamic-vml/sample-netcoreapp3.1-books-and-authors-simple/blob/master/Startup.cs) for the consumer applications.

>[!TIP]
Never use the [standalone nuget.exe](https://www.nuget.org/downloads) 
to pack Razor libraries. Always use [dotnet nuget pack](https://docs.microsoft.com/en-us/nuget/reference/dotnet-commands)
instead. They are completely different beasts.