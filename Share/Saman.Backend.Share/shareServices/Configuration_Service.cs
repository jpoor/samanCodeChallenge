using Microsoft.Extensions.Configuration;

namespace Saman.Backend.Share.shareServices
{
    public class Configuration_Service
    {
        private readonly IConfiguration _configuration;

        public Configuration_Service(IConfiguration configuration) => _configuration = configuration;

       
        public string JWT_TokenTimeoutMinutes => _configuration["JWT:TokenTimeoutMinutes"] ?? "";

        public string JWT_Secret => _configuration["JWT:Secret"] ?? "";

        public string JWT_ValidIssuer => _configuration["JWT:ValidIssuer"] ?? "";

        public string JWT_ValidAudience => _configuration["JWT:ValidAudience"] ?? "";

    }
}
