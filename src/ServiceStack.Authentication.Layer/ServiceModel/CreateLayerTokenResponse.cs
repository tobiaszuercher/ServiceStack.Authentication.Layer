using System.Runtime.Serialization;

namespace ServiceStack.Authentication.Layer.ServiceModel
{
    [DataContract]
    public class CreateLayerTokenResponse
    {
        [DataMember(Name = "identity_token")]
        public string IdentityToken { get; set; }
    }
}