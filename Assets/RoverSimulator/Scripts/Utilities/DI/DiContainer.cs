using System;
using System.Collections.Generic;
using System.Reflection;

namespace RoverSimulator.Utilities.DI
{
    public class DiContainer
    {
        private struct CachedConstructor
        {
            public ConstructorInfo Constructor;
            public ParameterInfo[] Parameters;
        }

        private readonly Dictionary<Type, CachedConstructor> _constructorCache =
            new Dictionary<Type, CachedConstructor>();

        private readonly Dictionary<Type, ServiceDescriptor> _serviceDescriptors;

        internal DiContainer(Dictionary<Type, ServiceDescriptor> serviceDescriptors)
        {
            _serviceDescriptors = serviceDescriptors;
        }

        public object GetService(Type serviceType)
        {
            if (!_serviceDescriptors.TryGetValue(serviceType, out var descriptor))
            {
                throw new Exception($"Service of type {serviceType.Name} isn't registered");
            }

            if (descriptor.Implementation != null)
            {
                return descriptor.Implementation;
            }

            Type actualType = descriptor.ImplementationType ?? descriptor.ServiceType;

            if (actualType.IsAbstract || actualType.IsInterface)
            {
                throw new Exception($"Cannot instantiate abstract classes or interfaces");
            }

            CachedConstructor cached = GetCachedConstructor(actualType);
            object[] parameters = new object[cached.Parameters.Length];

            for (int i = 0; i < cached.Parameters.Length; i++)
            {
                parameters[i] = GetService(cached.Parameters[i].ParameterType);
            }

            object implementation = Activator.CreateInstance(actualType, parameters);

            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                descriptor.SetImplementation(implementation);
            }

            return implementation;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        private CachedConstructor GetCachedConstructor(Type type)
        {
            if (!_constructorCache.TryGetValue(type, out CachedConstructor cached))
            {
                ConstructorInfo[] constructors = type.GetConstructors();
                cached = new CachedConstructor
                {
                    Constructor = constructors[0],
                    Parameters = constructors[0].GetParameters()
                };
                _constructorCache[type] = cached;
            }

            return cached;
        }
    }
}