using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imgur.API.Models.Impl;

namespace epicture.Model
{
    public class UserInfo
    {
        public OAuth2Token tokenObject { get; set; }
        public String Id { get; set; }
     public String UserName { get; set; }
     public String AccessToken { get; set; }
     public DateTimeOffset ExpiresAt { get; set; }
     public String RefreshToken { get; set; }
    }
}
