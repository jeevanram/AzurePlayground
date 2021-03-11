using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using System.Collections.Generic;
using DemoAzureFunctionApp;
using Microsoft.Extensions.Logging;

namespace DemoAzureFunctionAppTest
{
    public class UserInfoFunctionTests
    {
        [Fact]
        public void TestUserInfoFunctionSuccessUserFound()
        {
            //Set up
            string usernameQueryStringValue = "user1";
            HttpRequest request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection
                (
                    new Dictionary<string, StringValues>()
                    {
                        { "username", usernameQueryStringValue }
                    }
                )
            };


            //Execution
            ILogger logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            var response = UserInfoFunction.Run(request, logger);
            response.Wait();

            //Assertions
            // 1. 200 OK Assertion
            Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            // 2. Check expected user record fetched from the collection
            var actualResult = (OkObjectResult)response.Result;
            var expectedDataSetFromCollection = new { UserId = 1, FirstName = "User1 FN", LastName = "User1 LN", Email = "User1@email.com", Phone = "+1(123)-456 7890", Address1 = "User1 Address1", Address2 = "User1 Address2", City = "Lewisville", County = "Denton", State = "TX", Country = "USA", PostalCode = "75067", Username = "user1" };
            var expectedResult = $"User Details: {expectedDataSetFromCollection.FirstName}, {expectedDataSetFromCollection.LastName}, {expectedDataSetFromCollection.Email}, {expectedDataSetFromCollection.City}, {expectedDataSetFromCollection.State}, {expectedDataSetFromCollection.Country}, {expectedDataSetFromCollection.Username}";
            Assert.Equal(expectedResult, actualResult.Value);
        }

        [Fact]
        public void TestUserInfoFunctionSuccessUserNotFound()
        {
            //Set up
            string usernameQueryStringValue = "userx";
            HttpRequest request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection
                (
                    new Dictionary<string, StringValues>()
                    {
                        { "username", usernameQueryStringValue }
                    }
                )
            };


            //Execution
            ILogger logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            var response = UserInfoFunction.Run(request, logger);
            response.Wait();

            //Assertions
            // 1. 404 NotFound Assertion
            Assert.IsAssignableFrom<NotFoundObjectResult>(response.Result);
            // 2. Check User not found message assertion
            var actualResult = (NotFoundObjectResult)response.Result;
            var expectedResult = $"User not found !";
            Assert.Equal(expectedResult, actualResult.Value);
        }

        [Fact]
        public void TestUserInfoFunctionFailureMissingQueryString()
        {
            //Set up
            HttpRequest request = new DefaultHttpRequest(new DefaultHttpContext());


            //Execution
            ILogger logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            var response = UserInfoFunction.Run(request, logger);
            response.Wait();

            //Assertions
            // 1. 400 BadRequest Assertion
            Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);
            // 2. BadRequest message assertion
            var actualResult = (BadRequestObjectResult)response.Result;
            var expectedResult = $"Please provide username in the query string !";
            Assert.Equal(expectedResult, actualResult.Value);
        }

        [Fact]
        public void TestUserInfoFunctionFailureInvalidQueryString()
        {
            //Set up
            string usernameQueryStringValue = "userx";
            HttpRequest request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection
                (
                    new Dictionary<string, StringValues>()
                    {
                        { "invalidusername", usernameQueryStringValue }
                    }
                )
            };


            //Execution
            ILogger logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            var response = UserInfoFunction.Run(request, logger);
            response.Wait();

            //Assertions
            // 1. 400 BadRequest Assertion
            Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);
            // 2. BadRequest message assertion
            var actualResult = (BadRequestObjectResult)response.Result;
            var expectedResult = $"Please provide username in the query string !";
            Assert.Equal(expectedResult, actualResult.Value);
        }
    }
}
