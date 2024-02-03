#nullable enable

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace pwm.Service
{
    public class FileAccessService : IFileAccessService
    {
        public async Task<IStorageFile?> OpenAsync(IEnumerable<string> filters)
        {
            var filePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List
            };
            foreach (var f in filters)
            {
                filePicker.FileTypeFilter.Add(f);
            }

            return await filePicker.PickSingleFileAsync();
        }

        public async Task<string> ReadAsync(IStorageFile file)
        {
            return await FileIO.ReadTextAsync(file) ?? string.Empty;
        }

        public async Task<IStorageFile?> SaveAsync(string fileName, IDictionary<string, IList<string>> choices)
        {
            var filePicker = new FileSavePicker()
            {
                SuggestedFileName = fileName,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            foreach (var choice in choices)
            {
                filePicker.FileTypeChoices.Add(choice.Key, choice.Value);
            }

            return await filePicker.PickSaveFileAsync();
        }

        public async Task WriteAsync(IStorageFile file, string data)
        {
            await FileIO.WriteTextAsync(file, data);
        }
    }
}
