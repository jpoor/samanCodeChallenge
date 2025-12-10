using Saman.Backend.Share.shareExceptions.Rsx;

namespace Saman.Backend.Share.shareExceptions
{
    public class Exception_AccessDenied : baseException
    {
        public Exception_AccessDenied(string? message = null)
             : base
             (
                 !string.IsNullOrWhiteSpace(message)
                 ? message
                 : Exception_Rsx.Exception_AccessDenied
             )
             => this.HResult = 405; //StatusCodes.Status405MethodNotAllowed;
    }
}
