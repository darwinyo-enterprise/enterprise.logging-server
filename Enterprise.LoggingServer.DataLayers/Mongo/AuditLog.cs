using Enterprise.Interfaces.NetStandard;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.LoggingServer.DataLayers.Mongo
{
    public class AuditLog : ILogModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string LogID { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Exception { get; set; }
        public int ThreadID { get; set; }
        public string ThreadName { get; set; }
        public int ProcessID { get; set; }
        public string ProcessName { get; set; }
        public string UserName { get; set; }
        public string UserLogin { get; set; }
        public string UserID { get; set; }
        public string CurrentApplication { get; set; }
        public object Properties { get; set; }

        //public override bool Equals(object obj)
        //{
        //    var x = obj as AuditLog;
        //    bool result = x.LogID == LogID
        //        && x.Date == Date
        //        && x.Time == Time
        //        && x.Level == Level
        //        && x.Message == Message
        //        && x.Logger == Logger
        //        && x.Exception == Exception
        //        && x.ThreadID == ThreadID
        //        && x.ThreadName == ThreadName
        //        && x.ProcessID == ProcessID
        //        && x.ProcessName == ProcessName
        //        && x.UserName == UserName
        //        && x.UserLogin == UserLogin
        //        && x.UserID == UserID
        //        && x.CurrentApplication == CurrentApplication;
        //    return obj != null && result && obj is AuditLog;
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
    }
}
