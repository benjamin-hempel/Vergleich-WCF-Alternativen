using System;
using System.Collections.Generic;
using System.Net;
using JKang.IpcServiceFramework;
using Microsoft.Extensions.DependencyInjection;
using IPCChatApplication.Shared;

namespace IPCServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = ConfigureServices(new ServiceCollection());

            Console.WriteLine("Starting server...");

            new IpcServiceHostBuilder(services.BuildServiceProvider())
                .AddNamedPipeEndpoint<IChatService>(name: "pipeEndpoint", pipeName: "chatPipe")
                .AddTcpEndpoint<IChatService>(name: "tcpEndpoint", ipEndpoint: IPAddress.Loopback, port: 8000)
                .Build()
                .Run();
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services.AddIpc(builder =>
            {
                builder
                    .AddNamedPipe(options =>
                    {
                        options.ThreadCount = 2;
                    })
                    .AddService<IChatService, ChatService>();
            });
        }
    }
}
