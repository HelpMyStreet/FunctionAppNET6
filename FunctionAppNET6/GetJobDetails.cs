using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MediatR;
using FunctionAppNET6.Core;
using System.Threading;

namespace FunctionAppNET6
{
    public class GetJobDetails
    {
        private readonly IMediator _mediator;

        public GetJobDetails(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("GetJobDetails")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetJobDetailsResponse))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] GetJobDetailsRequest req,
            ILogger log,
            CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation("GetJobDetails started");
                GetJobDetailsResponse response = await _mediator.Send(req, cancellationToken);
                log.LogInformation("GetJobDetails completed");
                return new OkObjectResult(response);
            }
            catch (Exception exc)
            {
                log.LogError("Exception occured in GetJobDetails", exc.Message);
                return new BadRequestObjectResult(log);
            }            
        }
    }
}
