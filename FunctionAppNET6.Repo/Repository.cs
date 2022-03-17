using FunctionAppNET6.Core;
using FunctionAppNET6.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppNET6.Repo
{
    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public GetJobDetailsResponse GetJobDetails(int jobId)
        {
            var job =  _context.Job.Where(x => x.Id == jobId).Select(x => new GetJobDetailsResponse()
            {
                JobID = jobId,
                DueDate = x.DueDate
            }).FirstOrDefault();

            if(job != null)
            {
                return job;
            }
            else
            {
                throw new Exception($"Unable to find job details for jobId{jobId}");
            }
        }
    }
}
