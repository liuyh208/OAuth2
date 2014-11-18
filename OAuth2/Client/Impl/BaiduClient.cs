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
    /// baidu  OAuth2
    /// </summary>
    public  class BaiduClient : OAuth2Client
    {
        public BaiduClient(IRequestFactory factory, IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }
        public override string Name
        {
            get { return "Baidu"; }
        }

        protected override Endpoint AccessCodeServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://openapi.baidu.com",
                    Resource = "/oauth/2.0/authorize"
                };
            }
        }

        protected override Endpoint AccessTokenServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://openapi.baidu.com",
                    Resource = "/oauth/2.0/token"
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
                    BaseUri = "https://openapi.baidu.com",
                    Resource = "/rest/2.0/passport/users/getInfo"
                };
            }
        }

        protected override Models.UserInfo ParseUserInfo(string content)
        {
            var cnt = JObject.Parse(content);
            var names = cnt["username"].SafeGet(t => t.Value<string>());
            var portrait = cnt["portrait"].SafeGet(t => t.Value<string>());
            var result = new UserInfo
            {

                ProviderName = this.Name,
                Id = cnt["userid"].Value<int>().ToString(),
                LastName =  cnt["name"].SafeGet(t => t.Value<string>()),
                AvatarUri =
                {
                    Small = string.Format("http://tb.himg.baidu.com/sys/portraitn/item/{0}", portrait),
                    Normal = string.Format("http://tb.himg.baidu.com/sys/portraitn/item/{0}", portrait),
                    Large = string.Format("http://tb.himg.baidu.com/sys/portrait/item/{0}",portrait)
                }
            };
            return result;
        }
    }
}
