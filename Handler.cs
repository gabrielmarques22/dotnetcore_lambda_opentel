using System;
using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Amazon.Lambda.CloudWatchEvents.ScheduledEvents;
using Amazon.Lambda.Core;


namespace LabDotNetOpenTel{
    
    
    public static class Handler
    {
        private static readonly ActivitySource MyActivitySource = new ActivitySource("DynaLabs.DotNetCore.Lambda");
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public static string Handle(ScheduledEvent cloudWatchEvent)
        {           
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .AddSource("DynaLabs.DotNetCore.Lambda")
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("BTG Lambda"))
                .AddAWSInstrumentation() //Add auto-instrumentation for AWS SDK
                .AddOtlpExporter()
                .Build();

            using (var activity = MyActivitySource.StartActivity("Invoke",ActivityKind.Server, cloudWatchEvent.Id))
            {
                activity?.SetTag("foo", 1);
                activity?.SetTag("CloudWatchEvent", cloudWatchEvent.Source);
                activity?.SetTag("baz", new int[] { 1, 2, 3 });
                activity?.SetStatus(ActivityStatusCode.Ok);
                Console.WriteLine("Rodando de dentro da activitiy com return dentro da activity: " + activity?.TraceId); 
                Console.WriteLine("Message: " + cloudWatchEvent.Source);    
                return "Message: " + cloudWatchEvent.Source;        
            }             
            
        }
    }
}