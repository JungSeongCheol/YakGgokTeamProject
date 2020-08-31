using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XamarinPrismQrGen.ViewModels
{
    public class ViewAViewModel : BindableBase
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

        public ViewAViewModel(INavigationService navigationService)
        {
            Title = "ViewA";
            _navigationService = navigationService;
        }

        async void ExcuteNavigateCommand()
        {
            await _navigationService.NavigateAsync("ViewB");
        }

        //public ViewAViewModel(INavigationService navigationService) : base(navigationService)
        //{
        //    Title = "My ViewA";
        //}
    }
}
