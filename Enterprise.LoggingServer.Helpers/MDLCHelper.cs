using Enterprise.Models.NetStandard;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Enterprise.LoggingServer.Helpers
{
    public class MDLCHelper
    {
        public static void SetMDLC(LogModel logModel)
        {
            try
            {
                MappedDiagnosticsLogicalContext.Set("LogID", Guid.NewGuid());
                MappedDiagnosticsLogicalContext.Set("Date", DateTime.Now.ToString("dd-MM-yyyy"));
                MappedDiagnosticsLogicalContext.Set("Time", DateTime.Now.ToString("HH:mm:ss"));
                MappedDiagnosticsLogicalContext.Set("CurrentApplication", logModel.CurrentApplication.ToString());
                MappedDiagnosticsLogicalContext.Set("UserID", logModel.UserID);
                MappedDiagnosticsLogicalContext.Set("UserLogin", logModel.UserLogin);
            }
            catch
            {
                throw;
            }
        }
    }
}