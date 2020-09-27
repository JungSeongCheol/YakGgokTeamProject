using MySqlConnector;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using XamarinPrismQrGen.Helpers;

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

        private string _QRString;
        public string QRString
        {
            get { return _QRString; }
            set { SetProperty(ref _QRString, value); }
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


        private DelegateCommand _navigateCommand;
        private readonly INavigationService _navigationService;
        public DelegateCommand NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand(ExcuteNavigateCommand));

        public QRViewModel(INavigationService navigationService)
        {
            Title = "QR";
            _navigationService = navigationService;
            SQLConn();
            QRString = $"[QR]{MedicineA},{MedicineB},{MedicineC}";
        }

        async void ExcuteNavigateCommand()
        {
            await _navigationService.NavigateAsync("MainPage");
        }


        private void SQLConn()
        {
            //DB연동
            using (var conn = new MySqlConnection(Commons.CONSTRINGList))
            {
                try
                {
                    string strqry = "SELECT DevId, PatientId, A, B, C " +
                                    " FROM medicineinfo ";

                    MySqlCommand cmd = new MySqlCommand(strqry, conn);
                    conn.Open();

                    //MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.VarChar);
                    //paramId.Value = Commons.ID;
                    //cmd.Parameters.Add(paramId);

                    //MySqlParameter paramStartDatePicker = new MySqlParameter("@StartDatePicker", MySqlDbType.DateTime);
                    //paramStartDatePicker.Value = DateTime.Parse(StartDatePicker);
                    //cmd.Parameters.Add(paramStartDatePicker);

                    //MySqlParameter paramEndDatePicker = new MySqlParameter("@EndDatePicker", MySqlDbType.DateTime);
                    //paramEndDatePicker.Value = DateTime.Parse(EndDatePicker);
                    //cmd.Parameters.Add(paramEndDatePicker);

                    //MySqlParameter paramSubject = new MySqlParameter("@Subject", MySqlDbType.VarChar);
                    //if (isSelected && Selecteditem.Num > 0 && Selecteditem.Num < 8)
                    //{
                    //    paramSubject.Value = "%" + Selecteditem.Subject + "%";
                    //}
                    //else
                    //{
                    //    paramSubject.Value = "%";
                    //}
                    //cmd.Parameters.Add(paramSubject);

                    //MySqlParameter paramInputSearch = new MySqlParameter("@InputSearch", MySqlDbType.VarChar);
                    //paramInputSearch.Value = string.IsNullOrWhiteSpace(InputSearch) ? "%" : "%" + InputSearch + "%";
                    //cmd.Parameters.Add(paramInputSearch);

                    MySqlDataReader R = cmd.ExecuteReader();

                    if (R.HasRows)
                    {
                        while (R.Read())
                        {
                            MedicineA = R["A"].ToString();
                            MedicineB = R["B"].ToString();
                            MedicineC = R["C"].ToString();
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
