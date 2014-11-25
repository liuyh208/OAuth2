using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;

namespace OAuth2.Client.Impl
{
    /// <summary>
    /// 京东  OAuth2
    /// </summary>
    public class JDClient : OAuth2Client
    {
        public JDClient(IRequestFactory factory, IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }

        private UserInfo _userInfo;
        private string _baseUrl = "https://auth.360buy.com";
        public override string Name
        {
            get { return "jd"; }
        }

        protected override Endpoint AccessCodeServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = _baseUrl,
                    Resource = "/oauth/authorize"
                };
            }
        }

        protected override Endpoint AccessTokenServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = _baseUrl,
                    Resource = "/oauth/token"
                };
            }
        }

        protected override Endpoint UserInfoServiceEndpoint
        {
            get
            {
                return new Endpoint();
            }
        }
        protected override string ParseAccessTokenResponse(string content)
        {
            _userInfo = ParseUserInfo(content);
            return base.ParseAccessTokenResponse(content);
        }

        protected override UserInfo GetUserInfo()
        {
            return _userInfo;
        }

        protected override Models.UserInfo ParseUserInfo(string content)
        {
            var cnt = JObject.Parse(content);

            var result = new UserInfo
            {
                ProviderName = this.Name,
                Id = cnt["uid"].Value<string>(),
                LastName = cnt["user_nick"].SafeGet(t => t.Value<string>())
            };
            return result;
        }
    }
}
