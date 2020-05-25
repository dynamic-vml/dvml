# Dynamic View-Model Lists <span align="right" float="right">[![Build status](https://ci.appveyor.com/api/projects/status/77ej7gsuxo10q5hn?svg=true)](https://ci.appveyor.com/project/cesarsouza/dvml) [![Nuget](https://img.shields.io/nuget/dt/DynamicVML?color=green&label=NuGet)](https://www.nuget.org/packages/DynamicVML/) ![MIT](https://img.shields.io/github/license/dynamic-vml/dvml) </span>

<pre>
PM> dotnet add package <b>DynamicVML</b>
</pre>

The *Dynamic View Model Lists* library provides a templating engine to render dynamic item lists in ASP.NET. 
A dynamic list is a list inside an HTML form where the user can add new items to a list after the page 
has been rendered.
In ASP.NET, the default model binder makes certain assumptions to determine the name of the fields in the
form and how they should be posted back to the server. Using this library, those assumptions
are always fulfilled and your forms posted correctly.

This library provides an alternative to the [EditorForMany extension](https://github.com/mattlunn/DynamicListBinding)
by [@mattalun](https://www.mattlunn.me.uk/blog/2014/08/how-to-dynamically-via-ajax-add-new-items-to-a-bound-list-model-in-asp-mvc-net/)
and [BeginCollectionItem](https://www.nuget.org/packages/BeginCollectionItem/) by [Steve Sanderson](http://blog.stevensanderson.com/2010/01/28/editing-a-variable-length-list-aspnet-mvc-2-style/).
However, it supports nested lists of any depth, does not require adding an extra Index field to
your existing models or view models, and suports multiple editor and display templates for your items.
The library can also make your controllers include [additionalViewData](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.rendering.ihtmlhelper-1.editorfor?view=aspnetcore-3.1)
that had been specified in your view without cluttering your controller code with too many details
about the view.

In addition, the library can also handle requests sent by either GETs or POSTs.

-----

For more information on how to use it and for sample applications that can help you get started, check the [project website](https://dynamic-vml.github.io/).
