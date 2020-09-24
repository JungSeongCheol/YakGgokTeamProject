using LoginApp.Models;
using MySql.Data.MySqlClient;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using ProcessApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



namespace ProcessApp.ViewModels 
{
    public class MenuViewModel : BindableBase
    {
        public DelegateCommand OutPutCommand { get; set; }
        public DelegateCommand<string> ControlCommand { get; set; }
        public DelegateCommand ResetsCommand { get; set; }
        public DelegateCommand<string> LoginCommand { get; set; }
        public DelegateCommand<string> InfoCommand { get; set; }
        public DelegateCommand GOCommand { get; set; }
        private IRegionManager _regionManager;
        //private int Sitem;
        public static int SIndex;
        public static List<PrescriptionModel> Ps;

        private int sitem;
        public int SItem
        {
            get => sitem;
            set => SetProperty(ref sitem, value);
        }

        List<PrescriptionModel> psList;
        public List<PrescriptionModel> PsList
        {
            get => psList;
            set => SetProperty(ref psList, value);
        }


        private string hosNum = "";
        public string HosNum
        {
            get => hosNum;
            set => SetProperty(ref hosNum, value);
        }
        private string patientId = null;
        public string PatientId
        {
            get => patientId;
            set => SetProperty(ref patientId, value);
        }

        private string medicine;
        public string Medicine
        {
            get => medicine;
            set => SetProperty(ref medicine, value);
        }

        private string date = null;

        public string Datee
        {
            get => date;
            set => SetProperty(ref date, value);
        }
                
        //private bool v = false;

            //public bool V
            //{
            //    get => v;
            //    set => SetProperty(ref v, value);
            //}

        private string[] mList = {"","A","B","C","D" };

        public string[] MList
        {
            get => mList;
            set => SetProperty(ref mList, value);
        }

        string strConnString = "Server=localhost;Port=3306;" +
                               "Database=gguklist;Uid=root;Pwd=mysql_p@ssw0rd;";


        public MenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            
            OutPutCommand = new DelegateCommand(Navigate);
            ControlCommand = new DelegateCommand<string>(Controls);
            ResetsCommand = new DelegateCommand(Resetting);
            LoginCommand = new DelegateCommand<string>(LogOut);
            InfoCommand = new DelegateCommand<string>(Infos);
            GOCommand = new DelegateCommand(GOS);
        }

        private void GOS()
        {
            Ps = new List<PrescriptionModel>();
            Ps = PsList;
            SIndex = SItem;
            _regionManager.RequestNavigate("Hello", "PrescriptionView");
            _regionManager.Regions["Menu"].RemoveAll();
        }

        private void Infos(string uri)
        {
            _regionManager.RequestNavigate("Hello", uri);

            _regionManager.Regions["Menu"].RemoveAll();
        }

        private void LogOut(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);

            _regionManager.Regions["Menu"].RemoveAll();
            _regionManager.Regions["Hello"].RemoveAll();
        }

        private void Controls(string uri)
        {
            _regionManager.RequestNavigate("Hello", uri);

            _regionManager.Regions["Menu"].RemoveAll();
        }

        private void Resetting()
        {
            PatientId = Datee = null;
            Medicine = MList[0];
        }

        private void Navigate()
        {
            //SendPara();
            PsList = new List<PrescriptionModel>();
            SelectItem();

            
        }


        private void SelectItem()
        {
            string strQuery = " SELECT * " + " FROM prescribeinfo " +
                              " where PharmNum = @PharmNum ";
            //string where  = " Where ";
            string _Hos = " HospitalNum = @HospitalNum ";
           // string _MM = "  Medicine = @Medicine ";
            string _Datee = " TDate Like @TDatee ";
            string _PatientId = " PatientId = @PatientId ";


            if (string.IsNullOrEmpty(Datee) && string.IsNullOrEmpty(PatientId) && string.IsNullOrEmpty(HosNum))
            {

            }


            else
            {
                strQuery += " AND ";
                if (string.IsNullOrEmpty(Datee)  && string.IsNullOrEmpty(HosNum))
                {
                    strQuery += (_PatientId);
                }

                
                else if (string.IsNullOrEmpty(PatientId) && string.IsNullOrEmpty(HosNum))
                {
                    strQuery += (_Datee);
                }

               
                else if (string.IsNullOrEmpty(HosNum))
                {
                    strQuery += (_PatientId + " AND " + _Hos); 
                }

                else if (string.IsNullOrEmpty(Datee))
                {
                    strQuery += (_PatientId + " AND " + _Hos);
                }

                else if (string.IsNullOrEmpty(PatientId))
                {
                    strQuery += (_Hos + " AND " + _Datee);
                }

                else
                {
                    strQuery += (_Datee + " AND " + _PatientId  + " AND " + _Hos);
                }
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    MySqlParameter paramPharmNum = new MySqlParameter("@PharmNum", MySqlDbType.VarChar);
                    paramPharmNum.Value = Commons.Dev_Id;
                    cmd.Parameters.Add(paramPharmNum);

                    if (!string.IsNullOrEmpty(PatientId))
                    {
                        MySqlParameter paramName = new MySqlParameter("@PatientId", MySqlDbType.VarChar);
                        paramName.Value = PatientId;
                        cmd.Parameters.Add(paramName);
                    }

                    //if (!string.IsNullOrEmpty(Medicine))
                    //{
                    //    MySqlParameter paramMedicine = new MySqlParameter("@Medicine", MySqlDbType.VarChar);
                    //    paramMedicine.Value = Medicine;
                    //    cmd.Parameters.Add(paramMedicine);
                    //}

                    if (!string.IsNullOrEmpty(HosNum))
                    {
                        MySqlParameter paramHos = new MySqlParameter("@HospitalNum", MySqlDbType.Int32);
                        paramHos.Value = int.Parse(HosNum);
                        cmd.Parameters.Add(paramHos);
                    }

                    if (!string.IsNullOrEmpty(Datee))
                    {
                        MySqlParameter paramDatee = new MySqlParameter("@TDatee", MySqlDbType.VarChar);
                        paramDatee.Value = DateTime.Parse(Datee).ToString("yyyy-MM-dd") + "%";
                        cmd.Parameters.Add(paramDatee);
                    }

          
                    //MySqlDataAdapter adapter = new MySqlDataAdapter(strQuery, conn);


                    MySqlDataReader reader = cmd.ExecuteReader();
                    int cnt = 0;
                    while (reader.Read())
                    {
                        var temp = new PrescriptionModel
                        {
                            IdSub = int.Parse(reader["IdSub"].ToString()),
                            PatientId = reader["PatientId"].ToString(),
                            HospitalNum = int.Parse(reader["HospitalNum"].ToString()),
                            PharmNum = int.Parse(reader["PharmNum"].ToString()),
                            A = int.Parse(reader["A"].ToString()),
                            B = int.Parse(reader["B"].ToString()),
                            C = int.Parse(reader["C"].ToString()),
                            TDate = reader["TDate"].ToString(),
                            getDate = reader["getDate"].ToString(),
                            HoName = reader["HoName"].ToString(),
                            Holocation = reader["Holocation"].ToString(),
                            PharmName = reader["PharmName"].ToString(),
                            Plocation = reader["Plocation"].ToString(),
                            PName = reader["PName"].ToString(),
                            Medicine = " A : " + reader["A"].ToString() + "  B : " + reader["B"].ToString() +
                                       " C : " + reader["C"].ToString()
                        };
                        PsList.Add(temp);
                        Medicine = psList[cnt++].Medicine.ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }


        }

    }
}
