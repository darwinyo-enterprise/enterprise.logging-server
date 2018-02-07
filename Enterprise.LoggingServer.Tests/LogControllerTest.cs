using Enterprise.Fixtures.NetStandard;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Enterprise.LoggingServer.Tests
{
    [CollectionDefinition("Logging Test Collection")]
    public class LogControllerTest : ICollectionFixture<AuthorizationServiceFixture>, ICollectionFixture<LoggingServiceFixture>
    {
    }
}
