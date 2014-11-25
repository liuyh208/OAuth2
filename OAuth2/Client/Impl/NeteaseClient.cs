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
    /// 网易 163 Netease  OAuth2
    /// </summary>
    public  class NeteaseClient : OAuth2Client
    {
        public NeteaseClient(IRequestFactory factory, IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }

        private string _baseUrl = "http://reg.163.com";
        public override string Name
        {
            get { return "163"; }
        }

        protected override Endpoint AccessCodeServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = _baseUrl,
                    Resource = "/open/oauth2/authorize.do"
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
                    Resource = "/open/oauth2/token.do"
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
                    BaseUri = _baseUrl,
                    Resource = "/open/oauth2/getUserInfo.do"
                };
            }
        }

        protected override Models.UserInfo ParseUserInfo(string content)
        {
            var cnt = JObject.Parse(content);
            //var names = cnt["username"].SafeGet(t => t.Value<string>());
            //var portrait = cnt["avatar"].Value<string>();
            var result = new UserInfo
            {

                ProviderName = this.Name,
                Id = cnt["userId"].Value<string>(),
                LastName = cnt["username"].SafeGet(t => t.Value<string>()),
                
                
                AvatarUri =
                {
                    Small = ""   
                }
            };
            return result;
        }
    }
}
