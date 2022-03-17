using MediatR;

namespace FunctionAppNET6.Core
{
    public class GetJobDetailsRequest : IRequest<GetJobDetailsResponse>
    {
        public int JobID { get; set; }
    }
}