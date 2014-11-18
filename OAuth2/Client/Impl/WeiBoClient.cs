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
    /// sina Weibo OAuth
    /// </summary>
    class WeiboClient : OAuth2Client
    {
        public WeiboClient(IRequestFactory factory, IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }
        public override string Name
        {
            get { return "Weibo"; }
        }

        protected override Endpoint AccessCodeServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://api.weibo.com",
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
                    BaseUri = "https://api.weibo.com",
                    Resource = "/oauth2/access_token"
                };
            }
        }

        protected override Endpoint UserInfoServiceEndpoint
        {
            get
            {
                //https://api.weibo.com/2/users/domain_show.json
                return new Endpoint
                {
                    BaseUri = "https://api.weibo.com",
                    Resource = "/2/users/domain_show.json"
                };
            }
        }

        protected override Models.UserInfo ParseUserInfo(string content)
        {
            var cnt = JObject.Parse(content);
            var names = cnt["name"].SafeGet(t => t.Value<string>());
            
            var result = new UserInfo
            {

                ProviderName = this.Name,
                Id = cnt["id"].Value<string>(),
                LastName =  cnt["name"].SafeGet(t => t.Value<string>()),
                AvatarUri =
                {
                    Small =string.Empty,
                    Normal = cnt["profile_image_url"].SafeGet(t => t.Value<string>()),
                    Large = cnt["avatar_large"].SafeGet(t => t.Value<string>())
                }
            };
            return result;
        }
    }
}
