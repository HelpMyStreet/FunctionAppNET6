using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppNET6.Core.Interfaces
{
    public interface IRepository
    {
        GetJobDetailsResponse GetJobDetails(int jobId);
    }
}
