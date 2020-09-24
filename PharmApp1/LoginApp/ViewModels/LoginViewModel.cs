using LoginApp.Models;
using MySql.Data.MySqlClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
//using LoginApp.Models;
using System;
using System.Collections.Generic;
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

        public static List<InfoModel> Infoo;

        private string id;
        private string pw;
        int CK;

        string strConnString = "Server=localhost;Port=3306;" +
                               "Database=gguklist;Uid=root;Pwd=mysql_p@ssw0rd;";


        List<InfoModel> psList;
        public List<InfoModel> PsList
        {
            get => psList;
            set => SetProperty(ref psList, value);
        }

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

        private int dev_id;
        public int DEV_ID
        {
            get => dev_id;
            set => SetProperty(ref dev_id, value);
        }

        private int postnum;
        public int POSTNUM
        {
            get => postnum;
            set => SetProperty(ref postnum, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string addr;
        public string ADDR
        {
            get => addr;
            set => SetProperty(ref addr, value);
        }

        private string sido;
        public string SIDO
        {
            get => sido;
            set => SetProperty(ref sido, value);
        }

        private string sigugun;
        public string SIGUGUN
        {
            get => sigugun;
            set => SetProperty(ref sigugun, value);
        }

        private string contact;
        public string CONTACT
        {
            get => contact;
            set => SetProperty(ref contact, value);
        }

        private float px;
        public float PX
        {
            get => px;
            set => SetProperty(ref px, value);
        }

        private float py;
        public float PY
        {
            get => py;
            set => SetProperty(ref py, value);
        }

        private string pName;
        public string PName
        {
            get => pName;
            set => SetProperty(ref pName, value);
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
            _regionManager.RequestNavigate("ContentRegion", uri);

            _regionManager.Regions["Find"].RemoveAll();
            _regionManager.Regions["Reset"].RemoveAll();
        }

        //private void Join(string uri)
        //{
        //    _regionManager.RequestNavigate("Menu", uri);

        //    _regionManager.Regions["ContentRegion"].RemoveAll();
        //}

        private void Navigate(string uri)
        {
            //PW = uri;
            //MessageBox.Show($"{Id} {PW}");
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
                        PsList = new List<InfoModel>();
                        SelectItem();
                        
                        DEV_ID = PsList[0].DEV_ID;
                        Name = PsList[0].Name;
                        SIDO = PsList[0].SIDO;
                        SIGUGUN = PsList[0].SIGUGUN;
                        POSTNUM = psList[0].POSTNUM;
                        ADDR = psList[0].ADDR;
                        CONTACT = psList[0].CONTACT;
                        PX = psList[0].PX;
                        PY = psList[0].PY;
                        
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

        private void SelectItem()
        {
            string strQuery = " SELECT DEV_ID, " +
                              " Name, " +
                              " SIDO, " +
                              " SIGUGUN, " +
                              " POSTNUM, " +
                              " ADDR, " +
                              " CONTACT, " +
                              " PX, " + " PY " +
                              " FROM plist " +
                              " WHERE DEV_ID = @DEV_ID";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    MySqlParameter DEV_ID = new MySqlParameter("@DEV_ID", MySqlDbType.VarChar);
                    DEV_ID.Value = Commons.Dev_Id;
                    cmd.Parameters.Add(DEV_ID);



                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var temp = new InfoModel
                        {
                            DEV_ID = int.Parse(reader["DEV_ID"].ToString()),
                            Name = reader["Name"].ToString(),
                            SIDO = reader["SIDO"].ToString(),
                            SIGUGUN = reader["SIGUGUN"].ToString(),
                            POSTNUM = int.Parse(reader["POSTNUM"].ToString()),
                            ADDR = reader["ADDR"].ToString(),
                            CONTACT = reader["CONTACT"].ToString(),
                            PX = float.Parse(reader["PX"].ToString()),
                            PY = float.Parse(reader["PY"].ToString())
                        };
                        PsList.Add(temp);
                    }
                    Infoo = PsList; 
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }


        }


    }
    
}
