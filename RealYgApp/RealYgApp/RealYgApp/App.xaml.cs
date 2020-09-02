using Prism;
using Prism.Ioc;
using RealYgApp.ViewModels;
using RealYgApp.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;

namespace RealYgApp
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

            await NavigationService.NavigateAsync("NavigationPage/PatientPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<PatientPage, PatientPageViewModel>();
            containerRegistry.RegisterForNavigation<DetailPage, DetailPageViewModel>();
        }
    }
}
