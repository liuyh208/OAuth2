using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using OAuth2.Client;
using OAuth2.Client.Impl;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;

namespace OAuth2.Tests.Client.Impl
{
    [TestFixture]
    public class QQClientTests
    {
        private const string content = "{\"email\":\"email\",\"given_name\":\"name\",\"family_name\":\"surname\",\"id\":\"id\"}";
        private const string contentWithPicture = "{\"email\":\"email\",\"given_name\":\"name\",\"family_name\":\"surname\",\"id\":\"id\",\"picture\":\"picture\"}";

        private QQClientDescendant descendant;

        [SetUp]
        public void SetUp()
        {
            descendant = new QQClientDescendant(Substitute.For<IRequestFactory>(), Substitute.For<IClientConfiguration>());
        }

        [Test]
        public void Should_ReturnCorrectAccessCodeServiceEndpoint()
        {
            // act
            var endpoint = descendant.GetAccessCodeServiceEndpoint();

            // assert
            endpoint.BaseUri.Should().Be("https://graph.qq.com");
            endpoint.Resource.Should().Be("/oauth2.0/authorize");
        }

        [Test]
        public void Should_ReturnCorrectAccessTokenServiceEndpoint()
        {
            // act
            var endpoint = descendant.GetAccessTokenServiceEndpoint();

            // assert
            endpoint.BaseUri.Should().Be("https://graph.qq.com");
            endpoint.Resource.Should().Be("/oauth2.0/token");
        }

        [Test]
        public void Should_ReturnCorrectUserInfoServiceEndpoint()
        {
            // act
            var endpoint = descendant.GetUserInfoServiceEndpoint();

            // assert
            endpoint.BaseUri.Should().Be("https://graph.qq.com");
            endpoint.Resource.Should().Be("/oauth2.0/me");
        }

        [Test]
        public void ShouldNot_Throw_WhenParsingUserInfoAndPictureIsNotAvailable()
        {
            // act & assert
            descendant.Invoking(x => x.ParseUserInfo(content)).ShouldNotThrow();
        }

        [Test]
        public void Should_ParseAllFieldsOfUserInfo_WhenCorrectContentIsPassed()
        {
            // act
            var info = descendant.ParseUserInfo(contentWithPicture);

            //  assert
            info.Id.Should().Be("id");
            info.FirstName.Should().Be("name");
            info.LastName.Should().Be("surname");
            info.Email.Should().Be("email");
            info.PhotoUri.Should().Be("picture");
        }

        class QQClientDescendant : QQClient
        {
            public QQClientDescendant(IRequestFactory factory, IClientConfiguration configuration)
                : base(factory, configuration)
            {
            }

            public Endpoint GetAccessCodeServiceEndpoint()
            {
                return AccessCodeServiceEndpoint;
            }

            public Endpoint GetAccessTokenServiceEndpoint()
            {
                return AccessTokenServiceEndpoint;
            }

            public Endpoint GetUserInfoServiceEndpoint()
            {
                return UserInfoServiceEndpoint;
            }

            public new UserInfo ParseUserInfo(string content)
            {
                return base.ParseUserInfo(content);
            }
        } 
    }
}