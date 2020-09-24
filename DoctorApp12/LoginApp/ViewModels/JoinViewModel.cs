using LoginApp.Models;
using MySql.Data.MySqlClient;
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
    public class JoinViewModel : BindableBase
    {
        IRegionManager _regionManager;

        public DelegateCommand CheckCommand { get; set; }
        public DelegateCommand<string> JoinCommand { get; set; }

        public JoinViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            JoinCommand = new DelegateCommand<string>(Join);
            CheckCommand = new DelegateCommand(Chk);

        }

        #region 선언

        string strConnString = "Server=localhost;Port=3306;" +
                               "Database=member;Uid=root;Pwd=mysql_p@ssw0rd;";

        private string id;
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

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

        private string name;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private int doctorId;

        public int DoctorId
        {
            get => doctorId;
            set => SetProperty(ref doctorId, value);
        }

        private int hospitalNum;

        public int HospitalNum
        {
            get => hospitalNum;
            set => SetProperty(ref hospitalNum, value);
        }

        private string ck;

        public string CK
        {
            get => ck;
            set => SetProperty(ref ck, value);
        }

        private bool pwdFocus = false;

        public bool PwdFocus
        {
            get => pwdFocus;
            set => SetProperty(ref pwdFocus, value);
        }

        private bool IdFocus = false;

        public bool idFocus
        {
            get => pwdFocus;
            set => SetProperty(ref IdFocus, value);
        }
        #endregion

       

        int Check = 0;

        private void Chk()
        {
            if (string.IsNullOrEmpty(Id))
            {
                MessageBox.Show("아이디를 입력해주세요");
                return;
            }
            string strQuery = "SELECT Id FROM dmemtbl " + //반드시 뒤에 SpaceBar를 넣어줘야됨(안넣으면 userTBLWHERE로 붙어져서 Syntax Error나옴)
                                      " WHERE Id = @Id ";    //@userID로 사용안하고 직접적 ID(TxtUserID)를 바로 넣으면 SQL Injection으로 해킹위험이 나옴

            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {
                    conn.Open();
                    //MetroMessageBox.Show(this, $"DB접속성공!!");
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;
                    MySqlParameter paramUserId = new MySqlParameter("@Id", MySqlDbType.VarChar, 45);
                    cmd.Parameters.Add(paramUserId);
                    paramUserId.Value = Id;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows)
                    {
                        MessageBox.Show("이미 등록아이디가 있습니다.");
                        Id = null;
                        IdFocus = true;
                    }
                    else
                    {
                        MessageBox.Show("사용 가능한 아이디 입니다.");
                        Check = 1;
                        PwdFocus = true;
                        
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        


        private void Join(string uri)
        {

            if (InsertToDB()) {
                _regionManager.RequestNavigate("ContentRegion", uri);
                _regionManager.Regions["Menu"].RemoveAll();
            } 
        }

        private bool InsertToDB()
        {
            string strQuery = " INSERT INTO dmemtbl " +
                              " (Id, " +
                              " Pwd, " +
                              " Name, " +
                              " HospitalNum, " +
                              " DoctorId) " +
                              " VALUES " +
                              " (@Id, " +
                              " @Pwd, " +
                              " @Name, " +
                              " @HospitalNum, " +
                              " @DoctorId); ";


            if (Pwd != CkPwd)
            {
                MessageBox.Show("비번이 같지 않습니다.");
                CkPwd = Pwd = null;
                CK = "비번 같지않음";
                return false;
            }

            else
            {
                CK = "비번 같음";
            }

            if (string.IsNullOrEmpty(Id)
                || string.IsNullOrEmpty(Pwd) || string.IsNullOrEmpty(CkPwd))
            {
                MessageBox.Show("아이디 비번을 채워주세요.");
                Id = Pwd = CkPwd = null;
                IdFocus = true;
                return false;
            }
                        
            if (string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("이름을 채워주세요.");
                return false;
            }

            if (HospitalNum == 0 || DoctorId == 0)
            {
                MessageBox.Show("정보를 다 입력해주세요");
                return false;
            }

            using (MySqlConnection conn = new MySqlConnection(strConnString))
            {
                conn.Open();
                //string PW;
                MySqlCommand cmd = new MySqlCommand(strQuery, conn);
                MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.VarChar);
                paramId.Value = Id;
                cmd.Parameters.Add(paramId);
                MySqlParameter paramPassword = new MySqlParameter("@Pwd", MySqlDbType.VarChar);
                var md5Hash = MD5.Create();
                var cryptoPassword = Commons.GetMd5Hash(md5Hash, Pwd);
                paramPassword.Value = cryptoPassword;
                cmd.Parameters.Add(paramPassword);
                MySqlParameter paramName = new MySqlParameter("@Name", MySqlDbType.VarChar);
                paramName.Value = Name;
                cmd.Parameters.Add(paramName);
                MySqlParameter paramHospitalNum = new MySqlParameter("@HospitalNum", MySqlDbType.Int32);
                paramHospitalNum.Value = HospitalNum;
                cmd.Parameters.Add(paramHospitalNum);

                MySqlParameter paramDoctorId = new MySqlParameter("@DoctorId", MySqlDbType.Int32);
                paramDoctorId.Value = DoctorId;
                cmd.Parameters.Add(paramDoctorId);

                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("가입되었습니다.");

            return true;


        }


    }
    
}
