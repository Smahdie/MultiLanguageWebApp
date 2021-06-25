This is a sample project that shows how to support multiple languages in an ASP.NET web application. The project is created using .NET 5 and MVC framework.

**1. Startup**

First, we need to configure our startup file to handle request localization. There are comments in startup file which explain each part.

In the startup file, we introduce two files: RouteValueRequestCultureProvider and CultureRouteConstraint. The first one shows the app how to detect culture of the request, and the second one introduces a constraint for requests: Requests with unsupported cultures would be rejectd. If for example I navigate to address: /tr/privacy, I will get a 404 error.

**2. Resource Files**

2.1. Shared Resource

 We can have a shared resource which is available in all of views and controllers. To support shared resource, we need to introduce a dummy class. In this project I named it **SharedResource**. Then we can create resource files which have the same name as our dummy class. You can see in HomeController and _Layout view how it is used.

2.2. Other Resources

We can have seperate resource file for each view, model and controller. I found it difficult to manage seperate files for views and controllers, and prefer the shared resource. But this method is good for translating models, specially because it's the only way I found to translate data annotation messages. 
