using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enterprise.ActionResults.NetStandard;
using Enterprise.Enums.NetStandard;
using Enterprise.Exceptions.NetStandard;
using Enterprise.LoggingServer.DataLayers.Mongo;
using Enterprise.LoggingServer.Helpers;
using Enterprise.LoggingServer.Interfaces.Mongo;
using Enterprise.Models.NetStandard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NLog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Enterprise.LoggingServer.Controllers
{
    [Route("api/[controller]")]
    public class ErrorLogController : Controller
    {
        private ILogger _logger;
        private readonly ILogMongoUnitOfWork _logMongoUnitOfWork;
        public ErrorLogController(ILogMongoUnitOfWork logMongoUnitOfWork)
        {
            _logMongoUnitOfWork = logMongoUnitOfWork;

            // If Error Occured Within Logging Service this will be used, Otherwise This will replaced by logmodel
            _logger = LogManager.GetLogger(nameof(ErrorLogController));
        }
        // GET: api/<controller>
        [HttpGet]
        [Authorize(Policy = "read_only_access_policy")]
        public async Task<IActionResult> Get()
        {
            IEnumerable<ErrorLog> errorLog = null;
            try
            {
                errorLog = await _logMongoUnitOfWork.ErrorLogRepository.GetAllAsync(_logMongoUnitOfWork.LogMongoContext.ErrorLogCollection);
            }
            catch (Exception ex)
            {
                return new LogServiceExceptionResult(ex, _logger);
            }
            return Ok(errorLog);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = "read_only_access_policy")]
        public async Task<IActionResult> Get(string id)
        {
            ErrorLog errorLog = null;
            try
            {
                errorLog = await _logMongoUnitOfWork.ErrorLogRepository.GetSingleAsync(_logMongoUnitOfWork.LogMongoContext.ErrorLogCollection, x => x.LogID == id);
            }
            catch (Exception ex)
            {
                return new LogServiceExceptionResult(ex, _logger);
            }
            return Ok(errorLog);
        }

        // GET api/<controller>/5-2-2017
        [HttpGet("search/{id}")]
        [Authorize(Policy = "read_only_access_policy")]
        public async Task<IActionResult> GetByDate(string id)
        {
            IEnumerable<ErrorLog> errorLog = null;
            try
            {
                errorLog = await _logMongoUnitOfWork.ErrorLogRepository.FindByAsync(_logMongoUnitOfWork.LogMongoContext.ErrorLogCollection, x => x.Date == id);
            }
            catch (Exception ex)
            {
                return new LogServiceExceptionResult(ex, _logger);
            }
            return Ok(errorLog);
        }
        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]LogModel logModel)
        {
            try
            {
                _logger = LogManager.GetLogger(logModel.LoggerName);
                MDLCHelper.SetMDLC(logModel);
                if (logModel.LogType == LogTypeEnum.Error)
                    _logger.Error(logModel.LogException, logModel.LogMessage);
                else if (logModel.LogType == LogTypeEnum.Fatal)
                    _logger.Fatal(logModel.LogException, logModel.LogMessage);

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
                deleteResult = await _logMongoUnitOfWork.ErrorLogRepository.DeleteOneAsync(_logMongoUnitOfWork.LogMongoContext.ErrorLogCollection, x => x.LogID == id);
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
            DeleteResult deleteResult;
            try
            {
                deleteResult = await _logMongoUnitOfWork.ErrorLogRepository.DeleteManyAsync(_logMongoUnitOfWork.LogMongoContext.ErrorLogCollection, x => x.Id != null);
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
