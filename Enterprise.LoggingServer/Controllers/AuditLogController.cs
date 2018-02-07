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
using Microsoft.AspNetCore.Authorization;
using Enterprise.ActionResults.NetStandard;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Enterprise.LoggingServer.Controllers
{
    [Route("api/[controller]")]
    public class AuditLogController : Controller
    {
        private readonly ILogMongoUnitOfWork _logMongoUnitOfWork;

        private ILogger _logger;

        public AuditLogController(ILogMongoUnitOfWork logMongoUnitOfWork)
        {
            _logMongoUnitOfWork = logMongoUnitOfWork;

            // If Error Occured Within Logging Service this will be used, Otherwise This will replaced by logmodel
            _logger = LogManager.GetLogger(nameof(AuditLogController));
        }
        // GET: api/<controller>
        [HttpGet]
        [Authorize(Policy = "read_only_access_policy")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<AuditLog> auditLog = null;
            try
            {
                auditLog = await _logMongoUnitOfWork.AuditLogRepository.GetAllAsync(_logMongoUnitOfWork.LogMongoContext.AuditLogCollection);
            }
            catch (Exception ex)
            {
                return new LogServiceExceptionResult(ex, _logger);
            }
            return Ok(auditLog);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = "read_only_access_policy")]
        public async Task<IActionResult> Get(string id)
        {
            AuditLog auditLog = null;
            try
            {
                auditLog = await _logMongoUnitOfWork.AuditLogRepository.GetSingleAsync(_logMongoUnitOfWork.LogMongoContext.AuditLogCollection, x => x.LogID == id);
            }
            catch (Exception ex)
            {
                return new LogServiceExceptionResult(ex, _logger);
            }
            return Ok(auditLog);
        }
        // GET api/<controller>/5-2-2017
        [HttpGet("search/{id}")]
        [Authorize(Policy = "read_only_access_policy")]
        public async Task<IActionResult> GetByDate(string id)
        {
            IEnumerable<AuditLog> auditLog = null;
            try
            {
                auditLog = await _logMongoUnitOfWork.AuditLogRepository.FindByAsync(_logMongoUnitOfWork.LogMongoContext.AuditLogCollection, x => x.Date == id);
            }
            catch (Exception ex)
            {
                return new LogServiceExceptionResult(ex, _logger);
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
            catch (Exception ex)
            {
                return new LogServiceExceptionResult(ex, _logger);
            }
            return Ok();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "delete_access_policy")]
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
            catch (Exception ex)
            {
                return new LogServiceExceptionResult(ex, _logger);
            }
            return Ok();
        }
        [HttpDelete]
        [Authorize(Policy = "delete_access_policy")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                DeleteResult deleteResult;
                deleteResult = await _logMongoUnitOfWork.AuditLogRepository.DeleteManyAsync(_logMongoUnitOfWork.LogMongoContext.AuditLogCollection, x => x.LogID != null);
                if (deleteResult.DeletedCount == 0)
                {
                    throw new ItemNotFoundException();
                }
            }
            catch (Exception ex)
            {
                return new LogServiceExceptionResult(ex, _logger);
            }
            return Ok();
        }
    }
}
