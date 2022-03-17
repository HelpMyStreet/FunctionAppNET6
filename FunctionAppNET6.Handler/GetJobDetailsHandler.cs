using FunctionAppNET6.Core;
using FunctionAppNET6.Core.Interfaces;
using MediatR;

namespace FunctionAppNET6.Handler
{
    public class GetJobDetailsHandler : IRequestHandler<GetJobDetailsRequest, GetJobDetailsResponse>
    {
        private readonly IRepository _repository;

        public GetJobDetailsHandler(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<GetJobDetailsResponse> Handle(GetJobDetailsRequest request, CancellationToken cancellationToken)
        {
            var jobDetails = _repository.GetJobDetails(request.JobID);
            return jobDetails;
        }
    }
}