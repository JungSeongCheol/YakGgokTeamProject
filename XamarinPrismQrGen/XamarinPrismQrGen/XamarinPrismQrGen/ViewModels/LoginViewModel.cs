using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using XamarinPrismQrGen.Helpers;

namespace XamarinPrismQrGen.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string userId;
        public string UserId
        {
            get { return userId; }
            set { SetProperty(ref userId, value); }
        }

        private string userPwd;
        public string UserPwd
        {
            get { return userPwd; }
            set { SetProperty(ref userPwd, value); }
        }

        public TcpClient tcpClient;
        public NetworkStream stream;

        private DelegateCommand _navigateCommand;
        private readonly INavigationService _navigationService;
        public DelegateCommand NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand(ExcuteNavigateCommand));

        public LoginViewModel(INavigationService navigationService)
        {
            Title = "Main Page";
            _navigationService = navigationService;
        }

        async void ExcuteNavigateCommand()
        {
            tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect("192.168.0.165", 9876);
            }
            catch (Exception)
            {

            }
            stream = tcpClient.GetStream();

            byte[] buf = new byte[256];
            buf = Encoding.ASCII.GetBytes("[Login]" + UserId + "," + UserPwd);
            stream.Write(buf, 0, buf.Length);
            int bufBytes = stream.Read(buf, 0, buf.Length);

            string message = Encoding.ASCII.GetString(buf, 0, bufBytes);

            if (message == "OK")
            {
                await _navigationService.NavigateAsync("MainPage");
            }
        }
    }
}
