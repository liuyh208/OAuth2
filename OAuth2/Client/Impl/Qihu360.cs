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
    /// taobao  OAuth2
    /// </summary>
    public  class Qihu360Client : OAuth2Client
    {
        public Qihu360Client(IRequestFactory factory, IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }

        private string _baseUrl = " https://openapi.360.cn";
        public override string Name
        {
            get { return "360"; }
        }

        protected override Endpoint AccessCodeServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = _baseUrl,
                    Resource = "/oauth2/authorize"
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
                    Resource = "/oauth2/access_token"
                };
            }
        }

        protected override Endpoint UserInfoServiceEndpoint
        {
            get
            {
                //https://openapi.baidu.com/rest/2.0/passport/users/getLoggedInUser
                return new Endpoint
                {
                    BaseUri = "hhttps://openapi.360.cn",
                    Resource = "/user/me.json"
                };
            }
        }

        protected override Models.UserInfo ParseUserInfo(string content)
        {
            var cnt = JObject.Parse(content);

            var portrait = cnt["avatar"].Value<string>();
            var result = new UserInfo
            {

                ProviderName = this.Name,
                Id = cnt["id"].Value<string>(),
                LastName = cnt["name"].SafeGet(t => t.Value<string>()),
                Email = cnt["email"].Value<string>(),
                
                AvatarUri =
                {
                    Small = portrait,
                    Normal = portrait,
                    Large = portrait
                }
            };
            return result;
        }
    }
}
