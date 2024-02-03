using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using pwm.Core.API;
using pwm.Core.Models;
using pwm.Core.Store;
using pwm.Models.Core.Password;
using pwm.Models.DialogService;
using pwm.Models.PasswordAnalyzeService;
using pwm.Models.PasswordDataProvider;
using pwm.Models.PasswordInformationContentDialog;
using pwm.Models.PasswordPage;
using pwm.Models.Setting;
using pwm.Service;
using pwm.Store;
using pwm.ViewModels;
using pwm.ViewModels.Dialogs;
using pwm.Views.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace pwm.tests.ViewModels
{
    [TestClass]
    public class PasswordPageViewModelTest
    {
        private readonly Mock<IApplicationEventManager> applicationEventManager = new Mock<IApplicationEventManager>();

        private readonly Mock<IDialogService> dialogService = new Mock<IDialogService>();

        private readonly Mock<IPasswordAnalyzeService> passwordAnalyzeService = new Mock<IPasswordAnalyzeService>();

        private readonly Mock<IPasswordDataProvider> passwordDataProvider = new Mock<IPasswordDataProvider>();

        private readonly Mock<IAppSettingProvider> appSettingProvider = new Mock<IAppSettingProvider>();

        private readonly IPasswordInformationEntityStore passwordInformationEntityStore = new PasswordInformationEntityStore();

        private PasswordPageViewModel Target => new PasswordPageViewModel(new ConsoleLogger(), appSettingProvider.Object, passwordInformationEntityStore, applicationEventManager.Object, dialogService.Object, passwordDataProvider.Object, passwordAnalyzeService.Object);

        [TestInitialize]
        public void TestInitialize()
        {
            var sp = new MockDependencyServiceProvider();
            sp.AddService(passwordAnalyzeService.Object);

            DI.Provider = sp;
        }

        [TestMethod]
        public void OnLoadedAsync_Test_1()
        {
            _ = dialogService.Setup(d => d.ShowContentDialogAsync(It.IsAny<InitializeContentDialogViewModel>()))
                .Returns(new InvocationFunc(i =>
                {
                    var viewModel = (InitializeContentDialogViewModel)i.Arguments[0];
                    viewModel.CreateNewCommand.Execute(null);
                    return Task.FromResult(ContentDialogResult.Primary);
                }));

            var target = Target;

            // Test Method
            target.OnLoadedAsync().Wait();

            passwordDataProvider.Verify(p => p.OpenAsync(), Times.Never);

            passwordDataProvider.Verify(p => p.CreateAsync(It.IsAny<PasswordInformationEntity[]>()), Times.Once);

            passwordDataProvider.Verify(p => p.SaveAsync(It.IsAny<PasswordInformationEntity[]>(), It.IsAny<PasswordInformationSetting>()), Times.Never);
        }

        [TestMethod]
        public void OnLoadedAsync_Test_2()
        {
            _ = dialogService.Setup(d => d.ShowContentDialogAsync(It.IsAny<InitializeContentDialogViewModel>()))
                .Returns(new InvocationFunc(i =>
                {
                    var viewModel = (InitializeContentDialogViewModel)i.Arguments[0];
                    viewModel.OpenOneCommand.Execute(null);
                    return Task.FromResult(ContentDialogResult.Primary);
                }));

            var target = Target;

            // Test Method
            target.OnLoadedAsync().Wait();

            passwordDataProvider.Verify(p => p.OpenAsync(), Times.Once);

            passwordDataProvider.Verify(p => p.CreateAsync(It.IsAny<PasswordInformationEntity[]>()), Times.Never);

            passwordDataProvider.Verify(p => p.SaveAsync(It.IsAny<PasswordInformationEntity[]>(), It.IsAny<PasswordInformationSetting>()), Times.Never);
        }

        [TestMethod]
        public void EscapeDown_Test_1()
        {
            var pInfo = PasswordInformationEntityStoreExtension.ConvertPasswordInformation(CreatePasswordInformationEntities(), PasswordDisplayStatus.Mask);
            foreach (var p in pInfo)
            {
                p.WebSiteName.IsHighlightEnable = true;
                p.WebSiteURI.IsHighlightEnable = true;
                p.UserId.IsHighlightEnable = true;
                p.Description.IsHighlightEnable = true;
            }

            var target = Target;
            target.DisplayPasswordInformation.AddRange(pInfo);

            // Test Command
            target.EscapeDownCommand.Execute(null);

            foreach (var p in pInfo)
            {
                Assert.IsFalse(p.WebSiteName.IsHighlightEnable);
                Assert.IsFalse(p.WebSiteURI.IsHighlightEnable);
                Assert.IsFalse(p.UserId.IsHighlightEnable);
                Assert.IsFalse(p.Description.IsHighlightEnable);
            }
        }

        [TestMethod]
        public void Add_Test_1()
        {
            var webSiteName = "1";
            var userId = "2";
            var webSiteURI = "3";
            var password = "4";
            var description = "5";

            PasswordInformationContentDialogViewModel viewModel = null;
            _ = dialogService.Setup(d => d.ShowContentDialogAsync(It.IsAny<PasswordInformationContentDialogViewModel>()))
               .Returns(new InvocationFunc(i =>
               {
                   viewModel = (PasswordInformationContentDialogViewModel)i.Arguments[0];
                   viewModel.WebSiteName = webSiteName;
                   viewModel.UserId = userId;
                   viewModel.WebSiteURI = webSiteURI;
                   viewModel.Password = password;
                   viewModel.Description = description;
                   return Task.FromResult(ContentDialogResult.Primary);
               }));

            _ = passwordAnalyzeService.Setup(p => p.AnalyzePassword(It.IsAny<PasswordInformationEntity>()))
                .Returns(new AnalyzePasswordResult(true));

            var target = Target;

            // Test Command
            target.AddPasswordCommand.Execute(null);

            Assert.AreEqual(PasswordInformationContentDialogType.Add, viewModel?.PasswordInformationContentDialogType);

            Assert.AreEqual(1, target.DisplayPasswordInformation.Count);
            Assert.AreEqual(webSiteName, target.DisplayPasswordInformation[0].WebSiteName.Text);
            Assert.AreEqual(userId, target.DisplayPasswordInformation[0].UserId.Text);
            Assert.AreEqual(webSiteURI, target.DisplayPasswordInformation[0].WebSiteURI.Text);
            Assert.AreEqual(password, target.DisplayPasswordInformation[0].Password.GetPassword());
            Assert.AreEqual(description, target.DisplayPasswordInformation[0].Description.Text);
        }

        [TestMethod]
        public void Add_Test_2()
        {
            _ = dialogService.Setup(d => d.ShowContentDialogAsync(It.IsAny<PasswordInformationContentDialogViewModel>()))
               .Returns(Task.FromResult(ContentDialogResult.Secondary));

            var target = Target;

            // Test Command
            target.AddPasswordCommand.Execute(null);

            Assert.AreEqual(0, target.DisplayPasswordInformation.Count);
        }

        [TestMethod]
        public void View_Test_1()
        {
            var exp = PasswordInformation.FromEntity(CreatePasswordInformationEntity());

            PasswordInformationContentDialogViewModel viewModel = null;
            _ = dialogService.Setup(d => d.ShowContentDialogAsync(It.IsAny<PasswordInformationContentDialogViewModel>()))
               .Callback<object>((v) => viewModel = (PasswordInformationContentDialogViewModel)v)
               .Returns(Task.FromResult(ContentDialogResult.Primary));

            _ = passwordAnalyzeService.Setup(p => p.AnalyzePassword(It.IsAny<PasswordInformationEntity>()))
                .Returns(new AnalyzePasswordResult(true));

            var target = Target;
            target.DisplayPasswordInformation.Add(exp);

            // Test Command
            target.ViewPasswordCommand.Execute(exp);

            Assert.AreEqual(PasswordInformationContentDialogType.View, viewModel?.PasswordInformationContentDialogType);

            Assert.AreEqual(exp.WebSiteName.Text, viewModel.WebSiteName);
            Assert.AreEqual(exp.UserId.Text, viewModel.UserId);
            Assert.AreEqual(exp.WebSiteURI.Text, viewModel.WebSiteURI);
            Assert.AreEqual(exp.Password.GetPassword(), viewModel.Password);
            Assert.AreEqual(exp.Description.Text, viewModel.Description);
        }

        [TestMethod]
        public void Edit_Test_1()
        {
            var edit = CreatePasswordInformationEntity();
            var pInfo = PasswordInformation.FromEntity(edit);

            SetUpPasswordInformationEntityStore(edit);

            PasswordInformationEntity oldEntity = null;
            PasswordInformationContentDialogViewModel viewModel = null;
            _ = dialogService.Setup(d => d.ShowContentDialogAsync(It.IsAny<PasswordInformationContentDialogViewModel>()))
               .Returns(new InvocationFunc(i =>
               {
                   viewModel = (PasswordInformationContentDialogViewModel)i.Arguments[0];

                   oldEntity = viewModel.GetPasswordInformationEntity();

                   viewModel.WebSiteName = "1";
                   viewModel.UserId = "2";
                   viewModel.WebSiteURI = "3";
                   viewModel.Password = "4";
                   viewModel.Description = "5";

                   return Task.FromResult(ContentDialogResult.Primary);
               }));

            _ = passwordAnalyzeService.Setup(p => p.AnalyzePassword(It.IsAny<PasswordInformationEntity>()))
                .Returns(new AnalyzePasswordResult(true));

            var target = Target;

            // Test Command
            target.EditPasswordCommand.Execute(pInfo);

            Assert.AreEqual(PasswordInformationContentDialogType.Edit, viewModel?.PasswordInformationContentDialogType);

            Assert.AreEqual(edit.WebSiteName, oldEntity.WebSiteName);
            Assert.AreEqual(edit.UserId, oldEntity.UserId);
            Assert.AreEqual(edit.WebSiteURI, oldEntity.WebSiteURI);
            Assert.AreEqual(edit.Password, oldEntity.Password);
            Assert.AreEqual(edit.Description, oldEntity.Description);

            Assert.AreEqual(1, target.DisplayPasswordInformation.Count);
            Assert.AreEqual("1", target.DisplayPasswordInformation[0].WebSiteName.Text);
            Assert.AreEqual("2", target.DisplayPasswordInformation[0].UserId.Text);
            Assert.AreEqual("3", target.DisplayPasswordInformation[0].WebSiteURI.Text);
            Assert.AreEqual("4", target.DisplayPasswordInformation[0].Password.GetPassword());
            Assert.AreEqual("5", target.DisplayPasswordInformation[0].Description.Text);
        }

        [TestMethod]
        public void Edit_Test_2()
        {
            var edit = CreatePasswordInformationEntity();
            var pInfo = PasswordInformation.FromEntity(edit);

            SetUpPasswordInformationEntityStore(edit);

            PasswordInformationContentDialogViewModel viewModel = null;
            _ = dialogService.Setup(d => d.ShowContentDialogAsync(It.IsAny<PasswordInformationContentDialogViewModel>()))
               .Returns(new InvocationFunc(i =>
               {
                   viewModel = (PasswordInformationContentDialogViewModel)i.Arguments[0];
                   viewModel.WebSiteName = "1";
                   viewModel.UserId = "2";
                   viewModel.WebSiteURI = "3";
                   viewModel.Password = "4";
                   viewModel.Description = "5";

                   return Task.FromResult(ContentDialogResult.Secondary);
               }));

            _ = passwordAnalyzeService.Setup(p => p.AnalyzePassword(It.IsAny<PasswordInformationEntity>()))
                .Returns(new AnalyzePasswordResult(true));

            var target = Target;

            // Test Command
            target.EditPasswordCommand.Execute(pInfo);

            Assert.AreEqual(PasswordInformationContentDialogType.Edit, viewModel?.PasswordInformationContentDialogType);

            Assert.AreEqual(0, target.DisplayPasswordInformation.Count);
        }


        [TestMethod]
        public void Search_Test_1()
        {
            SetUpPasswordInformationEntityStore(CreatePasswordInformationEntities());

            _ = passwordAnalyzeService.Setup(p => p.MatchPassword(It.IsAny<PasswordInformationEntity>(), It.IsAny<PasswordAnalyzeOption[]>()))
                .Returns(new MatchPasswordResult(true, Array.Empty<PasswordAnalyzeInformation>()));

            appSettingProvider.Setup(s => s.PasswordSearchTargetSetting).Returns(new PasswordSearchTargetSetting());

            var target = Target;
            target.SearchKeyword = "hoge";

            // Test Command
            target.SearchPasswordCommand.Execute(null);

            Assert.AreEqual(10, target.DisplayPasswordInformation.Count);

            Assert.AreEqual(1, target.SuggestWords.Count);
        }

        [TestMethod]
        public void Search_Test_2()
        {
            SetUpPasswordInformationEntityStore(CreatePasswordInformationEntities());

            _ = passwordAnalyzeService.Setup(p => p.MatchPassword(It.IsAny<PasswordInformationEntity>(), It.IsAny<PasswordAnalyzeOption[]>()))
                .Returns(new MatchPasswordResult(false, Array.Empty<PasswordAnalyzeInformation>()));

            appSettingProvider.Setup(s => s.PasswordSearchTargetSetting).Returns(new PasswordSearchTargetSetting());

            var target = Target;
            target.SearchKeyword = "hoge";

            // Test Command
            target.SearchPasswordCommand.Execute(null);

            Assert.AreEqual(0, target.DisplayPasswordInformation.Count);

            Assert.AreEqual(1, target.SuggestWords.Count);
        }

        [TestMethod]
        public void Search_Test_3()
        {
            SetUpPasswordInformationEntityStore(CreatePasswordInformationEntities());

            var target = Target;
            target.SearchKeyword = " ";

            passwordAnalyzeService.Verify(p => p.MatchPassword(It.IsAny<PasswordInformationEntity>(), It.IsAny<PasswordAnalyzeOption[]>()), Times.Never);

            // Test Command
            target.SearchPasswordCommand.Execute(null);

            Assert.AreEqual(10, target.DisplayPasswordInformation.Count);
        }

        [TestMethod]
        public void Delete_Test_1()
        {
            var entities = CreatePasswordInformationEntities();
            var delete = PasswordInformation.FromEntity(entities[5]);

            SetUpPasswordInformationEntityStore(entities);

            _ = dialogService.Setup(d => d.ShowMessageDialogAsync(It.IsAny<ShowMessageDialogParameter>()))
                .Returns(Task.FromResult(ContentDialogResult.Primary));

            var target = Target;

            // Test Command
            target.DeletePasswordCommand.Execute(delete);

            Assert.AreEqual(9, target.DisplayPasswordInformation.Count);
            Assert.IsFalse(target.DisplayPasswordInformation.Contains(delete));
        }

        [TestMethod]
        public void Delete_Test_2()
        {
            var entities = CreatePasswordInformationEntities();
            var delete = PasswordInformation.FromEntity(entities[5]);

            SetUpPasswordInformationEntityStore(entities);

            _ = dialogService.Setup(d => d.ShowMessageDialogAsync(It.IsAny<ShowMessageDialogParameter>()))
                .Returns(Task.FromResult(ContentDialogResult.Secondary));

            var target = Target;

            // Test Command
            target.DeletePasswordCommand.Execute(delete);
            target.Update();

            Assert.AreEqual(10, target.DisplayPasswordInformation.Count);
            Assert.IsTrue(target.DisplayPasswordInformation.Contains(delete));
        }

        [TestMethod]
        public void DeleteSelection_Test_1()
        {
            SetUpPasswordInformationEntityStore(CreatePasswordInformationEntities());

            _ = dialogService.Setup(d => d.ShowMessageDialogAsync(It.IsAny<ShowMessageDialogParameter>()))
                .Returns(Task.FromResult(ContentDialogResult.Primary));

            var target = Target;
            target.Update();

            var delete1 = target.DisplayPasswordInformation[3];
            var delete2 = target.DisplayPasswordInformation[4];
            var delete3 = target.DisplayPasswordInformation[8];
            delete1.IsChecked = true;
            delete2.IsChecked = true;
            delete3.IsChecked = true;

            // Test Command
            target.DeleteSelectionCommand.Execute(null);

            Assert.AreEqual(7, target.DisplayPasswordInformation.Count);
            Assert.IsFalse(target.DisplayPasswordInformation.Contains(delete1));
            Assert.IsFalse(target.DisplayPasswordInformation.Contains(delete2));
            Assert.IsFalse(target.DisplayPasswordInformation.Contains(delete3));
        }

        [TestMethod]
        public void DeleteSelection_Test_2()
        {
            SetUpPasswordInformationEntityStore(CreatePasswordInformationEntities());

            _ = dialogService.Setup(d => d.ShowMessageDialogAsync(It.IsAny<ShowMessageDialogParameter>()))
                .Returns(Task.FromResult(ContentDialogResult.Secondary));

            var target = Target;
            target.Update();

            var delete1 = target.DisplayPasswordInformation[3];
            delete1.IsChecked = true;

            // Test Command
            target.DeleteSelectionCommand.Execute(null);

            Assert.AreEqual(10, target.DisplayPasswordInformation.Count);
        }

        [TestMethod]
        public void DeleteSelection_Test_3()
        {
            SetUpPasswordInformationEntityStore(CreatePasswordInformationEntities());

            var target = Target;
            target.Update();

            // Test Command
            target.DeleteSelectionCommand.Execute(null);

            dialogService.Verify(d => d.ShowMessageDialogAsync(It.IsAny<ShowMessageDialogParameter>()), Times.Once);

            Assert.AreEqual(10, target.DisplayPasswordInformation.Count);
        }

        [TestMethod]
        public void Save_Test_1()
        {
            SetUpPasswordInformationEntityStore(CreatePasswordInformationEntities());

            _ = passwordDataProvider.Setup(p => p.CreateAsync(It.IsAny<PasswordInformationEntity[]>()))
                .Returns(Task.FromResult(CreatePasswordDataSaveResult()));

            var target = Target;
            target.Update();

            // Test Command
            target.SaveCommand.Execute(null);

            passwordDataProvider.Verify(p => p.CreateAsync(It.IsAny<PasswordInformationEntity[]>()), Times.Once);
            passwordDataProvider.Verify(p => p.SaveAsync(It.IsAny<PasswordInformationEntity[]>(), It.IsAny<PasswordInformationSetting>()), Times.Never);
        }

        [TestMethod]
        public void Save_Test_2()
        {
            var dataResult = CreatePasswordDataSaveResult();

            passwordInformationEntityStore.Initialize(dataResult.PasswordDataContext.PasswordInformationSetting, dataResult.PasswordDataContext.PasswordInformationEntities);

            PasswordInformationSetting actSetting = null;
            _ = passwordDataProvider.Setup(p => p.SaveAsync(It.IsAny<PasswordInformationEntity[]>(), It.IsAny<PasswordInformationSetting>()))
                .Callback<PasswordInformationEntity[], PasswordInformationSetting>((p, setting) => actSetting = setting)
                .Returns(Task.FromResult(dataResult));

            var target = Target;
            target.Update();

            // Test Command
            target.SaveCommand.Execute(null);

            passwordDataProvider.Verify(p => p.CreateAsync(It.IsAny<PasswordInformationEntity[]>()), Times.Never);
            passwordDataProvider.Verify(p => p.SaveAsync(It.IsAny<PasswordInformationEntity[]>(), It.IsAny<PasswordInformationSetting>()), Times.Once);

            Assert.AreEqual("Password", actSetting.Password);
        }

        [TestMethod]
        public void Export_Test_1()
        {
            SetUpPasswordInformationEntityStore(CreatePasswordInformationEntities());

            _ = passwordDataProvider.Setup(p => p.ExportAsync(It.IsAny<PasswordInformationEntity[]>()))
                .Returns(Task.FromResult(CreatePasswordDataSaveResult()));

            var target = Target;

            target.Update();

            // Test Command
            target.ExportCommand.Execute(null);

            passwordDataProvider.Verify(p => p.ExportAsync(It.IsAny<PasswordInformationEntity[]>()), Times.Once);
        }

        private PasswordInformationEntity CreatePasswordInformationEntity(int i = 1)
        {
            return new PasswordInformationEntity(
                password: "Password-" + i,
                webSiteName: "webSiteName-" + i,
                webSiteURI: "WebSiteURI-" + i,
                userId: "UserId-" + i,
                description: "Description-" + i,
                updateDate: DateTime.Now
            );
        }

        private PasswordInformationEntity[] CreatePasswordInformationEntities()
        {
            return Enumerable.Range(1, 10).Select((num) => CreatePasswordInformationEntity(num)).ToArray();
        }

        private void SetUpPasswordInformationEntityStore(params PasswordInformationEntity[] entities)
        {
            passwordInformationEntityStore.Initialize(entities);
        }

        private PasswordDataSaveResult CreatePasswordDataSaveResult()
        {
            var entities = CreatePasswordInformationEntities();
            var file = new Mock<IStorageFile>();
            var context = new PasswordDataContext(new PasswordInformationSetting(file.Object, "Password"), entities);
            return new PasswordDataSaveResult(true, context);
        }
    }
}
