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
    public class PatientPageViewModel : ViewModelBase
    {
        INavigationService _navigationService;
        #region 속성
        ObservableCollection<Patient> patients;
        public ObservableCollection<Patient> Patients { get => patients; set => SetProperty(ref patients, value); }

        public ObservableCollection<Hospital> Searchlist { get; set; }

        public DelegateCommand CheackCommand { get; private set; }
        private DelegateCommand showUserDetailCommand;
        public DelegateCommand ShowUserDetailCommand => showUserDetailCommand ?? (showUserDetailCommand = new DelegateCommand(ExecuteNavigateCommand));

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
            }
        }

        private string scriptCount;
        public string ScriptCount
        {
            get { return scriptCount; }
            set
            {
                SetProperty(ref scriptCount, value);
            }
        }

        private string startDatePicker = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
        public string StartDatePicker
        {
            get { return startDatePicker; }
            set
            {
                SetProperty(ref startDatePicker, value);
            }

        }

        private string endDatePicker = DateTime.Now.ToString("yyyy-MM-dd");
        public string EndDatePicker
        {
            get { return endDatePicker; }
            set
            {
                SetProperty(ref endDatePicker, value);
            }
        }
        private Hospital selecteditem;
        public Hospital Selecteditem
        {
            get => selecteditem;
            set
            {
                if (selecteditem != value)
                {
                    SetProperty(ref selecteditem, value);
                    isSelected = true;
                }
            }
        }

        private Patient seletedlist;
        public Patient Seletedlist
        {
            get { return seletedlist; }
            //set { SetProperty(ref seletedlist, value); }
            set
            {
                SetProperty(ref seletedlist, value);
                Commons.patient = seletedlist;
            }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }

        private string inputSearch;
        public string InputSearch
        {
            get { return inputSearch; }
            set { SetProperty(ref inputSearch, value); }
        }
        #endregion 

        public PatientPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            InputSearch = string.Empty;

            //Combobox(Picker연결)
            Title = $"{Commons.ID}님의 처방전 정보";

            CheackCommand = new DelegateCommand(cheack);
            patients = new ObservableCollection<Patient>();

            SQLConn();
            init_cmb();
        }
        async void ExecuteNavigateCommand()
        {
            NavigationParameters p = new NavigationParameters();
            //p.add(key,value)
            p.Add("SeletedPatient", Seletedlist);

            await _navigationService.NavigateAsync("DetailPage", p);
        }
        private void cheack()
        {
            SQLConn();
        }

        public void init_cmb()
        {
            Searchlist = new ObservableCollection<Hospital>();

            Searchlist.Add(new Hospital { Num = 1, Subject = "종합병원" });
            Searchlist.Add(new Hospital { Num = 2, Subject = "내과" });
            Searchlist.Add(new Hospital { Num = 3, Subject = "성형외과" });
            Searchlist.Add(new Hospital { Num = 4, Subject = "안과" });
            Searchlist.Add(new Hospital { Num = 5, Subject = "이비인후과" });
            Searchlist.Add(new Hospital { Num = 6, Subject = "정형외과" });
            Searchlist.Add(new Hospital { Num = 7, Subject = "치과" });
            Searchlist.Add(new Hospital { Num = 8, Subject = "직접입력" });
        }
        private void SQLConn()
        {
            //DB연동
            patients.Clear();

            using (var conn = new MySqlConnection(Commons.CONSTRINGList))
            {
                try
                {
                    string strqry = " SELECT TDate, ParmName, HoName, Holocation, getdate, Medicine,Plocation FROM prescribeinfo " +
                                    " WHERE PatientId = @Id " +
                                    " AND  Tdate Between @StartDatePicker AND @EndDatePicker " +
                                    " AND HoName like @Subject " +
                                    " AND HoName like @InputSearch " +
                                    " ORDER BY Tdate Desc ";

                    MySqlCommand cmd = new MySqlCommand(strqry, conn);
                    conn.Open();

                    MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.VarChar);
                    paramId.Value = Commons.ID;
                    cmd.Parameters.Add(paramId);

                    MySqlParameter paramStartDatePicker = new MySqlParameter("@StartDatePicker", MySqlDbType.DateTime);
                    paramStartDatePicker.Value = DateTime.Parse(StartDatePicker);
                    cmd.Parameters.Add(paramStartDatePicker);

                    MySqlParameter paramEndDatePicker = new MySqlParameter("@EndDatePicker", MySqlDbType.DateTime);
                    paramEndDatePicker.Value = DateTime.Parse(EndDatePicker);
                    cmd.Parameters.Add(paramEndDatePicker);

                    MySqlParameter paramSubject = new MySqlParameter("@Subject", MySqlDbType.VarChar);
                    if (isSelected && Selecteditem.Num > 0 && Selecteditem.Num < 8)
                    {
                        paramSubject.Value = "%" + Selecteditem.Subject + "%";
                    }
                    else
                    {
                        paramSubject.Value = "%";
                    }
                    cmd.Parameters.Add(paramSubject);

                    MySqlParameter paramInputSearch = new MySqlParameter("@InputSearch", MySqlDbType.VarChar);
                    paramInputSearch.Value = string.IsNullOrWhiteSpace(InputSearch) ? "%" : "%" + InputSearch + "%";
                    cmd.Parameters.Add(paramInputSearch);

                    MySqlDataReader R = cmd.ExecuteReader();

                    if (R.HasRows)
                    {
                        int i = 0;
                        while (R.Read())
                        {
                            i = i + 1;

                            patients.Add(
                                new Patient
                                {
                                    TDate = DateTime.Parse(R["TDate"].ToString()).ToString("yyyy-MM-dd tt hh:mm"),
                                    ParmName = R["ParmName"].ToString(),
                                    Plocation = R["Plocation"].ToString(),
                                    HoName = R["HoName"].ToString(),
                                    Holocation = R["Holocation"].ToString(),
                                    Medicine = R["Medicine"].ToString(),
                                    getdate = DateTime.Parse(R["Getdate"].ToString()).ToString("yyyy-MM-dd tt hh:mm")
                                });
                        }
                        ScriptCount = "총 " + i.ToString() + "건";
                    }
                    else
                    {
                        ScriptCount = "데이터가 없습니다";
                    }

                    R.Close();
                }
                catch (Exception ex)
                {
                    ScriptCount = ex.Message;
                }
            }
        }
    }
}
