using Org.BouncyCastle.Crypto.Tls;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessApp.ViewModels
{
    public class PrescriptionViewModel : BindableBase
    {
        IRegionManager _regionManager;
        public DelegateCommand<string> BackCommand { get; set; }

        private string patientId = MenuViewModel.Ps[MenuViewModel.SIndex].PatientId;
        public string PatientId { 
            get => patientId; 
            set =>SetProperty(ref patientId,value); }

        private int hospitalNum = MenuViewModel.Ps[MenuViewModel.SIndex].HospitalNum;
        public int HospitalNum { 
            get => hospitalNum; 
            set => SetProperty(ref hospitalNum,value); }

        private int pharmNum = MenuViewModel.Ps[MenuViewModel.SIndex].PharmNum;
        public int PharmNum { 
            get => pharmNum; 
            set => SetProperty(ref pharmNum, value); }

        public int AM = MenuViewModel.Ps[MenuViewModel.SIndex].PharmNum;
        public int BM = MenuViewModel.Ps[MenuViewModel.SIndex].PharmNum;
        public int CM = MenuViewModel.Ps[MenuViewModel.SIndex].PharmNum;

        //private string medicine = MenuViewModel.Ps[MenuViewModel.SIndex].Medicine;
        //public string Medicine { 
        //    get => medicine; 
        //    set => SetProperty(ref medicine, value); }

        private string tDate = MenuViewModel.Ps[MenuViewModel.SIndex].TDate;
        public string TDate { 
            get => tDate; 
            set => SetProperty(ref tDate, value); }

        private string getDate = MenuViewModel.Ps[MenuViewModel.SIndex].getDate;
        public string GetDate { 
            get => getDate; 
            set => SetProperty(ref getDate, value); }

        private string hoName = MenuViewModel.Ps[MenuViewModel.SIndex].HoName;
        public string HoName { 
            get=>hoName; 
            set=> SetProperty(ref hoName, value); }

        private string holocation = MenuViewModel.Ps[MenuViewModel.SIndex].Holocation;
        public string Holocation { 
            get => holocation; 
            set => SetProperty(ref holocation, value); }

        private string pharmName = MenuViewModel.Ps[MenuViewModel.SIndex].PharmName;
        public string PharmName { 
            get => pharmName; 
            set => SetProperty(ref pharmName, value); }

        private string plocation = MenuViewModel.Ps[MenuViewModel.SIndex].Plocation;
        public string Plocation { 
            get => plocation; 
            set => SetProperty(ref plocation, value); }

        private string pName = MenuViewModel.Ps[MenuViewModel.SIndex].PName;
        public string PName
        {
            get => pName;
            set => SetProperty(ref pName, value);
        }

        private string pId = MenuViewModel.Ps[MenuViewModel.SIndex].PatientId;
        public string PId
        {
            get => pId;
            set => SetProperty(ref pId, value);
        }

        private string medicine = MenuViewModel.Ps[MenuViewModel.SIndex].Medicine;
        public string Medicine
        {
            get => medicine;
            set => SetProperty(ref medicine, value);
        }

        public PrescriptionViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            BackCommand = new DelegateCommand<string>(Backs);
        }

        private void Backs(string uri)
        {
            _regionManager.RequestNavigate("Menu", uri);
            _regionManager.Regions["Hello"].RemoveAll();
        }
    }
}
