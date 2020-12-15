using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Cds.Client;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

[assembly: FunctionsStartup(typeof(FunctionApp3.Startup))]

namespace FunctionApp3
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            CdsServiceClient client = null;
            if ( client == null )
            {
                client = GetClient(); // Set up the connection here. 
            }

            builder.Services.AddSingleton<CdsClientWithDate>(sp =>
            {
                return new CdsClientWithDate () { 
                    localClient = client,
                    createDate = DateTime.UtcNow};
            });

            builder.Services.AddScoped<CdsServiceClient>(sp =>
            {
                Stopwatch st = new Stopwatch();
                st.Restart();
                var ms01 = st.ElapsedMilliseconds;
                var logger = sp.GetRequiredService<ILogger<Startup>>();
                var ms02 = st.ElapsedMilliseconds;
                var client = sp.GetRequiredService<CdsClientWithDate>();
                var ms03 = st.ElapsedMilliseconds;
                var clone = client.localClient.Clone();
                var ms04 = st.ElapsedMilliseconds;
                logger.LogInformation($"Cloned client - {ms01} - {ms02} - {ms03} - {ms04}");
                st.Stop(); 
                st = null; 
                return clone;
            });
        }
        CdsServiceClient GetClient(ILogger<Startup> logger = null)
        {
            TraceControlSettings.TraceLevel = System.Diagnostics.SourceLevels.All;
            TraceControlSettings.AddTraceListener(new ConsoleTraceListener());
            logger?.LogInformation("Connecting to CDS");
            var connectionString = Environment.GetEnvironmentVariable("CdsServiceConnectionString"); // picking this up from local settings / Functions configuration 
            CdsServiceClient client = null;
            client = new CdsServiceClient(connectionString);
            if (client.IsReady)
            {
                    logger?.LogInformation("Connected to CDS...");
                    return client;
            }
            else throw client.LastCdsException;
       }
    }

    internal class CdsClientWithDate
    {
        public CdsServiceClient localClient { get; set; }
        public DateTime createDate { get; set; }
        
    }
    
}