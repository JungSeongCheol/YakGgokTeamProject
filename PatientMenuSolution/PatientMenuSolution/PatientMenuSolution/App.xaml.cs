using Prism;
using Prism.Ioc;
using PatientMenuSolution.ViewModels;
using PatientMenuSolution.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Prism.Mvvm;

namespace PatientMenuSolution
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/PatientMenuPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<PatientMenuPage, PatientMenuPageViewModel>();
            containerRegistry.RegisterForNavigation<MemDetailPage, MemDetailPageViewModel>();
        }

    }
}
