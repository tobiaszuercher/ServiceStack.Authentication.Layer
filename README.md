ServiceStack.Authentication.Layer
===========

ServiceStack Plugin which generates identity token for www.layer.com.

![NuGet Version](http://img.shields.io/nuget/v/Servicestack.Authentication.Layer.svg)

Branch | Status
:---|:---|:---
Master               | [![Build status](https://ci.appveyor.com/api/projects/status/nuetuwy9go5mif9v/branch/master?svg=true)](https://ci.appveyor.com/project/tobiaszuercher/servicestack-authentication-layer)
Dev                  | [![Build status](https://ci.appveyor.com/api/projects/status/nuetuwy9go5mif9v/branch/master?svg=true)](https://ci.appveyor.com/project/tobiaszuercher/servicestack-authentication-layer)

# Usage

Just add this plugin to your ServiceStack `AppHost`:

```csharp
/// <summary>
/// Configure the container with the necessary routes for your ServiceStack application.
/// </summary>
/// <param name="container">The built-in IoC used with ServiceStack.</param>
public override void Configure(Container container)
{
    // your stuff here

    Plugins.Add(
        new LayerTokenProviderFeature(
            "layer:///keys/xxxx",
            "layer:///providers/xxxx",
            TimeSpan.FromMinutes(30),
            "~/layer.pem".MapAbsolutePath()));
}
```

It will add a route for `/auth/layer/token` with the following DTO:

```csharp
/// <summary>
/// Model to get a layer identity token.
/// </summary>
[DataContract]
[Route("/auth/layer/token", Verbs = "POST")]
public class CreateLayerIdentityToken : IReturn<CreateLayerTokenResponse>
{
    /// <summary>
    /// Nonce created by Layer client
    /// </summary>
    [DataMember(Name = "nonce")]
    public string Nonce { get; set; }

    /// <summary>
    /// User id used to register with Layer server
    /// </summary>
    [DataMember(Name = "user_id")]
    public string UserId { get; set; }
}
```

For more details, please check the excellent documentation by layer.com: https://developer.layer.com/docs/client#authentication.

By default, it uses the ServiceStack built-in session to verify the user id (`UserAuthId`). 
This may be overriden by implementing the `ILayerUserValidation` interface. Don't forget
to register it to the container with `container.RegisterAs<MyUserValidator, ILayerUserValidator>();`

The response will be an `identity token` which can be used to create a layer session documented here: https://developer.layer.com/docs/client#3-obtain-a-session-token

##Contributors
- you can be listed here