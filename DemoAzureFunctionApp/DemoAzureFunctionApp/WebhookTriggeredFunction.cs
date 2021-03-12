using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DemoAzureFunctionApp
{
    public static class WebhookTriggeredFunction
    {
        [FunctionName("WebhookTriggeredFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var reader = new StreamReader(req.Body);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            dynamic requestBody = JsonConvert.DeserializeObject(reader.ReadToEnd());
            dynamic pages = requestBody.pages;

            if (pages != null)
            {
                dynamic pageInfo = pages[0];
                return (ActionResult)new OkObjectResult($"Page Details: Title-{pageInfo.title}, Action is-{pageInfo.action}, HtmlURL is-{pageInfo.html_url}");
            }
            return (ActionResult)new BadRequestObjectResult("Invalid request !!!");
        }
    }
}

