using Saman.Backend.Share.shareExceptions.Rsx;

namespace Saman.Backend.Share.shareExceptions
{
    public class Exception_Forbidden : baseException
    {
        public Exception_Forbidden(string? message = null)
             : base
             (
                 !string.IsNullOrWhiteSpace(message)
                 ? message
                 : Exception_Rsx.Exception_Forbidden
             )
             => this.HResult = 403; // StatusCodes.Status403Forbidden;
    }
}
