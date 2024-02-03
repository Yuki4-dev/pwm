
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace pwm.Service
{
    public interface IFileAccessService
    {
        Task<IStorageFile?> OpenAsync(IEnumerable<string> filters);

        Task<string> ReadAsync(IStorageFile file);

        Task<IStorageFile?> SaveAsync(string fileName, IDictionary<string, IList<string>> choices);

        Task WriteAsync(IStorageFile file, string data);
    }
}
