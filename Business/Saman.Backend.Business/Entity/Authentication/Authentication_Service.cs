using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Saman.Backend.Business.baseServices;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareExceptions;
using Saman.Backend.Share.shareServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Saman.Backend.Business.Entity.Authentication
{
    public class Authentication_Service : baseService
    {
        protected readonly UserManager<User_dBo> _userManager;
        private readonly Configuration_Service _configurationService;

        public Authentication_Service(
            UserManager<User_dBo> userManager,
            Configuration_Service configurationService,
            CRUD_Service crudService)
        : base(crudService)
        {
            _userManager = userManager;
            _configurationService = configurationService;
        }


        private List<Claim> CommonClaims(User_dBo user)
             => new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim(ClaimTypes.UserData, shareConvertor.JsonSerialize(new User_dTo(user))),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

        private List<Claim> RoleClaims(IList<string> userRoles)
            => userRoles
              .Select(x => new Claim(ClaimTypes.Role, x))
              .ToList();


        public async Task<Token_dTo> GET_Token(User_dBo user, bool roleContains = false)
        {
            // Set User Info
            var authClaims = CommonClaims(user);

            // Admin panel
            if (roleContains)
            {
                // Set user Roles
                authClaims.AddRange(RoleClaims(await _userManager.GetRolesAsync(user)));
            }

            // Generate Token
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationService.JWT_Secret));

            var token = new JwtSecurityToken(
                issuer: _configurationService.JWT_ValidIssuer,
                audience: _configurationService.JWT_ValidAudience,
                expires: DateTime.Now.AddMinutes(shareConvertor.double0(_configurationService.JWT_TokenTimeoutMinutes)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
              );

            return new Token_dTo(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
        }

        public IQueryable<User_dBo> User_GET(objFiltering? filtering = null)
        {
            var dbos = _userManager.Users;

            // searching
            if (!string.IsNullOrEmpty(filtering?.SearchValue))
            {
                var searchValue = filtering.SearchValue.ToUpper();

                dbos = dbos.Where(x =>
                       (x.Id.Equals(searchValue))
                    || (x.UserName != null && x.UserName.Contains(searchValue))
                    || (x.NormalizedEmail != null && x.NormalizedEmail.Contains(searchValue))
                    || (x.PhoneNumber != null && x.PhoneNumber.Contains(searchValue)));
            }

            return dbos;
        }

        public async Task<User_dBo> User_GET(string id)
            => await _userManager.FindByIdAsync(id)
            ?? throw new Exception_NotFound();

        public async Task<User_dBo?> User_FindByUsername(string username)
            => await _userManager.FindByNameAsync(username);

        public async Task<User_dBo?> User_FindByEmail(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task<User_dBo?> User_FindByIdentificationCode(string identificationCode)
            => await _userManager.Users.FirstOrDefaultAsync(x => x.IdentificationCode == identificationCode);

        public async Task<User_dBo?> User_FindByPhoneNumber(string phoneNumber)
            => await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);

        public async Task<IList<string>> User_Roles(User_dBo user)
            => await _userManager.GetRolesAsync(user);

        public async Task<bool> User_AddToRole(User_dBo user, string rolename)
            => await _userManager.IsInRoleAsync(user, rolename)
             ? true
             : await _userManager.AddToRoleAsync(
                user,
                rolename)
            is not null;

        public async Task<bool> User_RemoveFromRole(User_dBo user, string rolename)
            => !await _userManager.IsInRoleAsync(user, rolename)
              ? true
              : await _userManager.RemoveFromRoleAsync(
                 user,
                 rolename)
            is not null;

        public async Task<bool> User_IsEmailConfirmed(User_dBo user)
            => await _userManager.IsEmailConfirmedAsync(user);

        public async Task<bool> User_IsPhoneNumberConfirmed(User_dBo user)
            => await _userManager.IsPhoneNumberConfirmedAsync(user);

        public async Task<bool> User_CheckPassword(User_dBo user, string password)
            => await _userManager.CheckPasswordAsync(user, password);
    }
}
