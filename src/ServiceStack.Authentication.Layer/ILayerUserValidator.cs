namespace ServiceStack.Authentication.Layer
{
    public interface ILayerUserValidator
    {
        bool Validate(string requestedUserId);
    }
}