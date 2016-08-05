namespace ServiceStack.Authentication.Layer
{
    /// <summary>
    /// By default, the user will be validated against the ServiceStack session.
    /// </summary>
    public class ServiceStackSessionUserValidator : ILayerUserValidator
    {
        public bool Validate(string requestedUserId)
        {
            var session = SessionFeature.GetOrCreateSession<AuthUserSession>();

            return !string.IsNullOrEmpty(requestedUserId) &&
                   session.UserAuthId == requestedUserId;
        }
    }
}