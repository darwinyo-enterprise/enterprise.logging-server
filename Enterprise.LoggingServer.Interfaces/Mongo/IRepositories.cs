using Enterprise.Interfaces.NetStandard;
using Enterprise.LoggingServer.DataLayers.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.LoggingServer.Interfaces.Mongo
{
    public interface IErrorLogRepository: IMongoRepository<ErrorLog>
    {
    }
    public interface IDebugLogRepository : IMongoRepository<DebugLog>
    {
    }
    public interface IAuditLogRepository : IMongoRepository<AuditLog>
    {
    }
}
