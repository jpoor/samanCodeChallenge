using Saman.Backend.Share.shareExceptions.Rsx;

namespace Saman.Backend.Share.shareExceptions
{
    public abstract class baseException : Exception
    {
        protected baseException(string? message)
            : base(message) { }

        protected baseException(string? message, Exception? inner)
            : base(message, inner)
            => this.HResult = 500; // StatusCodes.Status500InternalServerError;

        public string DefaultMessage()
            => Exception_Rsx.ResourceManager.GetString(GetType().Name) 
            ?? Exception_Rsx.Exception_UnhandledError;
    }
}
