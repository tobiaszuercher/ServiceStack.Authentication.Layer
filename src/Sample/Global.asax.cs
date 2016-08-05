using System;
using Funq;
using ServiceStack;
using ServiceStack.Authentication.Layer;

namespace Sample
{
    /// <summary>
    /// Create your ServiceStack web service application with a singleton AppHost.
    /// </summary> 
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Initializes a new instance of your ServiceStack application, with the specified name and assembly containing the services.
        /// </summary>
        public AppHost()
            : base("Layer Token Provider Sample Usage", typeof(AppHost).Assembly)
        {
        }

        /// <summary>
        /// Configure the container with the necessary routes for your ServiceStack application.
        /// </summary>
        /// <param name="container">The built-in IoC used with ServiceStack.</param>
        public override void Configure(Container container)
        {
            SetConfig(new HostConfig { DebugMode = true, });

            Plugins.Add(
                new LayerTokenProviderFeature(
                    "layer:///keys/xxxx",
                    "layer:///providers/xxxx",
                    TimeSpan.FromMinutes(30),
                    "~/layer.pem".MapAbsolutePath()));
        }
    }

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //Initialize your application
            new AppHost().Init();
        }
    }
}