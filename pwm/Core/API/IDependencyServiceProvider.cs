#nullable enable

namespace pwm.Core.API
{
    public interface IDependencyServiceProvider
    {
        public T GetService<T>();
    }
}
