using DoctorApp11.Views;
using LoginApp;
using LoginApp.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using ProcessApp.Views;
using ProcessApp;
using System.Diagnostics;
using System.Windows;

namespace DoctorApp11
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginView>();
            containerRegistry.RegisterForNavigation<MenuView>();
            //containerRegistry.RegisterForNavigation<HListView>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<LoginModule>();
            moduleCatalog.AddModule<ProcessAModule>();
        }
    }
}
