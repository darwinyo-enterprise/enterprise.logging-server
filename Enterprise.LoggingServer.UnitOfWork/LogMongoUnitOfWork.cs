using Enterprise.LoggingServer.DataLayers.Mongo;
using Enterprise.LoggingServer.Interfaces.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.LoggingServer.UnitOfWork
{
    public class LogMongoUnitOfWork: ILogMongoUnitOfWork
    {
        private readonly LogMongoContext _logMongoContext;
        private readonly IErrorLogRepository _errorLogRepository;
        private readonly IDebugLogRepository _debugLogRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public LogMongoUnitOfWork(
            LogMongoContext logMongoContext,
            IErrorLogRepository errorLogRepository,
            IDebugLogRepository debugLogRepository,
            IAuditLogRepository auditLogRepository)
        {
            _logMongoContext = logMongoContext;
            _errorLogRepository = errorLogRepository;
            _debugLogRepository = debugLogRepository;
            _auditLogRepository = auditLogRepository;
        }
        public LogMongoContext LogMongoContext => this._logMongoContext;
        public IErrorLogRepository ErrorLogRepository => this._errorLogRepository;
        public IDebugLogRepository DebugLogRepository => this._debugLogRepository;
        public IAuditLogRepository AuditLogRepository => this._auditLogRepository;
    }
}
