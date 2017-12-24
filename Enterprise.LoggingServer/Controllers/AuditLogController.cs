using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Enterprise.LoggingServer.DataLayers.Mongo;
using Enterprise.LoggingServer.Helpers;
using Enterprise.LoggingServer.Interfaces.Mongo;
using Enterprise.Models.NetStandard;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Enterprise.Exceptions.NetStandard;
using NLog;
using Enterprise.Enums.NetStandard;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Enterprise.LoggingServer.Controllers
{
    [Route("api/[controller]")]
    public class AuditLogController : Controller
    {
        private Logger _logger;
        private readonly ILogMongoUnitOfWork _logMongoUnitOfWork;
        public AuditLogController(ILogMongoUnitOfWork logMongoUnitOfWork)
        {
            _logMongoUnitOfWork = logMongoUnitOfWork;
        }
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<AuditLog> auditLog = null;
            try
            {
                auditLog = await _logMongoUnitOfWork.AuditLogRepository.GetAllAsync(_logMongoUnitOfWork.LogMongoContext.AuditLogCollection);
            }
            catch
            {
                throw;
            }
            return Ok(auditLog);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            AuditLog auditLog = null;
            try
            {
                auditLog = await _logMongoUnitOfWork.AuditLogRepository.GetSingleAsync(_logMongoUnitOfWork.LogMongoContext.AuditLogCollection, x => x.LogID == id);
            }
            catch
            {
                throw;
            }
            return Ok(auditLog);
        }
        // GET api/<controller>/5-2-2017
        [HttpGet("search/{id}")]
        public async Task<IActionResult> GetByDate(string id)
        {
            IEnumerable<AuditLog> auditLog = null;
            try
            {
                auditLog = await _logMongoUnitOfWork.AuditLogRepository.FindByAsync(_logMongoUnitOfWork.LogMongoContext.AuditLogCollection, x => x.Date == id);
            }
            catch
            {
                throw;
            }
            return Ok(auditLog);
        }
        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]LogModel logModel)
        {
            try
            {
                _logger = LogManager.GetLogger(logModel.LoggerName);
                MDLCHelper.SetMDLC(logModel);
                if (logModel.LogType == LogTypeEnum.Info)
                    _logger.Info(logModel.LogException, logModel.LogMessage);
                else if (logModel.LogType == LogTypeEnum.Warn)
                    _logger.Warn(logModel.LogException, logModel.LogMessage);
                else
                    throw new InvalidLogTypeException();

            }
            catch
            {
                throw;
            }
            return Ok();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            DeleteResult deleteResult;
            try
            {
                deleteResult = await _logMongoUnitOfWork.AuditLogRepository.DeleteOneAsync(_logMongoUnitOfWork.LogMongoContext.AuditLogCollection, x => x.LogID == id);
                if (deleteResult.DeletedCount == 0)
                {
                    throw new ItemNotFoundException();
                }
            }
            catch
            {
                throw;
            }
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            DeleteResult deleteResult;
            try
            {
                deleteResult = await _logMongoUnitOfWork.AuditLogRepository.DeleteManyAsync(_logMongoUnitOfWork.LogMongoContext.AuditLogCollection, x => x.LogID != null);
                if (deleteResult.DeletedCount == 0)
                {
                    throw new ItemNotFoundException();
                }
            }
            catch
            {
                throw;
            }
            return Ok();
        }
    }
}
