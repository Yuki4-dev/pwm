#nullable enable

using pwm.Models.DialogService;
using pwm.UI;
using pwm.ViewModels;
using pwm.Views.UI;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace pwm.Service
{
    public class DialogService : IDialogService
    {
        private ContentDialog? currentDialog;

        private readonly IApplicationThemeListener applicationThemeListener;

        public bool IsOpenDialog => currentDialog != null;

        public DialogService(IApplicationThemeListener applicationThemeListener)
        {
            this.applicationThemeListener = applicationThemeListener;
        }

        public void CloseDialog()
        {
            if (!IsOpenDialog)
            {
                return;
            }

            currentDialog!.Hide();
            if (currentDialog is IViewModelContentDialog viewModelDialog)
            {
                viewModelDialog.ForceClosed();
            }
        }

        public async Task<ContentDialogResult> ShowMessageDialogAsync(ShowMessageDialogParameter message)
        {
            var dialog = new ContentDialog
            {
                Title = message.Title,
                CornerRadius = new CornerRadius(8),
                RequestedTheme = applicationThemeListener.IsDark ? ElementTheme.Dark : ElementTheme.Light,
                Content = GetMessageContent(message.Message),
                PrimaryButtonText = message.PrimaryButtonText,
                SecondaryButtonText = message.SecondaryButtonText ?? "",
                CloseButtonText = message.CloseButtonText ?? "",
                DefaultButton = ContentDialogButton.Primary
            };

            return await ShowAsync(dialog);
        }

        public async Task<ContentDialogResult> ShowContentDialogAsync(IViewModel viewModel)
        {
            var dialog = (ContentDialog)Activator.CreateInstance(viewModel.View);
            if (dialog is IViewModelContentDialog vmDialog)
            {
                vmDialog.ViewModel = viewModel;
            }
            else
            {
                dialog.DataContext = viewModel;
            }

            return await ShowAsync(dialog);
        }

        public async Task<ContentDialogResult> ShowErrorMessageDialogAsync(Exception ex)
        {
            return await ShowMessageDialogAsync(new ShowMessageDialogParameter(Language.Instance.Error, ex.Message));
        }

        private async Task<ContentDialogResult> ShowAsync(ContentDialog dialog)
        {
            currentDialog = dialog;
            var result = await dialog.ShowAsync();
            currentDialog = null;
            return result;
        }

        private FrameworkElement GetMessageContent(string message)
        {
            var run = new Run
            {
                Text = message
            };
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(run);
            var textBlock = new RichTextBlock();
            textBlock.Blocks.Add(paragraph);
            return textBlock;
        }
    }
}
