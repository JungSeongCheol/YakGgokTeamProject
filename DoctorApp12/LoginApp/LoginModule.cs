using LoginApp.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginApp
{
    public class LoginModule : IModule
    {
        private IRegionManager regionManager;

        public LoginModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(LoginView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginView>();
            containerRegistry.RegisterForNavigation<JoinView>();
            containerRegistry.RegisterForNavigation<FindView>();
            containerRegistry.RegisterForNavigation<ResetView>();
        }
    }
}
