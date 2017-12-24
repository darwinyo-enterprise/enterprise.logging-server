using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enterprise.Enums.NetStandard;
using Enterprise.Exceptions.NetStandard;
using Enterprise.LoggingServer.DataLayers.Mongo;
using Enterprise.LoggingServer.Helpers;
using Enterprise.LoggingServer.Interfaces.Mongo;
using Enterprise.Models.NetStandard;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NLog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Enterprise.LoggingServer.Controllers
{
    [Route("api/[controller]")]
    public class ErrorLogController : Controller
    {
        private Logger _logger;
        private readonly ILogMongoUnitOfWork _logMongoUnitOfWork;
        public ErrorLogController(ILogMongoUnitOfWork logMongoUnitOfWork)
        {
            _logMongoUnitOfWork = logMongoUnitOfWork;
        }
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<ErrorLog> errorLog = null;
            try
            {
                errorLog = await _logMongoUnitOfWork.ErrorLogRepository.GetAllAsync(_logMongoUnitOfWork.LogMongoContext.ErrorLogCollection);
            }
            catch
            {
                throw;
            }
            return Ok(errorLog);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            ErrorLog errorLog = null;
            try
            {
                errorLog = await _logMongoUnitOfWork.ErrorLogRepository.GetSingleAsync(_logMongoUnitOfWork.LogMongoContext.ErrorLogCollection, x => x.LogID == id);
            }
            catch
            {
                throw;
            }
            return Ok(errorLog);
        }

        // GET api/<controller>/5-2-2017
        [HttpGet("search/{id}")]
        public async Task<IActionResult> GetByDate(string id)
        {
            IEnumerable<ErrorLog> errorLog = null;
            try
            {
                errorLog = await _logMongoUnitOfWork.ErrorLogRepository.FindByAsync(_logMongoUnitOfWork.LogMongoContext.ErrorLogCollection, x => x.Date == id);
            }
            catch
            {
                throw;
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
                deleteResult = await _logMongoUnitOfWork.ErrorLogRepository.DeleteOneAsync(_logMongoUnitOfWork.LogMongoContext.ErrorLogCollection, x => x.LogID == id);
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
                deleteResult = await _logMongoUnitOfWork.ErrorLogRepository.DeleteManyAsync(_logMongoUnitOfWork.LogMongoContext.ErrorLogCollection, x => x.LogID != null);
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
