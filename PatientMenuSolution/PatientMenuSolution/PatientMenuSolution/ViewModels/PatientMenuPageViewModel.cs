using PatientMenuSolution.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatientMenuSolution.ViewModels
{
    public class PatientMenuPageViewModel : BindableBase
    {
        //TODO ChkHaveQR 구현, MemDetailPage

        INavigationService _navigationService;

        #region 속성
        private int count;
        public int Count
        {
            get { return count; }
            set { SetProperty(ref count, value); }
        }
        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string qRMenu;
        public string QRMenu
        {
            get { return qRMenu; }
            set { SetProperty(ref qRMenu, value); }
        }

        private string prescrip = "내 처방전 목록";
        public string Prescrip
        {
            get { return prescrip; }
            set { SetProperty(ref prescrip, value); }
        }

        private string noneQR;
        public string NoneQR
        {
            get { return noneQR; }
            set { SetProperty(ref noneQR, value); }
        }

        private bool chkHaveQR;
        public bool ChkHaveQR
        {
            get { return chkHaveQR; }
            set { SetProperty(ref chkHaveQR, value); }
        }


        private DelegateCommand goMemDetailPageCommand;
        public DelegateCommand GoMemDetailPageCommand => goMemDetailPageCommand ?? (goMemDetailPageCommand = new DelegateCommand(async () => await GoMemDetailPage()));

        private DelegateCommand goPrescriptionPageCommand;
        //public DelegateCommand GoPrescriptionPageCommand => goPrescriptionPageCommand ?? (goPrescriptionPageCommand = new DelegateCommand(async () => await GoPrescriptionPage()));

        private DelegateCommand goQRPageCommand;
        //public DelegateCommand GoQRPageCommand => goQRPageCommand ?? (goQRPageCommand = new DelegateCommand(async () => await GoQRPage()));
        #endregion


        public PatientMenuPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            initControl();
        }

        private void initControl()
        {
            if (string.IsNullOrEmpty(Commons.ID))
            {
                Title = "Login Please";
                QRMenu = string.Empty;
                NoneQR = "새로운 QR정보가 없습니다.";
            }
            else
            {
                Title = $"{Commons.ID}님";
                if (ChkHaveQR)
                {
                    NoneQR = string.Empty;
                    QRMenu = " New! ";
                }
            }
        }

        /// <summary>
        /// 큐알페이지 이동
        /// </summary>
        /// <returns></returns>
        /*
        async Task GoQRPage()
        {
            
            // 로그인 안되어있다면 로그인페이지로 이동.
            if (string.IsNullOrEmpty(Commons.ID))
            {
                NavigationParameters p = new NavigationParameters();
                await _navigationService.NavigateAsync(로그인페이지);
            }
            else
            {
                NavigationParameters p = new NavigationParameters();
                await _navigationService.NavigateAsync(큐알페이지);
            }
            
        }
        */

        /// <summary>
        /// 처방전 관리 페이지 이동
        /// </summary>
        /// <returns></returns>
        /*
        async Task GoPrescriptionPage()
        {

            // 로그인 안되어있다면 로그인페이지로 이동.
            if (string.IsNullOrEmpty(Commons.ID))
            {
                NavigationParameters p = new NavigationParameters();
                await _navigationService.NavigateAsync(로그인페이지);
            }
            else
            {
                NavigationParameters p = new NavigationParameters();
                await _navigationService.NavigateAsync(처방전페이지);
            }

        }*/

        /// <summary>
        /// 내정보 페이지 이동 
        /// </summary>
        /// <returns></returns>
        async Task GoMemDetailPage()
        {
            /*
            // 로그인 안되어있다면 로그인페이지로 이동.
            if (string.IsNullOrEmpty(Commons.ID))
            {
                NavigationParameters p = new NavigationParameters();
                await _navigationService.NavigateAsync(로그인페이지);
            }
            else
            {*/
                await _navigationService.NavigateAsync("MemDetailPage");
            //}
            
        }
    }
}
