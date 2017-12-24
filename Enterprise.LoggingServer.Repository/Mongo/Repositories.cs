using Enterprise.Abstract.NetStandard;
using Enterprise.LoggingServer.DataLayers.Mongo;
using Enterprise.LoggingServer.Interfaces.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.LoggingServer.Repository.Mongo
{
    public class ErrorLogRepository : BaseMongoRepository<ErrorLog>, IErrorLogRepository
    {
    }
    public class DebugLogRepository : BaseMongoRepository<DebugLog>, IDebugLogRepository
    {
    }
    public class AuditLogRepository : BaseMongoRepository<AuditLog>, IAuditLogRepository
    {
    }
}
