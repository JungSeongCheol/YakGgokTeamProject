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
        public DelegateCommand<string> GOCommand { get; set; }
        private IRegionManager _regionManager;
        private int Sitem;
        public static int SIndex;
        public static List<PrescriptionModel> Ps;

        public int SItem
        {
            get => Sitem;
            set => SetProperty(ref Sitem, value);
        }

        private int sIdx;
        public int SIdx
        {
            get => sIdx;
            set => SetProperty(ref sIdx, value);
        }

        List<PrescriptionModel> psList;
        public List<PrescriptionModel> PsList
        {
            get => psList;
            set => SetProperty(ref psList, value);
        }

        private string medicine;
        public string Medicine
        {
            get => medicine;
            set => SetProperty(ref medicine, value);
        }

        private int hosNum = Commons.DEV_ID;
        public int HosNum
        {
            get => hosNum;
            set => SetProperty(ref hosNum, value);
        }

        private string mediNum;
        public string MediNum
        {
            get => mediNum;
            set => SetProperty(ref mediNum, value);
        }

        private string patientId = null;
        public string PatientId
        {
            get => patientId;
            set => SetProperty(ref patientId, value);
        }
               
        private string date = null;

        public string Datee
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        private string pName = null;

        public string PName
        {
            get => pName;
            set => SetProperty(ref pName, value);
        }

        private int am = 0;

        public int AM
        {
            get => am;
            set => SetProperty(ref am, value);
        }

        private int bm = 0;

        public int BM
        {
            get => bm;
            set => SetProperty(ref bm, value);
        }

        private int cm = 0;

        public int CM
        {
            get => cm;
            set => SetProperty(ref cm, value);
        }


        //private string[] mList = { "", "A", "B", "C"};

        //public string[] MList
        //{
        //    get => mList;
        //    set => SetProperty(ref mList, value);
        //}

        string strConnString = "Server=localhost;Port=3306;" +
                               "Database=member;Uid=root;Pwd=mysql_p@ssw0rd;";

        string strConnString1 = "Server=localhost;Port=3306;" +
                               "Database=gguklist;Uid=root;Pwd=mysql_p@ssw0rd;";

        string strConnString2 = "Server=localhost;Port=3306;" +
                               "Database=medicine;Uid=root;Pwd=mysql_p@ssw0rd;";

        public MenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            OutPutCommand = new DelegateCommand(Navigate);
            //ControlCommand = new DelegateCommand<string>(Controls);
            ResetsCommand = new DelegateCommand(Resetting);
            LoginCommand = new DelegateCommand<string>(LogOut);
            InfoCommand = new DelegateCommand<string>(Infos);
            InputCommand = new DelegateCommand(InputV);
            GOCommand = new DelegateCommand<string>(GOS);
        }

        private void GOS(string obj)
        {
            Ps = new List<PrescriptionModel>();
            Ps = PsList;
            SIndex = SIdx;
            _regionManager.RequestNavigate("Hello", "PrescriptionView");
            _regionManager.Regions["Menu"].RemoveAll();
        }

        private string sel()
        {
            string strQuery = " SELECT Name " + 
                              " FROM pmemtbl " +
                              " WHERE Id = @Id";


            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    MySqlParameter paramId = new MySqlParameter("@Id", MySqlDbType.VarChar);
                    paramId.Value = PatientId;
                    cmd.Parameters.Add(paramId);

                    var tmp = cmd.ExecuteScalar();
                    if (tmp == null) return null;
                    return tmp.ToString();
                
                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        private void InputV()
        {
            
            if (string.IsNullOrEmpty(PatientId) || (AM == 0 && BM == 0 && CM == 0))
            {
                MessageBox.Show("정보를 입력해주세요");
                return;
            }

            string strQuery = "INSERT INTO " + " prescribeinfo " + " ( PatientId, "
                              + " HospitalNum, " + " A, " + "B, " + "C, " + " TDate, " +
                              " HoName, " + " Holocation, " + " PName) " +
                              " VALUES " +
                              " ( @PatientId, " + " @HospitalNum, " + " @A, " + "@B, " + "@C, " +
                              " @TDate, " + " @HoName, " + " @Holocation, " +
                              " @PName); ";
            
            //InfoViewModel.SelectItems();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString1))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    MySqlParameter paramHospitalNum = new MySqlParameter("@HospitalNum", MySqlDbType.Int32);
                    paramHospitalNum.Value = Commons.DEV_ID;
                    cmd.Parameters.Add(paramHospitalNum);

                    MySqlParameter paramName = new MySqlParameter("@PName", MySqlDbType.VarChar);
                    paramName.Value = sel() == null ? "": sel().ToString().Trim();
                    cmd.Parameters.Add(paramName);

                    MySqlParameter paramPatientId = new MySqlParameter("@PatientId", MySqlDbType.Int32);
                    paramPatientId.Value = int.Parse(PatientId);
                    cmd.Parameters.Add(paramPatientId);

                    MySqlParameter paramA = new MySqlParameter("@A", MySqlDbType.Int32);
                    paramA.Value = AM;
                    cmd.Parameters.Add(paramA);

                    MySqlParameter paramB = new MySqlParameter("@B", MySqlDbType.Int32);
                    paramB.Value = BM;
                    cmd.Parameters.Add(paramB);

                    MySqlParameter paramC = new MySqlParameter("@C", MySqlDbType.Int32);
                    paramC.Value = CM;
                    cmd.Parameters.Add(paramC);

                    MySqlParameter paramDatee = new MySqlParameter("@TDate", MySqlDbType.VarChar);
                    paramDatee.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters.Add(paramDatee);
                    
                    MySqlParameter paramHolocation = new MySqlParameter("@Holocation", MySqlDbType.VarChar);
                    paramHolocation.Value = LoginApp.ViewModels.LoginViewModel.Infoo[0].ADDR;
                    cmd.Parameters.Add(paramHolocation);

                    MySqlParameter paramHoName = new MySqlParameter("@HoName", MySqlDbType.VarChar);
                    paramHoName.Value = LoginApp.ViewModels.LoginViewModel.Infoo[0].Name;
                    cmd.Parameters.Add(paramHoName);

                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            PName = null;
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
            PName = Datee = null;
            AM = BM = CM = 0;
            //Medicine = MList[0];
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
                              " where HospitalNum = @HospitalNum ";
            //string where  = " Where ";
            string _Medi = " PharmNum = @PharmNum ";
            //string _MM = "  Medicine = @Medicine ";
            string _Datee = " TDate Like @TDatee ";
            string _PatientId = " PatientId = @PatientId " ;

            if (string.IsNullOrEmpty(Datee) && string.IsNullOrEmpty(PatientId) && string.IsNullOrEmpty(MediNum))
            {

            }


            else
            {
                strQuery += " AND ";
                if (string.IsNullOrEmpty(Datee) && string.IsNullOrEmpty(MediNum))
                {
                    strQuery += (_PatientId);
                }

                else if (string.IsNullOrEmpty(PatientId) && string.IsNullOrEmpty(MediNum))
                {
                    strQuery += (_Datee);
                }

                else if (string.IsNullOrEmpty(PatientId) && string.IsNullOrEmpty(Datee))
                {
                    strQuery += (_Medi);
                }

                else if (string.IsNullOrEmpty(MediNum))
                {
                    strQuery += (_PatientId + " AND " + _Medi);
                }

                else if (string.IsNullOrEmpty(Datee))
                {
                    strQuery += (_PatientId + " AND " + _Medi);
                }

                else if (string.IsNullOrEmpty(PatientId))
                {
                    strQuery += ( _Medi + " AND " + _Datee);
                }

                else
                {
                    strQuery += (_Datee + " AND " + _PatientId + " AND " + _Medi);
                }
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConnString1))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = strQuery;

                    MySqlParameter paramHospitalNum = new MySqlParameter("@HospitalNum", MySqlDbType.Int32);
                    paramHospitalNum.Value = Commons.DEV_ID;
                    cmd.Parameters.Add(paramHospitalNum);

                    if (!string.IsNullOrEmpty(MediNum))
                    {
                        MySqlParameter paramPharmNum = new MySqlParameter("@PharmNum", MySqlDbType.Int32);
                        paramPharmNum.Value = int.Parse(mediNum);
                        cmd.Parameters.Add(paramPharmNum);

                    }

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


                    if (!string.IsNullOrEmpty(Datee))
                    {
                        MySqlParameter paramDatee = new MySqlParameter("@TDatee", MySqlDbType.VarChar);
                        paramDatee.Value = DateTime.Parse(Datee).ToString("yyyy-MM-dd") + "% ";
                        cmd.Parameters.Add(paramDatee);
                        MessageBox.Show(strQuery+ paramDatee.Value);
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
                            PharmNum = (string.IsNullOrEmpty(reader["PharmNum"].ToString()))? 0: int.Parse(reader["PharmNum"].ToString()),
                            AM = int.Parse(reader["A"].ToString()),
                            BM = int.Parse(reader["B"].ToString()),
                            CM = int.Parse(reader["C"].ToString()),
                            TDate = reader["TDate"].ToString(),
                            getDate = (string.IsNullOrEmpty(reader["getDate"].ToString()))?"":reader["getDate"].ToString(),
                            HoName = reader["HoName"].ToString(),
                            Holocation = reader["Holocation"].ToString(),
                            PharmName = (string.IsNullOrEmpty(reader["PharmName"].ToString()))? "" : reader["PharmName"].ToString(),
                            Plocation = (string.IsNullOrEmpty(reader["Plocation"].ToString()))? "" : reader["Plocation"].ToString(),
                            PName = (string.IsNullOrEmpty(reader["PName"].ToString()))? "" : reader["PName"].ToString(),
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
