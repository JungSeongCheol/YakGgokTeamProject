using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace XamarinPrismQrGen.ViewModels
{
    public class SignUpViewModel : BindableBase
    {
        //private TcpClient tcpClient = LoginViewModel.tcpClient;
        private TcpClient tcpClient;
        private NetworkStream stream;
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
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        private string phone;
        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        private readonly INavigationService _navigationService;
        public SignUpViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private DelegateCommand _signCommand;
        public DelegateCommand SignCommand =>
            _signCommand ?? (_signCommand = new DelegateCommand(SignUp));
        private DelegateCommand _backCommand;
        public DelegateCommand BackCommand =>
            _backCommand ?? (_backCommand = new DelegateCommand(Back));

        async void Back()
        {
            await _navigationService.NavigateAsync("Login");
        }

        async void SignUp()
        {
            tcpClient = new TcpClient();
            if (string.IsNullOrEmpty(Id))
            {
                await App.Current.MainPage.DisplayAlert("경고", "아이디를 입력해주세요", "확인");
                return;
            }
            else if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("경고", "패스워드를 입력해주세요", "확인");
                return;
            }
            else if (string.IsNullOrEmpty(Name))
            {
                await App.Current.MainPage.DisplayAlert("경고", "이름을 입력해주세요", "확인");
                return;
            }
            else if (string.IsNullOrEmpty(Phone))
            {
                await App.Current.MainPage.DisplayAlert("경고", "전화번호를 입력해주세요", "확인");
                return;
            }
            try
            {
                tcpClient.Connect("192.168.0.8", 9876);
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("경고", "서버가 연결되지 않았습니다.", "확인");
            }
            stream = tcpClient.GetStream();

            byte[] buf = Encoding.UTF8.GetBytes($"[SignUp]pmemtbl,{Id},{Password},{Name},{Phone}");
            stream.Write(buf, 0, buf.Length);


            int bufBytes = stream.Read(buf, 0, buf.Length);
            string message = Encoding.UTF8.GetString(buf, 0, bufBytes);

            if (message == "succ")
            {
                await App.Current.MainPage.DisplayAlert("성공", "가입성공", "확인");
                await _navigationService.NavigateAsync("Login");
            }
            else if (message == "fail")
            {
                await App.Current.MainPage.DisplayAlert("실패", "가입실패", "확인");
            }
        }

    }
}
