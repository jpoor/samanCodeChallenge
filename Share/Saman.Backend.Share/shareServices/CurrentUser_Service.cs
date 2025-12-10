using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Saman.Backend.Share.shareServices
{
    public class CurrentUser_Service
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser_Service(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        private ClaimsPrincipal? _contextUser => (_httpContextAccessor.HttpContext != null) ? _httpContextAccessor.HttpContext.User : null;

        public bool IsAuthenticated => _contextUser?.Identity?.IsAuthenticated ?? false;

        public string UserName => IsAuthenticated ? _contextUser?.Identity?.Name ?? string.Empty : string.Empty;

        public string Id => IsAuthenticated ? _contextUser!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "0" : "0";

        public string IdentificationCode => IsAuthenticated ? _contextUser!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value ?? "" : "";

        public List<string>? Roles => IsAuthenticated ? _contextUser!.Claims.Where(c => c.Type == ClaimTypes.Role).Select(s => s.Value).ToList() : null;

        public bool IsInRole(string roleName) => IsAuthenticated ? _contextUser!.IsInRole(roleName) : false;

        public string IP => IsAuthenticated ? _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty : string.Empty;
    }
}
