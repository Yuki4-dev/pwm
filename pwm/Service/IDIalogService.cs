#nullable enable

using pwm.Models.DialogService;
using pwm.ViewModels;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace pwm.Service
{
    public interface IDialogService
    {
        bool IsOpenDialog { get; }

        void CloseDialog();

        Task<ContentDialogResult> ShowContentDialogAsync(IViewModel viewModel);

        Task<ContentDialogResult> ShowMessageDialogAsync(ShowMessageDialogParameter message);

        Task<ContentDialogResult> ShowErrorMessageDialogAsync(Exception ex);
    }
}
