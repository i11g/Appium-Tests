using IdeaAPITesting.Models;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;
using System.Text.Json;

namespace IdeaAPITesting
{ 
    
    [TestFixture]

  public class IdeaAPITests
  {
        private RestClient client;
        private static string lastCreatedIdeaId;

        [OneTimeSetUp]
        public void Setup()
        {
            string jwtToken = GetJwtToken("iv@test.com", "123456");

            var options = new RestClientOptions("http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:84")
            {
                Authenticator = new JwtAuthenticator(jwtToken)
            };

            this.client = new RestClient(options);
        }
        private string GetJwtToken(string email, string password)
        {
            var tempClient = new RestClient("http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:84");
            var request = new RestRequest("/api/User/Authentication", Method.Post);
            request.AddJsonBody(new
            {
                email,
                password
            });

            var response = tempClient.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = JsonSerializer.Deserialize<JsonElement>(response.Content);
                var token = content.GetProperty("accessToken").GetString();
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new InvalidOperationException("The JWT token is null or empty.");
                }
                return token;
            }
            else
            {
                throw new InvalidOperationException($"Authentication failed: {response.StatusCode}, {response.Content}");
            }
        }

        [Order(1)]
        [Test]
        public void Create_A_New_Idea()
        {
            var idea = new IdeaDTO
            {
                Type = "New idea",
                Description = "New idea description",
                Url = ""
            };

            var request = new RestRequest("/api/Idea/all", Method.Post);
            request.AddJsonBody(idea);
            var response = client.Execute(request);
            var content=JsonSerializer.Deserialize<JsonElement>(response.Content);
        }
  }
}