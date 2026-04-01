using System;

namespace RoverSimulator.Utilities.DI
{
    internal class ServiceDescriptor
    {
        public ServiceLifetime Lifetime { get; set; }
        public Type ServiceType { get; set; }
        public Type ImplementationType { get; }
        public object Implementation { get; private set; }

        public ServiceDescriptor(object implementation, ServiceLifetime lifetime)
        {
            ServiceType = implementation.GetType();
            Implementation = implementation;
            Lifetime = lifetime;
        }

        public ServiceDescriptor(Type serviceType, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            Lifetime = lifetime;
        }

        public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime singleton)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            Lifetime = singleton;
        }

        public void SetImplementation(object implementation)
        {
            Implementation = implementation;
        }
    }
}