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
        public DelegateCommand InputCommand { get; set; }
        //public DelegateCommand<string> ControlCommand { get; set; }
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


        private int hosNum = Commons.DEV_ID;
        public int HosNum
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

        private string[] mList = { "", "A", "B", "C"};

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
           // ControlCommand = new DelegateCommand<string>(Controls);
            ResetsCommand = new DelegateCommand(Resetting);
            LoginCommand = new DelegateCommand<string>(LogOut);
            InfoCommand = new DelegateCommand<string>(Infos);
            InputCommand = new DelegateCommand(InputV);
        }

        private void InputV()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Medicine))
            {
                MessageBox.Show("정보를 입력해주세요");
                return;
            }

            string strQuery = " INSERT INTO prescription " +
                              " (HospitalNum, " +
                              " Name, " +
                              " Date, " +
                              " Medicine) " +
                              " VALUES " +
                              " (@HospitalNum, " +
                              " @Name, " +
                              " @Datee," +
                              " @Medicine); ";


            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    MySqlParameter paramHospitalNum = new MySqlParameter("@HospitalNum", MySqlDbType.Int32);
                    paramHospitalNum.Value = Commons.DEV_ID;
                    cmd.Parameters.Add(paramHospitalNum);

                    MySqlParameter paramName = new MySqlParameter("@Name", MySqlDbType.VarChar);
                    paramName.Value = Name;
                    cmd.Parameters.Add(paramName);

                    MySqlParameter paramMedicine = new MySqlParameter("@Medicine", MySqlDbType.VarChar);
                    paramMedicine.Value = Medicine;
                    cmd.Parameters.Add(paramMedicine);

                    MySqlParameter paramDatee = new MySqlParameter("@Datee", MySqlDbType.VarChar);
                    paramDatee.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add(paramDatee);
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                return;
            }
            Name = null;
            MessageBox.Show("입력되었습니다.");
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

        //private void Controls(string uri)
        //{
        //    _regionManager.RequestNavigate("Hello", uri);

        //    _regionManager.Regions["Menu"].RemoveAll();
        //}

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
                              " Where HospitalNum = @HospitalNum AND ";
            string _MM = "  Medicine = @Medicine ";
            string _Datee = " Date = @Datee ";
            string _Name = " Name = @Name ";

            if (string.IsNullOrEmpty(Datee) && string.IsNullOrEmpty(Medicine) &&
               string.IsNullOrEmpty(Name))
            {

            }


            else if (string.IsNullOrEmpty(Datee) && string.IsNullOrEmpty(Medicine))
            {
                strQuery += ( _Name);
            }

            else if (string.IsNullOrEmpty(Datee) && string.IsNullOrEmpty(Name))
            {
                strQuery += ( _MM);
            }

            else if (string.IsNullOrEmpty(Medicine) && string.IsNullOrEmpty(Name))
            {
                strQuery += (_Datee);
            }
                        

            else if (string.IsNullOrEmpty(Datee))
            {
                strQuery += (_Name + "AND" + _MM);
            }

            else if (string.IsNullOrEmpty(Name))
            {
                strQuery += (_MM + "AND" + _Datee);
            }
                        

            else if (string.IsNullOrEmpty(Medicine))
            {
                strQuery += ( _Name + "AND" + _Datee);
            }
              

            else
            {
                strQuery += (_Datee + _Name + _MM);
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    MySqlParameter paramHospitalNum = new MySqlParameter("@HospitalNum", MySqlDbType.Int32);
                    paramHospitalNum.Value = Commons.DEV_ID;
                    cmd.Parameters.Add(paramHospitalNum);


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
