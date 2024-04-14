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
                Title = "New idea",
                Description = "New idea description",
                Url = ""
            };

            var request = new RestRequest("/api/Idea/Create", Method.Post);
            request.AddJsonBody(idea);
            var response = this.client.Execute(request);
            var content=JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);
            
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content.Msg, Is.EqualTo("Successfully created!"));
        }
        [Order(2)]
        [Test]

        public void Get_All_Ideas ()
        {
            var request = new RestRequest("/api/Idea/All"); 

            var response = this.client.Execute(request);

            var content = JsonSerializer.Deserialize<List<ApiResponseDTO>>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsNotNull(content);
            Assert.IsNotEmpty(content);

            lastCreatedIdeaId = content.LastOrDefault().ideaId;
        }

        [Order(3)]
        [Test]

        public void Edit_Lat_Idea()
        {
            var editedIdea = new IdeaDTO
            {
                Title = "Edited title",
                Description = "Edited description",
                Url = ""
            };

            
            var request = new RestRequest($"/api/Idea/Edit", Method.Put);
            request.AddQueryParameter("ideaId", lastCreatedIdeaId);
            request.AddJsonBody(editedIdea);

            var response = client.Execute(request);

            var content = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content.Msg, Is.EqualTo("Edited successfully"));        


        }
        [Order(4)]
        [Test] 

        public void Delete_Lat_Idea()
        {
            var request = new RestRequest("/api/Idea/Delete", Method.Delete);

            request.AddQueryParameter("ideaId", lastCreatedIdeaId);

            var response=this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            //var content=JsonSerializer.Deserialize<string>(response.Content);

            Assert.That(response.Content, Does.Contain("The idea is deleted!"));
        }

        [Order(5)]
        [Test]

        public void Create_Idea_Without_RequiredFields_ShouldReturn_BadRequest ()
        {
            var newIdea = new IdeaDTO
            {
                Url = ""
            };
            
            var request = new RestRequest("/api/Idea/Create", Method.Post);
            request.AddJsonBody(newIdea);
            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Order(6)]
        [Test]

        public void Edit_NonExistingIdea_ShouldReturn_BadRequest()
        {
            string ideaId = "1999";
            var editedIdea = new IdeaDTO
            {
                Title = "Edited Title",
                Description = "Edited descriptio"
            };
            var request = new RestRequest("/api/Idea/Edit", Method.Put);
            request.AddJsonBody(editedIdea);
            request.AddQueryParameter("ideaId", ideaId);

            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Does.Contain("There is no such idea!"));
        }

        [Order(7)]
        [Test] 

        public void Delete_Non_ExistingIdea_Should_Return_BadRequest ()
        {
            string ideaId = "1000";
            var request = new RestRequest("/api/Idea/Delete", Method.Delete);
            request.AddQueryParameter("ideaId", ideaId);

            var response = client.Execute(request);

            string content = "There is no such idea!";
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Does.Contain(content));
        }
  }
}