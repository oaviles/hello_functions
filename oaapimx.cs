using System.IO;
using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace OA.Function
{
    public class oaapimx
    {
        private readonly ILogger<oaapimx> _logger;

        public oaapimx(ILogger<oaapimx> log)
        {
            _logger = log;
        }

        [FunctionName("oaapimx")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "event" })]
        [OpenApiParameter(name: "event", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **event** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string e = req.Query["event"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            e = e ?? data?.e;
            Guid valueGuid = Guid.NewGuid();

            string responseMessage = string.IsNullOrEmpty(e)
                ? "Must need: Pass an event in the query string or in the request body to get event ID.."
                : $"This is your event ID: {valueGuid}";

            return new OkObjectResult(responseMessage);
        }
    }
}

