using LoginApp.Models;
using MySql.Data.MySqlClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Security.Cryptography;
using System.Windows;

namespace LoginApp.ViewModels
{
    public interface IHavePassword
    { 
    
    }
    public class LoginViewModel : BindableBase
    {
        public DelegateCommand<string> NavigateCommand { get; set; }
        //public DelegateCommand<string> ShowViewCommand { get; set; }
        public DelegateCommand<string> FindCommand { get; set; }

        private string id;
        private string pw;
        int CK;

        string strConnString = "Server=localhost;Port=3306;" +
                               "Database=gguklist;Uid=root;Pwd=mysql_p@ssw0rd;";

        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string PW
        {
            get => pw;
            set => SetProperty(ref pw, value);
        }

        private IRegionManager _regionManager;

        public LoginViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            //ShowViewCommand = new DelegateCommand<string>(Join);
            FindCommand = new DelegateCommand<string>(Find);

            Commons.Dev_Id = -1;
        }


        private void Find(string uri)
        {
            _regionManager.RequestNavigate("Find", uri);

            _regionManager.Regions["ContentRegion"].RemoveAll();
        }

        //private void Join(string uri)
        //{
        //    _regionManager.RequestNavigate("Menu", uri);

        //    _regionManager.Regions["ContentRegion"].RemoveAll();
        //}

        private void Navigate(string uri)
        {
            PW = uri;
            MessageBox.Show($"{Id} {PW}");
            if (LoginOK() == true)
            {
                Commons.CK = CK;
                if (CK == 1) _regionManager.RequestNavigate("Menu", "MenuView");
                else _regionManager.RequestNavigate("Menu", "ResetView");
                _regionManager.Regions["ContentRegion"].RemoveAll();
            }
        }

        private void CKFirst()
        {
            string strQuery = " SELECT CK " +
                               " FROM plist " +
                               " where DEV_ID = @Id";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {

                    conn.Open();
                    //MetroMessageBox.Show(this, $"DB접속성공!!");
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.VarChar);
                    paramId.Value = Id;
                    cmd.Parameters.Add(paramId);

                    var tmp = cmd.ExecuteScalar();

                    if(tmp.ToString() == "0")
                    {
                        CK = 0;
                        
                    }
                    else
                    {
                        CK = 1;    
                    }
                }
            }

            catch (Exception e)
            {

            }
        }

        private bool LoginOK()
        {
            CKFirst();
            //Id = PW = null;
            string strQuery = " SELECT " +
                              " Dev_Id, " +
                              " PW " +
                              " FROM plist " +
                              " Where Dev_Id = @Id " +
                              " AND " +
                              " PW = @Pwd; ";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.VarChar);
                    paramId.Value = Id;
                    cmd.Parameters.Add(paramId);

                    MySqlParameter paramPW = new MySqlParameter("@Pwd", MySqlDbType.VarChar);


                    if (CK == 1)
                    {
                        string PP = PW;
                        var md5Hash = MD5.Create();
                        var cryptoPassword = Commons.GetMd5Hash(md5Hash, PW);
                        paramPW.Value = cryptoPassword;

                    }

                    else
                    {
                        paramPW.Value = PW;
                    }
                    
                    cmd.Parameters.Add(paramPW);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!reader.HasRows)
                    {
                        PW = Id = null;
                        MessageBox.Show("로그인실패");
                        return false;
                    }

                    else
                    {
                        Commons.Dev_Id = int.Parse(Id);
                        PW = Id = null;
                        MessageBox.Show("로그인성공");
                        
                        return true;

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return false;
            }

        }

 
    }
    
}
