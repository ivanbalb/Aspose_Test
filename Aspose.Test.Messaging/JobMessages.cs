using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Messaging
{
    public abstract record JobMessage(Guid Id) : Message;

    public record JobCreatedMessage(Guid Id) : JobMessage(Id)
    {
        public override string Channel => "JobCreated";
    }

    public record JobDownloadedMessage(Guid Id) : JobMessage(Id)
    {
        public override string Channel => "JobDownloaded";
    }

    public record JobParsedMessage(Guid Id) : JobMessage(Id)
    {
        public override string Channel => "JobParsed";
    }
}
