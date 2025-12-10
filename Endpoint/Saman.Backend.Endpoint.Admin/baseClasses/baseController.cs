using Saman.Backend.Share.shareExceptions;
using Saman.Backend.Share.shareServices;
using Microsoft.AspNetCore.Mvc;

namespace Saman.Backend.Endpoint.Admin.baseClasses
{
    public class baseController : ControllerBase
    {
        protected readonly CurrentUser_Service _currentUser;

        /// <summary>
        /// Returns "0" on Unauthorized
        /// </summary>
        protected string _currentUser_Id => _currentUser?.Id ?? "0";

        /// <summary>
        /// Returns true OR throw Exception_Unauthorized
        /// </summary>
        protected bool _currentUser_Authorize => _currentUser.IsAuthenticated ? true : throw new Exception_Unauthorized();

        public baseController(CurrentUser_Service currentUser)
            => _currentUser = currentUser;
    }
}
