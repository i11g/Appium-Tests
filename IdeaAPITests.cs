using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;
using IdeaAPI.Tests.Models;

namespace IdeaAPI.Tests
{
    [TestFixture]
    public class Api
    {
        private RestClient _client;
        //private const string BASEURL = "http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:84";
        //private const string EMAIL = "iv@test.com";
        //private const string PASSWORD = "123456";

        [OneTimeSetUp]
        public void Setup()
        {
            string jwtToken = GetJwtToken("iv@test.com", "123456");

            
            var options = new RestClientOptions("http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:84")

            {
                Authenticator = new JwtAuthenticator(jwtToken)
            };

            this._client = new RestClient(options);
        }
    
        private string GetJwtToken(string email, string password)
        {
           
        var authClient = new RestClient("http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:84");

        var request = new RestRequest("/api/User/Authentication");
            request.AddJsonBody( new
            {
                email,
                password
            });

            var response = authClient.Execute(request, Method.Post);

            if (response.StatusCode== HttpStatusCode.OK)
            {
                var content = JsonSerializer.Deserialize<JsonElement>(response.Content);
                var token=content.GetProperty("accessToken").GetString();
                if(string.IsNullOrWhiteSpace(token))
                {
                    throw new InvalidOperationException("Access Token is null or White Space");
                }
                return token;
            }
            else
            {
                throw new InvalidOperationException($"Unexpected response type {response.StatusCode} with resposnse data {response.Content}");
            }           
           
        }

        [Order (1)]

        [Test]
        public void CreateIdea_WithRequiredFields_ShouldSucceed()
        {
            var ideaRequest = new IdeaDTO
            {
                Title = "New Idea",
                Url = "",
                Description = "A detailed description of the idea."

            };

            var request = new RestRequest("/api/Idea/Create", Method.Post);
            request.AddJsonBody(ideaRequest);
            var response = this._client.Execute(request);
            var createResponse = JsonSerializer.Deserialize<ApiResponseDto>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(createResponse.Msg, Is.EqualTo("Successfully created!"));

        }
    }
}