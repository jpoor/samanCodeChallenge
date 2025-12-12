using Microsoft.AspNetCore.Mvc;
using Saman.Backend.Business.Entity.Authentication;
using Saman.Backend.Business.Entity.Authentication.Rsx;
using Saman.Backend.Endpoint.Admin.baseClasses;
using Saman.Backend.Endpoint.Admin.Models;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareExceptions;
using Saman.Backend.Share.shareServices;

namespace Saman.Backend.Endpoint.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : baseController
    {
        private readonly Authentication_Service _authService;

        public AuthenticationController(
            CurrentUser_Service currentUser,
            Authentication_Service authService)
        : base(currentUser)
            => _authService = authService;

        // POST: api/Authentication/login
        [HttpPost("login")]
        public async Task<IActionResult> Login_POST([FromBody] AuthModel_Login dto)
        {
            // refactore username and password
            var username = shareConvertor.strFarsi(dto.Username ?? "");
            var password = shareConvertor.strFarsi(dto.Password ?? "");

            // find user
            var user = await _authService.User_FindByUsername(username)
                    ?? throw new Exception_BadRequest(Authentication_Rsx.Exception_InvalidUsernameOrPassword);

            // Check password
            bool passwordIsValid = await _authService.User_CheckPassword(user, password);
            if (!passwordIsValid) { throw new Exception_BadRequest(Authentication_Rsx.Exception_InvalidUsernameOrPassword); }

            // Check user has roles
            var userRoles = await _authService.User_Roles(user);
            if (userRoles.Count == 0)
                throw new Exception_BadRequest(Authentication_Rsx.Exception_InvalidRoleName);

            // Return Token
            var result = await _authService.GET_Token(user, true);

            return Ok(result);
        }
    }
}
