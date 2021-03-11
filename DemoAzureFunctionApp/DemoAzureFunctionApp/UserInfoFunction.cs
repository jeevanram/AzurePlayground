using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DemoAzureFunctionApp
{
    public static class UserInfoFunction
    {
        [FunctionName("UserInfoFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            List<dynamic> users = new List<dynamic>()
            {
                new { UserId=1,FirstName="User1 FN", LastName="User1 LN", Email="User1@email.com", Phone ="+1(123)-456 7890", Address1="User1 Address1",Address2 = "User1 Address2", City="Lewisville", County="Denton", State="TX", Country="USA", PostalCode="75067", Username="user1" },
                new { UserId=2,FirstName="User2 FN", LastName="User2 LN", Email="User2@email.com", Phone ="+1(123)-456 7890", Address1="User2 Address1",Address2 = "User2 Address2", City="Lewisville", County="Denton", State="TX", Country="USA", PostalCode="75067", Username="user2" },
                new { UserId=3,FirstName="User3 FN", LastName="User3 LN", Email="User3@email.com", Phone ="+1(123)-456 7890", Address1="User3 Address1",Address2 = "User3 Address2", City="Lewisville", County="Denton", State="TX", Country="USA", PostalCode="75067", Username="user3" },
                new { UserId=4,FirstName="User4 FN", LastName="User4 LN", Email="User4@email.com", Phone ="+1(123)-456 7890", Address1="User4 Address1",Address2 = "User4 Address2", City="Lewisville", County="Denton", State="TX", Country="USA", PostalCode="75067", Username="user4" },
                new { UserId=5,FirstName="User5 FN", LastName="User5 LN", Email="User5@email.com", Phone ="+1(123)-456 7890", Address1="User5 Address1",Address2 = "User5 Address2", City="Lewisville", County="Denton", State="TX", Country="USA", PostalCode="75067", Username="user5" },
                new { UserId=6,FirstName="User6 FN", LastName="User6 LN", Email="User6@email.com", Phone ="+1(123)-456 7890", Address1="User6 Address1",Address2 = "User6 Address2", City="Lewisville", County="Denton", State="TX", Country="USA", PostalCode="75067", Username="user6" },
                new { UserId=7,FirstName="User7 FN", LastName="User7 LN", Email="User7@email.com", Phone ="+1(123)-456 7890", Address1="User7 Address1",Address2 = "User7 Address2", City="Lewisville", County="Denton", State="TX", Country="USA", PostalCode="75067", Username="user7" }
            };

            // Retrieve the username from the query string
            string username = req.Query["username"];

            //If Username present in the query string, fetch the matching user from the collection based on username
            if (username != null)
            {
                dynamic user = users.Where(usr => usr.Username == username).FirstOrDefault();
                if(user != null)
                {
                    return (ActionResult)new OkObjectResult($"User Details: {user.FirstName}, {user.LastName}, {user.Email}, {user.City}, {user.State}, {user.Country}, {user.Username}");
                }
                else
                {
                    //If Username is not found in the collection return 404 
                    return (ActionResult)new NotFoundObjectResult("User not found !");
                }
            }
            //Return 400 bad request if username is missing in the URL query string
            return (ActionResult)new BadRequestObjectResult("Please provide username in the query string !");
        }
    }
}
