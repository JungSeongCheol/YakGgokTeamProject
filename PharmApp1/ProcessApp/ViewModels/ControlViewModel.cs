using LoginApp.Models;
using MySql.Data.MySqlClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProcessApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProcessApp.ViewModels
{
    public class ControlViewModel:BindableBase
    {
        IRegionManager _regionManager;

        string strConnString = "Server=localhost;Port=3306;" +
                               "Database=medicine;Uid=root;Pwd=mysql_p@ssw0rd;";

        List<ControlModel> psList;
        public List<ControlModel> PsList
        {
            get => psList;
            set => SetProperty(ref psList, value);
        }

        private int aM;

        public int AM
        {
            get => aM;
            set => SetProperty(ref aM, value);
        }

        private int bM;

        public int BM
        {
            get => bM;
            set => SetProperty(ref bM, value);
        }

        private int cM;

        public int CM
        {
            get => cM;
            set => SetProperty(ref cM, value);
        }

        public DelegateCommand APlusCommand { get; set; }
        public DelegateCommand BPlusCommand { get; set; }
        public DelegateCommand CPlusCommand { get; set; }
        public DelegateCommand AMinusCommand { get; set; }
        public DelegateCommand BMinusCommand { get; set; }
        public DelegateCommand CMinusCommand { get; set; }
        public DelegateCommand UpCommand { get; set; }
        public DelegateCommand<string> LoginCommand { get; set; }
        //public DelegateCommand OutPutCommand { get; set; }
        public DelegateCommand<string> BackCommand { get; set; }

        public ControlViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            PsList = new List<ControlModel>();
            selItems();
            AM = PsList[0].A;
            BM = PsList[0].B;
            CM = PsList[0].C;

            APlusCommand = new DelegateCommand(AP);
            BPlusCommand = new DelegateCommand(BP);
            CPlusCommand = new DelegateCommand(CP);
            AMinusCommand = new DelegateCommand(AMi);
            BMinusCommand = new DelegateCommand(BMi);
            CMinusCommand = new DelegateCommand(CMi);
            UpCommand = new DelegateCommand(UpdateItems);
            LoginCommand = new DelegateCommand<string>(LogOut);
            //OutPutCommand = new DelegateCommand(Outputs);
            BackCommand = new DelegateCommand<string>(Back);
        }

        private void Outputs()
        {
            //PsList = new List<ControlModel>();
            //selItems(); 
        }

        private void LogOut(string uri)
        {
            _regionManager.RequestNavigate("ContentRegion", uri);

            _regionManager.Regions["Hello"].RemoveAll();
        }

        private void CMi()
        {
            if (CM >= 0) CM--;
        }

        private void BMi()
        {
            if (BM >= 0) BM--;
        }

        private void AMi()
        {
            if (AM >= 0) AM--;

        }

        private void CP()
        {
            CM++;
        }

        private void BP()
        {
            BM++;
        }

        private void AP()
        {
            AM++;
        }

        private void selItems()
        {
            string strQuery = "SELECT A, B, C " +
                              " FROM mminfotbl " +
                              " Where PharmNum = @Id";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.VarChar);
                    paramId.Value = Commons.Dev_Id;
                    cmd.Parameters.Add(paramId);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var temp = new ControlModel
                        {
                            //PharmNum = int.Parse(reader["PharmNum"].ToString()),
                            A = int.Parse(reader["A"].ToString()),
                            B = int.Parse(reader["B"].ToString()),
                            C = int.Parse(reader["C"].ToString())

                        };
                        PsList.Add(temp);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Back(string uri)
        {
            _regionManager.RequestNavigate("Menu", uri);

            _regionManager.Regions["Hello"].RemoveAll();
        }

        private void UpdateItems()
        {
            string strQuery = " UPDATE mminfotbl " +
                              " SET " +
                              " A = @A , " +
                              " B = @B , " +
                              " C = @C " +
                              " WHERE PharmNum = @Id; ";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    MySqlParameter paramA = new MySqlParameter("@A", MySqlDbType.VarChar);
                    paramA.Value = AM;
                    cmd.Parameters.Add(paramA);

                    MySqlParameter paramB = new MySqlParameter("@B", MySqlDbType.VarChar);
                    paramB.Value = BM;
                    cmd.Parameters.Add(paramB);

                    MySqlParameter paramC = new MySqlParameter("@C", MySqlDbType.VarChar);
                    paramC.Value = CM;
                    cmd.Parameters.Add(paramC);

                    MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.VarChar);
                    paramId.Value = Commons.Dev_Id;
                    cmd.Parameters.Add(paramId);

                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            MessageBox.Show("입력되었습니다.");
        }
    }
}
