using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PrecriptionModule
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModuleLoadMaster : ContentPage
    {
        public ListView ListView;

        public ModuleLoadMaster()
        {
            InitializeComponent();

            BindingContext = new ModuleLoadMasterViewModel();
            ListView = MenuItemsListView;
        }

        class ModuleLoadMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<ModuleLoadMasterMenuItem> MenuItems { get; set; }

            public ModuleLoadMasterViewModel()
            {
                MenuItems = new ObservableCollection<ModuleLoadMasterMenuItem>(new[]
                {
                    new ModuleLoadMasterMenuItem { Id = 0, Title = "Page 1" },
                    new ModuleLoadMasterMenuItem { Id = 1, Title = "Page 2" },
                    new ModuleLoadMasterMenuItem { Id = 2, Title = "Page 3" },
                    new ModuleLoadMasterMenuItem { Id = 3, Title = "Page 4" },
                    new ModuleLoadMasterMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}