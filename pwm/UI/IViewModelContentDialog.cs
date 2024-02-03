#nullable enable

namespace pwm.Views.UI
{
    public interface IViewModelContentDialog
    {
        void ForceClosed();

        object ViewModel { get; set; }
    }
}
