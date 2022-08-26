using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspose.Test.Storage.Mongo
{
    public class MongoStorageSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string JobsCollectionName { get; set; }
    }
}
