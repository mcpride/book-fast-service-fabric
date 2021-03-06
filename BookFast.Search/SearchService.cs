﻿using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using BookFast.ServiceFabric;

namespace BookFast.Search
{
    internal sealed class SearchService : StatelessService
    {
        public SearchService(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners() =>
            new ServiceInstanceListener[]
            {
                ServiceInstanceListenerFactory.CreateInternalListener(typeof(Startup), (serviceContext, message) => ServiceEventSource.Current.ServiceMessage(serviceContext, message))
            };
    }
}