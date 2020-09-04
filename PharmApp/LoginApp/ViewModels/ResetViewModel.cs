using LoginApp.Models;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LoginApp.ViewModels
{
    public class ResetViewModel : BindableBase
    {
        IRegionManager _regionManager;

        
        public DelegateCommand<string> ResetCommand { get; set; }

        string strConnString = "Server=localhost;Port=3306;" +
                               "Database=gguklist;Uid=root;Pwd=mysql_p@ssw0rd;";

        int Id = Commons.Dev_Id;

        private string pwd;
        public string Pwd
        {
            get => pwd;
            set => SetProperty(ref pwd, value);
        }

        private string ckPwd;

        public string CkPwd
        {
            get => ckPwd;
            set => SetProperty(ref ckPwd, value);
        }


        public ResetViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            ResetCommand = new DelegateCommand<string>(Reset);
        }

        private void Reset(string uri)
        {
            if(Pwd != CkPwd)
            {
                MessageBox.Show("비밀번호를 다시 입력해주세요");
                return;
            }

            else
            {
                string strQuery = " UPDATE plist " +
                               " SET " +
                               " PW = @Pwd, " +
                               " CK = 1" +
                               " WHERE " +
                               " DEV_ID = @Id ";

                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {

                    conn.Open();
                    //MetroMessageBox.Show(this, $"DB접속성공!!");
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;
                    MySqlParameter paramPwd = new MySqlParameter("@Pwd", MySqlDbType.VarChar);
                    string PW = Pwd;
                    var md5Hash = MD5.Create();
                    var cryptoPassword = Commons.GetMd5Hash(md5Hash, Pwd);
                    paramPwd.Value = cryptoPassword;

                    MySqlParameter paramid = new MySqlParameter("@Id", MySqlDbType.Int32);
                    paramid.Value = Id;

                    cmd.Parameters.Add(paramid);
                    cmd.Parameters.Add(paramPwd);
                    cmd.ExecuteNonQuery();
                }

                _regionManager.RequestNavigate("ContentRegion", uri);
                _regionManager.Regions["Find"].RemoveAll();
                _regionManager.Regions["Reset"].RemoveAll();
                _regionManager.Regions["Menu"].RemoveAll();
            }
        }

        private void upCK()
        {
            string strQuery = " UPDATE plist " +
                              " SET " +
                              " PW = @Pwd " +
                              " WHERE " +
                              " DEV_ID = @Id ";

            using (MySqlConnection conn = new MySqlConnection(strConnString))
            {

                conn.Open();
                //MetroMessageBox.Show(this, $"DB접속성공!!");
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = strQuery;
            }
        }
    }
}
