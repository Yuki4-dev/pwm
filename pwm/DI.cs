#nullable enable

using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using pwm.Core.API;
using pwm.Core.Crypto;
using pwm.Core.Store;
using pwm.Service;
using pwm.Service.Setting;
using pwm.Store;
using pwm.UI;
using pwm.ViewModels;

namespace pwm
{
    public class DI
    {
        public static IDependencyServiceProvider? Provider { get; set; }

        public static T Get<T>()
        {
            var services = Provider != null ? Provider.GetService<T>() : Ioc.Default.GetService<T>();
            return services!;
        }

        public static void Injection()
        {
            var sc = new ServiceCollection();

            // API
            _ = sc.AddSingleton<ILogger, ConsoleLogger>();
            _ = sc.AddSingleton<IApplicationThemeListener, ApplicationThemeListener>();
            _ = sc.AddSingleton<IApplicationEventManager, ApplicationEventManager>();

            // Service
            _ = sc.AddSingleton<IPasswordDataProvider, PasswordDataProvider>();
            _ = sc.AddSingleton<IPasswordAnalyzeService, PasswordAnalyzeService>();
            _ = sc.AddSingleton<ICryptographyLogic, BasicCryptographyLogic>();
            _ = sc.AddSingleton<IDialogService, DialogService>();
            _ = sc.AddSingleton<IFileAccessService, FileAccessService>();
            _ = sc.AddSingleton<IAppSettingProvider, AppSettingProvider>();

            // Store
            _ = sc.AddSingleton<IApplicationSettingStore, ApplicationLocalSettingStore>();
            _ = sc.AddSingleton<IPasswordInformationEntityStore, PasswordInformationEntityStore>();

            // ViewModel
            _ = sc.AddSingleton<PasswordPageViewModel>();
            _ = sc.AddSingleton<SettingPageViewModel>();

            Ioc.Default.ConfigureServices(sc.BuildServiceProvider());
        }
    }
}
