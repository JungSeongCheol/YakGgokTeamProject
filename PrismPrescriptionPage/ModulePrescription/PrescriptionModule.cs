using ModulePrescription.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModulePrescription
{
    public class PrescriptionModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion("ContentRegion", typeof(PrescriptionView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
