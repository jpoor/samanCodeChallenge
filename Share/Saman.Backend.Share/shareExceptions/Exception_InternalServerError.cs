using Saman.Backend.Share.shareExceptions.Rsx;

namespace Saman.Backend.Share.shareExceptions
{
    public class Exception_InternalServerError : baseException
    {
        public Exception_InternalServerError(string? message = null)
             : base
             (
                 !string.IsNullOrWhiteSpace(message)
                 ? message
                 : Exception_Rsx.Exception_InternalServerError
             )
             => this.HResult = 500; // StatusCodes.Status500InternalServerError;
    }
}
