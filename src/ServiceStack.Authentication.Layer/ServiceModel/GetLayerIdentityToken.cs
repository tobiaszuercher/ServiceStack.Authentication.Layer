using System.Runtime.Serialization;

namespace ServiceStack.Authentication.Layer.ServiceModel
{
    /// <summary>
    /// Model to get a layer identity token.
    /// </summary>
    [DataContract]
    [Route("/auth/layer/token", Verbs = "GET")]
    public class GetLayerIdentityToken : IReturn<GetLayerTokenResponse>
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
}