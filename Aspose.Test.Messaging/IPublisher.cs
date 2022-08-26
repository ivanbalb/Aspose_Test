using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Messaging
{
    public interface IPublisher<TMessage> where TMessage : Message
    {
        Task PublishAsync(TMessage message);
    }
}
