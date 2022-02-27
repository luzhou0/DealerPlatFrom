using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DearlerPlatform.Common.TokenModule.Models
{
    public class JwtTokenModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expires { get; set; }
        //SecurityKey 的长度必须大于 16个字符
        public string Security { get; set; }
        public int Id { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
    }
    // iss (issuer)：签发人

    // exp (expiration time)：过期时间

    // sub (subject)：主题

    // aud (audience)：受众

    // nbf (Not Before)：生效时间

    // iat (Issued At)：签发时间

    // jti (JWT ID)：编号
}