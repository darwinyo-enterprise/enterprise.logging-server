using Enterprise.Abstract.NetStandard;
using Enterprise.Comparer.NetStandard;
using Enterprise.Constants.NetStandard.LoggingServer;
using Enterprise.Extension.NetStandard;
using Enterprise.Helpers.NetStandard;
using Enterprise.LoggingServer.DataLayers.Mongo;
using Enterprise.LoggingServer.MockData;
using Enterprise.Models.NetStandard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace Enterprise.LoggingServer.Tests
{
    public class ErrorLogControllerTest : BaseTest
    {
        public ErrorLogControllerTest()
        {
            StartUp();
        }
        #region Fields
        private LogModelMockData logModelMockData;
        #endregion

        #region Overrides
        public override void InitMockData()
        {
            logModelMockData = new LogModelMockData();
        }
        public override void CleanUpLogs()
        {
            var httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.DeleteLoggingAllAsync(ControllerUrls.ErrorLog_URL).Wait();
        }
        public override void CleanUpMockData()
        {
            logModelMockData = null;
        }
        #endregion

        [Fact]
        public void Post_WhenPassRightModel_LogToMongoDB()
        {
            // Pre Test
            var httpClient = HttpClientHelper.CreateHttpClient();
            var datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetListLoggingByDateAsync(DateTime.Today, ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.Empty(datas);

            // Action
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();

            // Assert
            httpClient = HttpClientHelper.CreateHttpClient();
            datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetListLoggingByDateAsync(DateTime.Today, ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.NotEmpty(datas);
        }
        [Fact]
        public void Get_WhenIDPass_ReturnSingleLog()
        {
            // Pre Test
            var httpClient = HttpClientHelper.CreateHttpClient();
            var datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetListLoggingByDateAsync(DateTime.Today, ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.Empty(datas);

            // Action
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();

            // Assert Item Exists
            httpClient = HttpClientHelper.CreateHttpClient();
            datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetListLoggingByDateAsync(DateTime.Today, ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.NotEmpty(datas);

            // Assert
            httpClient = HttpClientHelper.CreateHttpClient();
            var x = httpClient.GetLoggingByIdAsync(datas[0].LogID.ToString(), ControllerUrls.ErrorLog_URL).Result;
            var response = JsonConvert.DeserializeObject<ErrorLog>(x.Content.ReadAsStringAsync().Result);
            Assert.True(LogComparer<ErrorLog>.Compare(datas[0], response));
        }
        [Fact]
        public void Get_WhenDatePass_ReturnEnumerablesOfLogs()
        {
            // Pre Test
            var httpClient = HttpClientHelper.CreateHttpClient();
            var datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetListLoggingByDateAsync(DateTime.Today, ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.Empty(datas);

            // Action
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();

            // Assert Item Exists
            httpClient = HttpClientHelper.CreateHttpClient();
            datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetListLoggingByDateAsync(DateTime.Today, ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.NotEmpty(datas);

            // Assert Data Count
            Assert.Equal(3, datas.Count);
        }
        [Fact]
        public void Get_WhenNoParamaterPass_ReturnAllOfLogs()
        {
            // Pre Test
            var httpClient = HttpClientHelper.CreateHttpClient();
            var datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetAllLoggingAsync(ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.Empty(datas);

            // Action
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();

            // Assert Item Exists
            httpClient = HttpClientHelper.CreateHttpClient();
            datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetAllLoggingAsync(ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.NotEmpty(datas);

            // Assert Data Count
            Assert.Equal(3, datas.Count);
        }
        [Fact]
        public void Delete_WhenIDParamaterPass_DeleteSingleLogs()
        {
            // Pre Test Init Data
            var httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();

            httpClient = HttpClientHelper.CreateHttpClient();
            var datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetAllLoggingAsync(ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.NotEmpty(datas);

            // Assert Data Count
            Assert.Equal(3, datas.Count);

            var searchedID = datas[0].LogID;
            // Action
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.DeleteLoggingByIDAsync(searchedID, ControllerUrls.ErrorLog_URL).Wait();

            // Assert Item Exists
            httpClient = HttpClientHelper.CreateHttpClient();
            datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetAllLoggingAsync(ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.NotEmpty(datas);

            // Assert Data Count
            Assert.Equal(2, datas.Count);

            // Assert Data Deleted
            httpClient = HttpClientHelper.CreateHttpClient();
            var data = JsonConvert.DeserializeObject<ErrorLog>(httpClient.GetLoggingByIdAsync(searchedID, ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.Null(data);
        }
        [Fact]
        public void Delete_WhenIDParamaterPassNotExists_DeleteSingleLogs()
        {
            // Pre Test
            // Verify Data Not Exists
            var httpClient = HttpClientHelper.CreateHttpClient();
            var datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetAllLoggingAsync(ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.Empty(datas);

            var searchedID = Guid.NewGuid().ToString();
            // Action
            httpClient = HttpClientHelper.CreateHttpClient();
            string expectedMessage = JsonConvert.SerializeObject(new { error = Constants.NetStandard.ExceptionMessage.ITEM_NOT_FOUND });
            var code = HttpStatusCode.NotFound;
            var response = httpClient.DeleteLoggingByIDAsync(searchedID, ControllerUrls.ErrorLog_URL).Result;

            Assert.Equal(expectedMessage, response.Content.ReadAsStringAsync().Result);
            Assert.Equal(code, response.StatusCode);
        }
        [Fact]
        public void Delete_WhenNoParamaterPass_DeleteAllOfLogs()
        {
            // Pre Test Init Data
            var httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.PostLoggingAsync(logModelMockData.GetErrorLogMockData(), false).Wait();

            httpClient = HttpClientHelper.CreateHttpClient();
            var datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetAllLoggingAsync(ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.NotEmpty(datas);

            // Assert Data Count
            Assert.Equal(3, datas.Count);

            // Action
            httpClient = HttpClientHelper.CreateHttpClient();
            httpClient.DeleteLoggingAllAsync(ControllerUrls.ErrorLog_URL).Wait();

            // Assert Item Exists
            httpClient = HttpClientHelper.CreateHttpClient();
            datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetAllLoggingAsync(ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.Empty(datas);
        }
        [Fact]
        public void Delete_WhenItemNotExists_ThrowItemNotFoundException()
        {
            // Pre Test
            // Verify Data Not Exists
            var httpClient = HttpClientHelper.CreateHttpClient();
            var datas = JsonConvert.DeserializeObject<List<ErrorLog>>(httpClient.GetAllLoggingAsync(ControllerUrls.ErrorLog_URL).Result.Content.ReadAsStringAsync().Result);
            Assert.Empty(datas);

            // Assert Action Throw Exception
            // Action
            httpClient = HttpClientHelper.CreateHttpClient();
            string expectedMessage = JsonConvert.SerializeObject(new { error = Constants.NetStandard.ExceptionMessage.ITEM_NOT_FOUND });
            var code = HttpStatusCode.NotFound;
            var response = httpClient.DeleteLoggingAllAsync(ControllerUrls.ErrorLog_URL).Result;

            Assert.Equal(expectedMessage, response.Content.ReadAsStringAsync().Result);
            Assert.Equal(code, response.StatusCode);
        }
    }
}
