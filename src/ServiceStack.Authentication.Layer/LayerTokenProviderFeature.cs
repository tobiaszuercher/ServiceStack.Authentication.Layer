using System;

namespace ServiceStack.Authentication.Layer
{
    public class LayerTokenProviderFeature : IPlugin
    {
        public string KeyName { get; }
        public string ProviderId { get; }
        public TimeSpan ExpiresIn { get; set; }
        public string PemPath { get; set; }

        public LayerTokenProviderFeature(string keyName, string providerId, TimeSpan expiresIn, string pemPath)
        {
            KeyName = keyName;
            ProviderId = providerId;
            ExpiresIn = expiresIn;
            PemPath = pemPath;
        }

        public void Register(IAppHost appHost)
        {
            appHost.RegisterServicesInAssembly(typeof(LayerTokenService).Assembly);

            // if nothing is registered, fallback to the default one.
            if (appHost.TryResolve<ILayerUserValidator>() == null)
            {
                appHost.RegisterAs<ServiceStackSessionUserValidator, ILayerUserValidator>();
            }
        }
    }
}