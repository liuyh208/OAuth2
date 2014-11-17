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
    public class QQClient : OAuth2Client
    {
        public QQClient(IRequestFactory factory, IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }
        public override string Name
        {
            get { return "QQ"; }
        }

        protected override Endpoint AccessCodeServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://graph.qq.com",
                    Resource = "/oauth2.0/authorize"
                };
            }
        }

        protected override Endpoint AccessTokenServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://graph.qq.com",
                    Resource = "/oauth2.0/token"
                };
            }
        }

        protected override Endpoint UserInfoServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = "https://graph.qq.com",
                    Resource = "/oauth2.0/me"
                };
            }
        }

        protected override Models.UserInfo ParseUserInfo(string content)
        {
            var cnt = JObject.Parse(content);
            var names = cnt["name"].Value<string>().Split(' ').ToList();
            const string avatarUriTemplate = "{0}&s={1}";
            var avatarUri = cnt["avatar_url"].Value<string>();
            var result = new UserInfo
            {
                Email = cnt["email"].SafeGet(x => x.Value<string>()),
                ProviderName = this.Name,
                Id = cnt["id"].Value<string>(),
                FirstName = names.Count > 0 ? names.First() : cnt["login"].Value<string>(),
                LastName = names.Count > 1 ? names.Last() : string.Empty,
                AvatarUri =
                {
                    Small = !string.IsNullOrWhiteSpace(avatarUri) ? string.Format(avatarUriTemplate, avatarUri, AvatarInfo.SmallSize) : string.Empty,
                    Normal = avatarUri,
                    Large = !string.IsNullOrWhiteSpace(avatarUri) ? string.Format(avatarUriTemplate, avatarUri, AvatarInfo.LargeSize) : string.Empty
                }
            };
            return result;
        }
    }
}
