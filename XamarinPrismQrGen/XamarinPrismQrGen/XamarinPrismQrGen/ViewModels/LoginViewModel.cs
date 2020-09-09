using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
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
        public static TcpClient tcpClient;
        public static NetworkStream stream;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string id;
        public string Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        private DelegateCommand _loginCommand;
        private readonly INavigationService _navigationService;
        public DelegateCommand LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        private DelegateCommand _signUpCommand;

        public DelegateCommand SignUpCommand =>
            _signUpCommand ?? (_signUpCommand = new DelegateCommand(SignUp));

        async void SignUp()
        {
            await _navigationService.NavigateAsync("SignUp");
        }

        public LoginViewModel(INavigationService navigationService)
        {
            Title = "Main Page";
            _navigationService = navigationService;
        }

        async void Login()
        {
            if (string.IsNullOrEmpty(Id))
            {
                await App.Current.MainPage.DisplayAlert("경고", "아이디를 입력해주세요.", "확인");
                return;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("경고", "패스워드를 입력해주세요.", "확인");
                return;
            }
            tcpClient = new TcpClient();

            try
            {
                tcpClient.Connect("192.168.0.8", 9876);
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("경고", "서버가 연결되지 않았습니다.", "확인");
            }
            stream = tcpClient.GetStream();

            byte[] buf = new byte[256];
            buf = Encoding.ASCII.GetBytes("[Login]" + Id + "," + Password);
            stream.Write(buf, 0, buf.Length);
            int bufBytes = stream.Read(buf, 0, buf.Length);

            string message = Encoding.ASCII.GetString(buf, 0, bufBytes);

            if (message == "LogIn")
            {
                await _navigationService.NavigateAsync("MainPage");

            }

            else if (message == "AlreadyLogin")
            {
                await App.Current.MainPage.DisplayAlert("경고", "로그인중입니다.", "확인");
            }

            else if (message == "Fail")
            {
                await App.Current.MainPage.DisplayAlert("경고", "아이디 또는 패스워드를 정확히입력해주세요.", "확인");
            }
        }
    }
}
