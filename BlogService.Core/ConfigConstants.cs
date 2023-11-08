using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogService.Core
{
    public class ConfigConstants
    {
        public const string SectionName = "JWT";
        public const string JWTSecretKey = $"{SectionName}:Key";
        public const string JWTIssuer = $"{SectionName}:Issuer";
        public const string JWTAudience = $"{SectionName}:Audience";

        public const string CacheConnectionString = "CacheSettings:ConnectionString";
    }
}
