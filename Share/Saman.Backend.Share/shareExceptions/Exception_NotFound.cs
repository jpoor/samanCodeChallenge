using Saman.Backend.Share.shareExceptions.Rsx;

namespace Saman.Backend.Share.shareExceptions
{
    public class Exception_NotFound : baseException
    {
        public Exception_NotFound(string? message = null)
             : base
             (
                 !string.IsNullOrWhiteSpace(message)
                 ? message
                 : Exception_Rsx.Exception_NotFound
             )
             => this.HResult = 404; // StatusCodes.Status404NotFound;
    }
}
