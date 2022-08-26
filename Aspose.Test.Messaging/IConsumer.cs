using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Messaging
{
    public interface IConsumer<TMessage> where TMessage : Message 
    {
        Task SubscribeAsync(Func<TMessage, Task> handler);
    }
}
