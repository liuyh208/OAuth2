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
    /// 人人网  OAuth2
    /// </summary>
    public  class RenRenClient : OAuth2Client
    {
        public RenRenClient(IRequestFactory factory, IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }

        private UserInfo _userInfo;
        private string _baseUrl = "https://graph.renren.com";
        public override string Name
        {
            get { return "RenRen"; }
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
                ////https://openapi.baidu.com/rest/2.0/passport/users/getLoggedInUser
                //return new Endpoint
                //{
                //    BaseUri = "hhttps://openapi.360.cn",
                //    Resource = "/user/me.json"
                //};
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
            var user = cnt["user"];
            var portrait = user["avatar"];
            var result = new UserInfo
            {

                ProviderName = this.Name,
                Id = user["id"].Value<string>(),
                LastName = user["name"].SafeGet(t => t.Value<string>()),
                
                AvatarUri =
                {
                    Small = portrait["tiny"]["url"].SafeGet(t => t.Value<string>()),
                    Normal = portrait["main"]["url"].SafeGet(t => t.Value<string>()),
                    Large = portrait["large"]["url"].SafeGet(t => t.Value<string>()),
                }
            };
            return result;
        }
    }
}
