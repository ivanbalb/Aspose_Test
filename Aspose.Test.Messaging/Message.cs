using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Messaging
{
    public abstract record Message
    {
        public abstract string Channel { get; }
    }
}
