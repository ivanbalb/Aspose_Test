using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Html
{
    public interface ITextExtractor
    {
        Task ExtractText(Guid jobId);
    }
}
