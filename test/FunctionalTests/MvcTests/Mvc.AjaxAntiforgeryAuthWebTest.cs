// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using System.Threading.Tasks;
using EntropyTests;
using Microsoft.AspNetCore.Server.IntegrationTesting;
using Xunit;

namespace FunctionalTests.MvcTests
{
    public class MvcAjaxAntiforgeryAuthWebTest
    {
        private const string SiteName = "Mvc.AjaxAntiforgeryAuth.Web";

        [Theory]
        [InlineData(ServerType.Kestrel, RuntimeFlavor.CoreClr, RuntimeArchitecture.x64, "http://localhost:6208")]
        public async Task RunAntiforgery_AllPlatforms(ServerType server, RuntimeFlavor runtimeFlavor, RuntimeArchitecture architecture, string applicationBaseUrl)
        {
            await RunAntiforgerySite(server, runtimeFlavor, architecture, applicationBaseUrl);
        }

        private async Task RunAntiforgerySite(
            ServerType server,
            RuntimeFlavor runtimeFlavor,
            RuntimeArchitecture architecture,
            string applicationBaseUrl)
        {
            await TestServices.RunSiteTest(
                SiteName,
                server,
                runtimeFlavor,
                architecture,
                applicationBaseUrl,
                async (httpClient, logger, token) =>
                {
                    var response = await RetryHelper.RetryRequest(async () =>
                    {
                        return await httpClient.GetAsync("/Home/Antiforgery");
                    }, logger, token, retryCount: 30);

                    // Authentication should run before Antiforgery
                    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                });
        }
    }
}
