using Enterprise.LoggingServer.DataLayers.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.LoggingServer.Interfaces.Mongo
{
    public interface ILogMongoUnitOfWork
    {
        LogMongoContext LogMongoContext { get; }
        IErrorLogRepository ErrorLogRepository { get; }
        IDebugLogRepository DebugLogRepository { get; }
        IAuditLogRepository AuditLogRepository { get; }
    }
}
