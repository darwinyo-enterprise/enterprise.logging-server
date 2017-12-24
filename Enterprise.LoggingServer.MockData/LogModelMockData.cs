using Enterprise.Abstract.NetStandard;
using Enterprise.Enums.NetStandard;
using Enterprise.Models.NetStandard;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.LoggingServer.MockData
{
    public class LogModelMockData : BaseMockData<LogModel>
    {
        public LogModel GetAuditLogMockData()
        {
            return new LogModel
            {
                LogType = LogTypeEnum.Info,
                CurrentApplication = ApplicationEnum.Testing,
                LoggerName = "Enterprise.LoggingServer.MockData",
                LogMessage = "Additional Message...",
                UserID = Guid.NewGuid().ToString(),
                UserLogin = "TestUserLogin"
            };
        }
        public LogModel GetDebugLogMockData()
        {
            return new LogModel
            {
                LogType = LogTypeEnum.Debug,
                CurrentApplication = ApplicationEnum.Testing,
                LoggerName = "Enterprise.LoggingServer.MockData",
                LogMessage = "Additional Message...",
                UserID = Guid.NewGuid().ToString(),
                UserLogin = "TestUserLogin"
            };
        }
        public LogModel GetErrorLogMockData()
        {
            return new LogModel
            {
                LogType = LogTypeEnum.Error,
                CurrentApplication = ApplicationEnum.Testing,
                LoggerName = "Enterprise.LoggingServer.MockData",
                LogMessage = "Additional Message...",
                UserID = Guid.NewGuid().ToString(),
                UserLogin = "TestUserLogin"
            };
        }

        public override LogModel GetNormalCaseMockData()
        {
            throw new NotImplementedException();
        }
    }
}
