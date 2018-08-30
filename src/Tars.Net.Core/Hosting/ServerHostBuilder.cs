﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tars.Net.Attributes;
using Tars.Net.Hosting;

namespace Tars.Net
{
    public class ServerHostBuilder : IServerHostBuilder
    {
        public IServiceCollection Services { get; }

        public IConfigurationBuilder ConfigurationBuilder { get; }

        public IEnumerable<Type> Clients { get; }

        public IEnumerable<(Type Service, Type Implementation)> RpcServices { get; }

        public ServerHostBuilder()
        {
            Services = new ServiceCollection();
            ConfigurationBuilder = new ConfigurationBuilder();

            var all = RpcHelper.GetAllHasAttributeTypes<RpcAttribute>();
            var (rpcServices, rpcClients) = RpcHelper.GetAllRpcServicesAndClients(all);
            Clients = rpcClients;
            RpcServices = rpcServices;
        }

        public virtual IServerHost Build()
        {
            return Services
                .AddSingleton<IConfiguration>(ConfigurationBuilder.Build())
                .BuildServiceProvider()
                .GetRequiredService<IServerHost>();
        }
    }
}