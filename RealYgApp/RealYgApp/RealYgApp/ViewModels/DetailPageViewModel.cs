using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using RealYgApp.Helpers;
using RealYgApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace RealYgApp.ViewModels
{
    public class DetailPageViewModel : BindableBase//, INavigatedAware
    {
        public double Xlocation;
        public double Ylocation;

        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private Patient selectedPatient = Commons.patient;
        public Patient SelectedPatient
        {
            get { return selectedPatient; }
            set { SetProperty(ref selectedPatient, value); }
        }

        private string tDate;
        public string TDate
        {
            get { return tDate; }
            set { SetProperty(ref tDate, value); }
        }

        //GetDate ParmName Plocation

        private string medicine;
        public string Medicine
        {
            get { return medicine; }
            set { SetProperty(ref medicine, value); }
        }

        private string hoName;
        public string HoName
        {
            get { return hoName; }
            set { SetProperty(ref hoName, value); }
        }

        private string hoLocation;
        public string HoLocation
        {
            get { return hoLocation; }
            set { SetProperty(ref hoLocation, value); }
        }

        private string getDate;
        public string GetDate
        {
            get { return getDate; }
            set { SetProperty(ref getDate, value); }
        }

        private string parmName;
        public string ParmName
        {
            get { return parmName; }
            set { SetProperty(ref parmName, value); }
        }

        private string plocation;
        public string Plocation
        {
            get { return plocation; }
            set { SetProperty(ref plocation, value); }
        }

        public DelegateCommand HoMapCommand { get; private set; }
        public DelegateCommand PMapCommand { get; private set; }
        public DetailPageViewModel()
        {
            TDate = selectedPatient.TDate;
            HoName = selectedPatient.HoName;
            Medicine = selectedPatient.Medicine;
            ParmName = selectedPatient.ParmName;
            HoLocation = selectedPatient.Holocation;
            GetDate = selectedPatient.getdate;
            Plocation = selectedPatient.Plocation;

            Title = "처방전 상세정보";

            HoMapCommand = new DelegateCommand(GoHos);
            PMapCommand = new DelegateCommand(GoParm);
        }

        private void GoHos()
        {
            GetXY(HoLocation,HoName);
        }
        private void GoParm()
        {
            GetXY(Plocation, ParmName);
        }

        private async void GetXY(string Target, string Name)
        {
            
            string Temp = Target;
            WebClient wc = new WebClient() { Encoding = Encoding.UTF8 }; //API로 x,y좌표값 얻어오기
            XmlDocument doc = new XmlDocument();

            StringBuilder str = new StringBuilder();

            str.Append("http://api.vworld.kr/req/address");              //open API기본URL
            str.Append("?service=address&request=getcoord&version=2.0"); //Geocoder API 2.0
            str.Append("&crs=epsg:4326");                                //WGS84 경위도이용
            str.Append($"&address={Temp}");                       //검색주소
            str.Append("&refine=true");                                  //주소 정제해서 사용
            str.Append("&simple=false&format=xml");
            if (Temp.Contains("번지"))
            { str.Append("&type=parcel"); }
            else
            { str.Append("&type=road"); }//도로명 주소 

            str.Append("&key=6677E95B-8E1C-38D1-8B74-875AE27E3B15");     //인증키

            string xml = wc.DownloadString(str.ToString());
            doc.LoadXml(xml);

            XmlElement root = doc.DocumentElement;
            XmlNodeList items = doc.GetElementsByTagName("point");

            // GrdNam.Rows.Clear();

            foreach (XmlNode item in items)
            {
                try
                {
                    Xlocation = Double.Parse(item["x"].InnerText);
                    Ylocation = Double.Parse(item["y"].InnerText);

                    await Map.OpenAsync(Ylocation, Xlocation, new MapLaunchOptions
                    {
                        Name = Name,
                        NavigationMode = Xamarin.Essentials.NavigationMode.None
                    });
                }
                catch (NullReferenceException ex)
                {
                    return;
                }
            }

        }
    }

    //public void OnNavigatedFrom(INavigationParameters parameters)
    //{
    //}

    //public void OnNavigatedTo(INavigationParameters parameters)
    //{
    //    selectedPatient = parameters["SeletedPatient"] as Patient;
    //}

}
