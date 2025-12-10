using Saman.Backend.Share.shareExceptions.Rsx;

namespace Saman.Backend.Share.shareExceptions
{
    public class Exception_Unauthorized : baseException
    {
        public Exception_Unauthorized(string? message = null)
             : base
             (
                 !string.IsNullOrWhiteSpace(message)
                 ? message
                 : Exception_Rsx.Exception_Unauthorized
             )
             => this.HResult = 401; // StatusCodes.Status401Unauthorized;
    }
}
