using Saman.Backend.Share.shareExceptions.Rsx;

namespace Saman.Backend.Share.shareExceptions
{
    public class Exception_BadRequest : baseException
    {
        public Exception_BadRequest(string? message = null)
             : base
             (
                 !string.IsNullOrWhiteSpace(message)
                 ? message
                 : Exception_Rsx.Exception_BadRequest
             )
             => this.HResult = 400; // StatusCodes.Status400BadRequest;

        public Exception_BadRequest(string? message = null, params string[] invalidParams)
             : base
             (
                  string.Format(
                      (
                       !string.IsNullOrWhiteSpace(message)
                       ? message
                       : Exception_Rsx.Exception_BadRequest
                      )
                      + " {0}",
                      string.Join(',', invalidParams)
                  )
             )
             => this.HResult = 400; // StatusCodes.Status400BadRequest;
    }
}
