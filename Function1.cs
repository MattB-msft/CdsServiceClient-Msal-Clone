using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text.Json;
using System.Xml;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Cds.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Diagnostics;

namespace PortalSynchronization
{
    [ServiceBusAccount("ServiceBusConnectionString")]
    public class Function
    {
        private readonly CdsServiceClient _organizationService;
        private readonly ILogger<Function> _logger;

        public Function(CdsServiceClient organizationService , ILogger<Function> logger)
        {
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        [FunctionName("accounts")]
        public void Accounts([ServiceBusTrigger("foo")] byte[] body,
            string contentType,
            IDictionary<string, object> userProperties,
            string messageId
            )
        {
            

            _organizationService.Dispose();
        }
    }
}