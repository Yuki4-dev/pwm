using pwm.Core.API;
using System;
using System.Collections.Generic;

namespace pwm.tests
{
    internal class MockDependencyServiceProvider : IDependencyServiceProvider
    {
        private readonly IDictionary<Type, object> services = new Dictionary<Type, object>();

        public void AddService<T>(T service)
        {
            services[typeof(T)] = service;
        }

        public T GetService<T>()
        {
            return (T)services[typeof(T)];
        }
    }
}
