using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Counter
{
    public interface ICounter
    {
        Task CountJob(Guid jobId);
    }
}
