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
    public  class TaobaoClient : OAuth2Client
    {
        public TaobaoClient(IRequestFactory factory, IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }

        private string _baseUrl = " https://oauth.taobao.com";
        public override string Name
        {
            get { return "Taobao"; }
        }

        protected override Endpoint AccessCodeServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = _baseUrl,
                    Resource = "/authorize"
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
                    Resource = "/token"
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
                    BaseUri = "http://gw.api.taobao.com",
                    Resource = "/router/rest?method=taobao.user.get"
                };
            }
        }

        protected override Models.UserInfo ParseUserInfo(string content)
        {
            var cnt = JObject.Parse(content);
            //var names = cnt["username"].SafeGet(t => t.Value<string>());
            var portrait = cnt["avatar"].Value<string>();
            var result = new UserInfo
            {

                ProviderName = this.Name,
                Id = cnt["uid"].Value<string>(),
                LastName = cnt["nick"].SafeGet(t => t.Value<string>()),
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
