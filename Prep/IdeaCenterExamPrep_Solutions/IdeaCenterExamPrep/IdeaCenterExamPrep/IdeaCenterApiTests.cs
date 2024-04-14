using System;
using System.Net;
using System.Text.Json;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using IdeaCenterExamPrep.Models;

namespace IdeaCenterExamPrep
{
    [TestFixture]
    public class IdeaCenterApiTests
    {
        private RestClient client;
        private static string lastCreatedIdeaId;

        [OneTimeSetUp]
        public void Setup()
        {
            string jwtToken = GetJwtToken("user@example.com", "string");

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
        public void CreateIdea_WithRequiredFields_ShouldSucceed()
        {
            var ideaRequest = new IdeaDto
            {
                Title = "New Idea",
                Url = "",
                Description = "A detailed description of the idea."
                
            };

            var request = new RestRequest("/api/Idea/Create", Method.Post);
            request.AddJsonBody(ideaRequest);
            var response = this.client.Execute(request);
            var createResponse = JsonSerializer.Deserialize<ApiResponseDto>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(createResponse.Msg, Is.EqualTo("Successfully created!"));

        }

        [Order(2)]
        [Test]
        public void GetAllIdeas_ShouldReturnNonEmptyArray()
        {
            var request = new RestRequest("/api/Idea/All", Method.Get);
            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseItems = JsonSerializer.Deserialize<List<ApiResponseDto>>(response.Content);
            Assert.IsNotNull(responseItems);
            Assert.IsNotEmpty(responseItems);

            // Extract the ID of the last created idea, the list is ordered by creation date
            lastCreatedIdeaId = responseItems.LastOrDefault()?.IdeaId;
            
        }

        [Order(3)]
        [Test]
        public void EditExistingIdea_ShouldSucceed()
        {
            //Assume.That(!string.IsNullOrEmpty(lastCreatedIdeaId), "CreateIdea test must run first and succeed.");

            var editRequest = new IdeaDto
            {
                Title = "Edited Idea",
                Url = "",
                Description = "Updated description."
                
            };

            var request = new RestRequest($"/api/Idea/Edit", Method.Put);
            request.AddQueryParameter("ideaId", lastCreatedIdeaId);
            request.AddJsonBody(editRequest);
            var response = this.client.Execute(request);
            var editResponse = JsonSerializer.Deserialize<ApiResponseDto>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(editResponse.Msg, Is.EqualTo("Edited successfully"));
        }

        [Order(4)]
        [Test]
        public void DeleteExistingIdea_ShouldSucceed()
        {
            var request = new RestRequest($"/api/Idea/Delete", Method.Delete);
            request.AddQueryParameter("ideaId", lastCreatedIdeaId);
            var response = this.client.Execute(request);
            
            // Verify the response
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            string expectedResponse = "The idea is deleted!"; // Response is string, not JSON object
            Assert.That(response.Content, Does.Contain(expectedResponse));

        }

        [Order(5)]
        [Test]
        public void CreateIdea_WithoutRequiredFields_ShouldReturnBadRequest()
        {
            // Try to create an idea request without title and description
            var ideaRequest = new IdeaDto
            {
                Title = "",
                Description = "",
            };

            var request = new RestRequest("/api/Idea/Create", Method.Post);
            request.AddJsonBody(ideaRequest);
            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        }

        [Order(6)]
        [Test]
        public void EditNonExistingIdea_ShouldReturnNotFound()
        {
            // Using an idea ID that is known to not exist.
            string nonExistingIdeaId = "123";
            var ideaRequest = new IdeaDto
            {
                Title = "Updated Title",
                Description = "Updated Description",
            };

            var request = new RestRequest($"/api/Idea/Edit", Method.Put);
            request.AddQueryParameter("ideaId", nonExistingIdeaId);
            request.AddJsonBody(ideaRequest);
            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            string expectedResponse = "There is no such idea!"; // Response is string, not JSON object
            Assert.That(response.Content, Does.Contain(expectedResponse));
        }

        [Order(7)]
        [Test]
        public void DeleteNonExistingIdea_ShouldReturnNotFound()
        {
            // Using a idea ID that is known to not exist.
            string nonExistingIdeaId = "123";
           
            var request = new RestRequest($"/api/Idea/Delete", Method.Delete);
            request.AddQueryParameter("ideaId", nonExistingIdeaId);

            var response = this.client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            string expectedResponse = "There is no such idea!"; // Response is string, not JSON object
            Assert.That(response.Content, Does.Contain(expectedResponse));
        }

    }
}