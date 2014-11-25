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
    /// 开心网 kaixin001  OAuth2
    /// </summary>
    public  class Kaixin001Client : OAuth2Client
    {
        public Kaixin001Client(IRequestFactory factory, IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }

        private string _baseUrl = "http://api.kaixin001.com";
        public override string Name
        {
            get { return "kaixin001"; }
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
                return new Endpoint
                {
                    BaseUri = _baseUrl,
                    Resource = "/users/me.json"
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
                Id = cnt["uid"].Value<string>(),
                LastName = cnt["name"].SafeGet(t => t.Value<string>()),
                
                
                AvatarUri =
                {
                    Small = cnt["logo50"].SafeGet(t => t.Value<string>()),
                    Normal = cnt["logo120"].SafeGet(t => t.Value<string>()),
                    Large = cnt["logo120"].SafeGet(t => t.Value<string>())
                }
            };
            return result;
        }
    }
}
