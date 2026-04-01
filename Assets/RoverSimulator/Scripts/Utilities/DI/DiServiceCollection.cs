using System;
using System.Collections.Generic;

namespace RoverSimulator.Utilities.DI
{
    public class DiServiceCollection
    {
        private readonly Dictionary<Type, ServiceDescriptor> _serviceDescriptors = new Dictionary<Type, ServiceDescriptor>();

        public void RegisterSingleton<TService>()
        {
            ServiceDescriptor descriptor = new ServiceDescriptor(typeof(TService), ServiceLifetime.Singleton);
            _serviceDescriptors[descriptor.ServiceType] = descriptor;
        }

        public void RegisterSingleton<TService>(TService implementation)
        {
            ServiceDescriptor descriptor = new ServiceDescriptor(implementation, ServiceLifetime.Singleton);
            _serviceDescriptors[descriptor.ServiceType] = descriptor;
        }

        public void RegisterSingleton<TService, TImplementation>() where TImplementation : TService
        {
            ServiceDescriptor descriptor = new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
            _serviceDescriptors[descriptor.ServiceType] = descriptor;
        }

        public void RegisterTransient<TService, TImplementation>() where TImplementation : TService
        {
            ServiceDescriptor descriptor = new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
            _serviceDescriptors[descriptor.ServiceType] = descriptor;
        }

        public void RegisterTransient<TService>()
        {
            ServiceDescriptor descriptor = new ServiceDescriptor(typeof(TService), ServiceLifetime.Transient);
            _serviceDescriptors[descriptor.ServiceType] = descriptor;
        }

        public DiContainer GenerateContainer()
        {
            return new DiContainer(_serviceDescriptors);
        }
    }
}