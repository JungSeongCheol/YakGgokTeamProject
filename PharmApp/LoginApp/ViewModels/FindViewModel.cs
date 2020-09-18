using LoginApp.Models;
using MySql.Data.MySqlClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LoginApp.ViewModels
{
    class FindViewModel:BindableBase
    {
        public DelegateCommand<string> ResetCommand { get; set; }
        public DelegateCommand CKCommand { get; set; }

        int CHK;
        IRegionManager _regionManager;

        
        string strConnString = "Server=localhost;Port=3306;" +
                               "Database=gguklist;Uid=root;Pwd=mysql_p@ssw0rd;";
        public static string UserId;

        private int id;
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private string name;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string sigugun;

        public string SIGUGUN
        {
            get => sigugun;
            set => SetProperty(ref sigugun, value);
        }

        int CK = 0;

        public FindViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CKCommand = new DelegateCommand(CHECK_ID);
            ResetCommand = new DelegateCommand<string>(Reset);
        }

        private void Reset(string uri)
        {
            if (isOk())
            {
                Commons.Dev_Id = Id;
                _regionManager.RequestNavigate("Reset", uri);
                
            }
        }

        private bool isOk()
        {
            if (CK == 1)
            {
               
                string strQuery = " SELECT PW " +
                               " FROM plist " +
                               " WHERE DEV_ID = @Id " +
                               " AND SIGUGUN = @SIGUGUN " +
                               " AND Name = @Name; ";

                try
                {
                    using (MySqlConnection conn = new MySqlConnection(strConnString))
                    {

                        conn.Open();
                        //MetroMessageBox.Show(this, $"DB접속성공!!");
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = strQuery;
                        MySqlParameter paramUserId = new MySqlParameter("@Id", MySqlDbType.Int32);
                        paramUserId.Value = Id; // 공백 넣는 경우가 아주 많기때문에                    
                        cmd.Parameters.Add(paramUserId);
                        MySqlParameter paramSIGUGUN = new MySqlParameter("@SIGUGUN", MySqlDbType.VarChar);
                        paramSIGUGUN.Value = SIGUGUN;
                        cmd.Parameters.Add(paramSIGUGUN);
                        MySqlParameter paramName = new MySqlParameter("@Name", MySqlDbType.VarChar);
                        paramName.Value = Name;
                        cmd.Parameters.Add(paramName);

                        //MySqlDataReader reader = cmd.ExecuteReader();
                        //reader.Read();

                        //cmd.ExecuteNonQuery();
                        string PW = null;
                        var tmp = cmd.ExecuteScalar();

                        if (tmp != null )
                        {
                            PW = tmp.ToString();
                            //Commons.Dev_Id = Id;
                            return true;
                        } else
                        {
                            MessageBox.Show("없는 정보입니다.");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }

            else
            {
                MessageBox.Show("없는 아이디입니다.");
                return false;
            }
        }

        private void CHECK_ID()
        {
            CKFirst();
            string strQuery = "SELECT DEV_Id FROM  plist " + //반드시 뒤에 SpaceBar를 넣어줘야됨(안넣으면 userTBLWHERE로 붙어져서 Syntax Error나옴)
                                      " WHERE DEV_ID = @Id " +    //@userID로 사용안하고 직접적 ID(TxtUserID)를 바로 넣으면 SQL Injection으로 해킹위험이 나옴
                                      " AND Name = @Name ";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {
                    conn.Open();
                    //MetroMessageBox.Show(this, $"DB접속성공!!");
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;
                    MySqlParameter paramUserId = new MySqlParameter("@Id", MySqlDbType.Int32);
                    cmd.Parameters.Add(paramUserId);
                    paramUserId.Value = Id;
                    

                    MySqlParameter paramName = new MySqlParameter("@Name", MySqlDbType.VarChar);
                    paramName.Value = Name;
                    cmd.Parameters.Add(paramName);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows && CHK == 1)
                    {
                        CK = 1;
                        MessageBox.Show("등록정보가 있습니다.");
                    }
                    else
                    {
                        MessageBox.Show("첫 로그인을 안했거나 없는 정보 입니다.");
                        SIGUGUN = Name = null;
                        Id = -1;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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

                    if (tmp.ToString() == "0")
                    {
                        CHK = 0;

                    }
                    else
                    {
                        CHK = 1;
                    }
                }
            }

            catch (Exception e)
            {

            }
        }
    }
}
