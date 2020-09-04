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
        private IRegionManager _regionManager;

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
        private string name = null;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string medicine = null;
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
                               "Database=hospital;Uid=root;Pwd=mysql_p@ssw0rd;";


        public MenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            
            OutPutCommand = new DelegateCommand(Navigate);
            ControlCommand = new DelegateCommand<string>(Controls);
            ResetsCommand = new DelegateCommand(Resetting);
            LoginCommand = new DelegateCommand<string>(LogOut);
            InfoCommand = new DelegateCommand<string>(Infos);
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
            Name = Datee = null;
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
            string strQuery = " SELECT * " + " FROM prescription " +
                              " Where ";
            string _Hos = " HospitalNum = @HospitalNum ";
            string _MM = "  Medicine = @Medicine ";
            string _Datee = " Date = @Datee ";
            string _Name = " Name = @Name ";

            if (string.IsNullOrEmpty(Datee) && string.IsNullOrEmpty(Medicine) &&
               string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(HosNum))
            {

            }


            else if (string.IsNullOrEmpty(Datee) && string.IsNullOrEmpty(Medicine) && string.IsNullOrEmpty(HosNum))
            {
                strQuery += ("AND" + _Name);
            }

            else if (string.IsNullOrEmpty(Datee) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(HosNum))
            {
                strQuery += ("AND" + _MM);
            }

            else if (string.IsNullOrEmpty(Medicine) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(HosNum))
            {
                strQuery += ("AND" + _Datee);
            }

            else if (string.IsNullOrEmpty(Medicine) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(_Datee))
            {
                strQuery += ("AND" + _Hos);
            }

            else if (string.IsNullOrEmpty(Datee) && string.IsNullOrEmpty(HosNum))
            {
                strQuery += ("AND" + _Name + "AND" + _MM);
            }

            else if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(HosNum))
            {
                strQuery += ("AND" + _MM + "AND" + _Datee);
            }

            else if (string.IsNullOrEmpty(Medicine) && string.IsNullOrEmpty(Datee))
            {
                strQuery += ("AND" + _Name + "AND" + _Hos);
            }

            else if (string.IsNullOrEmpty(Datee) && string.IsNullOrEmpty(Medicine))
            {
                strQuery += ("AND" + _Name + "AND" + _Hos);
            }

            else if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Datee))
            {
                strQuery += ("AND" + _MM + "AND" + _Hos);
            }

            else if (string.IsNullOrEmpty(Medicine) && string.IsNullOrEmpty(Name))
            {
                strQuery += ("AND" + _Datee + "AND" + _Hos);
            }

            else if (string.IsNullOrEmpty(HosNum))
            {
                strQuery += ("AND" + _Name + "AND" + _Hos + "AND" + _MM);
            }

            else if (string.IsNullOrEmpty(Datee))
            {
                strQuery += ("AND" + _Name + "AND" + _Hos + "AND" + _MM);
            }

            else if (string.IsNullOrEmpty(Name))
            {
                strQuery += ("AND" + _MM + "AND" + _Hos + "AND" + _Datee);
            }

            else if (string.IsNullOrEmpty(Medicine))
            {
                strQuery += ("AND" + _Datee + "AND" + _Hos + "AND" + _MM);
            }

            else
            {
                strQuery += (_Datee + _Name + _MM + _Hos);
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    if (!string.IsNullOrEmpty(Name))
                    {
                        MySqlParameter paramHospitalNum = new MySqlParameter("@HospitalNum", MySqlDbType.VarChar);
                        paramHospitalNum.Value = Commons.Dev_Id;
                        cmd.Parameters.Add(paramHospitalNum);

                    }
                        


                    if (!string.IsNullOrEmpty(Name))
                    {
                        MySqlParameter paramName = new MySqlParameter("@Name", MySqlDbType.VarChar);
                        paramName.Value = Name;
                        cmd.Parameters.Add(paramName);
                    }

                    if (!string.IsNullOrEmpty(Medicine))
                    {
                        MySqlParameter paramMedicine = new MySqlParameter("@Medicine", MySqlDbType.VarChar);
                        paramMedicine.Value = Medicine;
                        cmd.Parameters.Add(paramMedicine);
                    }

                    if (!string.IsNullOrEmpty(HosNum))
                    {
                        MySqlParameter paramHos = new MySqlParameter("@HospitalNum", MySqlDbType.VarChar);
                        paramHos.Value = HosNum;
                        cmd.Parameters.Add(paramHos);
                    }

                    if (!string.IsNullOrEmpty(Datee))
                    {
                        MySqlParameter paramDatee = new MySqlParameter("@Datee", MySqlDbType.VarChar);
                        paramDatee.Value = Datee;
                        cmd.Parameters.Add(paramDatee);
                    }

          
                    //MySqlDataAdapter adapter = new MySqlDataAdapter(strQuery, conn);


                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var temp = new PrescriptionModel
                        {
                            PreNum = int.Parse(reader["PreNum"].ToString()),
                            HospitalNum = int.Parse(reader["HospitalNum"].ToString()),
                            Name = reader["Name"].ToString(),
                            Date = reader["Date"].ToString(),
                            Medicine = reader["Medicine"].ToString()
                        };
                        PsList.Add(temp);
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
