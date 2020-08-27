using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XamarinPrismQrGen.ViewModels
{
    public class ViewBViewModel : BindableBase
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

        public ViewBViewModel(INavigationService navigationService)
        {
            Title = "ViewB";
            _navigationService = navigationService;
        }

        async void ExcuteNavigateCommand()
        {
            await _navigationService.NavigateAsync("MainPage");
        }
    }
}
