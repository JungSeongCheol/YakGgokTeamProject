using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XamarinPrismQrGen.ViewModels
{
    public class MainPageViewModel : /*ViewModelBase*/ BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private DelegateCommand _navigateCommand;
        private readonly INavigationService _navigationService;
        public DelegateCommand NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand(ExcuteNavigateCommand));

        public MainPageViewModel(INavigationService navigationService)
        {
            Title = "Main Page";
            _navigationService = navigationService;
        }
        
        async void ExcuteNavigateCommand()
        {
            await _navigationService.NavigateAsync("ViewA");
        }
    }
}
