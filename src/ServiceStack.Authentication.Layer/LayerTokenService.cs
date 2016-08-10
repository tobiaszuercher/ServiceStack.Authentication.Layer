using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

using ServiceStack.Authentication.Layer.ServiceModel;   
using ServiceStack.Text;

namespace ServiceStack.Authentication.Layer
{
    public class LayerTokenService : Service
    {
        private readonly ILayerUserValidator _validator;

        public LayerTokenService(ILayerUserValidator validator)
        {
            _validator = validator;
        }

        public CreateLayerTokenResponse Post(CreateLayerIdentityToken request)
        {
            // validate if the user is authenticated
            if (!_validator.Validate(request.UserId))
                throw HttpError.Forbidden("This user is not allowed to create a validation token for the requested user.");

            var feature = HostContext.GetPlugin<LayerTokenProviderFeature>();

            var header = new JsonObject
            {
                {"typ", "JWT"},
                {"alg", "RS256"},
                {"cty", "layer-eit;v=1"}, // String - Express a Content Type of Layer External Identity Token, version 1
                {"kid", feature.KeyName}
            };

            var body = new JsonObject
            {
                {"iss", feature.ProviderId},
                {"prn", request.UserId},
                {"iat", DateTime.UtcNow.ToUnixTime().ToString()},
                {"exp", DateTime.UtcNow.Add(feature.ExpiresIn).ToUnixTime().ToString()},
                {"nce", request.Nonce}
            };

            var headerBytes = header.ToJson().ToUtf8Bytes();
            var headerEncoded = headerBytes.ToBase64UrlSafe();

            var payloadBytes = body.ToJson().ToUtf8Bytes();
            var payloadEncoded = payloadBytes.ToBase64UrlSafe();

            var base64Header = headerBytes.ToBase64UrlSafe();
            var base64Payload = payloadBytes.ToBase64UrlSafe();

            var stringToSign = base64Header + "." + base64Payload;
            
            using (var sr = new StreamReader(feature.PemPath))
            {
                var pr = new PemReader(sr);
                var keyPair = (AsymmetricCipherKeyPair) pr.ReadObject();
                var rsaPrivate = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters) keyPair.Private);

                var rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(rsaPrivate);
                var inputBytes = Encoding.UTF8.GetBytes(stringToSign);
                var signatureBytesTemp = rsa.SignData(inputBytes, new SHA256CryptoServiceProvider());

                var token = string.Join(".", headerEncoded, payloadEncoded, signatureBytesTemp.ToBase64UrlSafe());

                return new CreateLayerTokenResponse() { IdentityToken = token };
            }
        }
    }
}