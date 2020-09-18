using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ProcessApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessApp
{
    public class ProcessAModule : IModule
    {
        private IRegionManager _regionManager;

        public ProcessAModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //_regionManager.RegisterViewWithRegion("Menu", typeof(MenuView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MenuView>();
            containerRegistry.RegisterForNavigation<InfoView>();
        }
    }
}
