using MySqlConnector;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XamarinPrismQrGen.Helpers;
using XamarinPrismQrGen.Models;

namespace XamarinPrismQrGen.ViewModels
{
    public class QRViewModel : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _MedicineA;
        public string MedicineA
        {
            get { return _MedicineA; }
            set { SetProperty(ref _MedicineA, value); }
        }

        private string _MedicineB;
        public string MedicineB
        {
            get { return _MedicineB; }
            set { SetProperty(ref _MedicineB, value); }
        }

        private string _MedicineC;
        public string MedicineC
        {
            get { return _MedicineC; }
            set { SetProperty(ref _MedicineC, value); }
        }

        private ObservableCollection<MedicinePackage> mediPackage;
        public ObservableCollection<MedicinePackage> MediPackage
        {
            get { return mediPackage; }
            set { SetProperty(ref mediPackage, value); }
        }


        private DelegateCommand _navigateCommand;
        private readonly INavigationService _navigationService;
        public DelegateCommand NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand(ExcuteNavigateCommand));

        public QRViewModel(INavigationService navigationService)
        {
            MediPackage = new ObservableCollection<MedicinePackage>();

            Title = "QR";
            _navigationService = navigationService;
          
            SQLConn();
            //QRString = $"[QR]{MedicineA},{MedicineB},{MedicineC}";
        }

        async void ExcuteNavigateCommand()
        {
            await _navigationService.NavigateAsync("MainPage");
        }


        private void SQLConn()
        {
            MediPackage.Clear();
            //DB연동
            using (var conn = new MySqlConnection(Commons.CONSTRINGList))
            {
                try
                {
                    string strqry = " SELECT  TDate, IdSub, A, B, C " + //2020 09 27수정
                                    " FROM prescribeinfo " +
                                    " WHERE getdate IS NULL " +
                                    " AND PatientId = @Id " +
                                    " ORDER BY Tdate Desc "; //medicineinfo
                                                             //" WHERE " +; 

                    MySqlCommand cmd = new MySqlCommand(strqry, conn);
                    conn.Open();
                    

                    MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.VarChar);
                    paramId.Value = Commons.ID;
                    cmd.Parameters.Add(paramId);

                    MySqlDataReader R = cmd.ExecuteReader();

                    if (R.HasRows)
                    {
                        while (R.Read())
                        {
                            MedicinePackage temp = new MedicinePackage
                            {
                                TDate = $"진료일 : {DateTime.Parse(R["TDate"].ToString()).ToString("yyyy-MM-dd tt hh:mm")}",
                                MediA_str = string.Format("A : {0}", (string.IsNullOrEmpty(R["A"].ToString())) ? "" : R["A"].ToString()),
                                MediB_str = string.Format("B : {0}", (string.IsNullOrEmpty(R["B"].ToString())) ? "" : R["B"].ToString()),
                                MediC_str = string.Format("C : {0}", (string.IsNullOrEmpty(R["C"].ToString())) ? "" : R["C"].ToString()),
                                IdSub = (string.IsNullOrEmpty(R["IdSub"].ToString())) ? "" : R["IdSub"].ToString(),
                                QRString = $"{R["IdSub"]},{R["A"]},{R["B"]},{R["C"]}"
                            };
                            MediPackage.Add(temp);
                        }
                    }
                    else
                    {

                    }
                    R.Close();
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
    }
}
