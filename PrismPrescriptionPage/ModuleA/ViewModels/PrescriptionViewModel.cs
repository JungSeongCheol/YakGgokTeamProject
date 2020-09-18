using ModuleA.Models;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleA.ViewModels
{
    public class PrescriptionViewModel:BindableBase
    {
        private string title="안녕";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        /// <summary>
        /// 선택된 환자
        /// </summary>
        private Patient selectedPatient;
        public Patient SelectedPatient
        {
            get { return selectedPatient; }
            set { SetProperty(ref selectedPatient, value); }
        }

        private string patientId;
        public string PatientId
        {
            get { return patientId; }
            set { SetProperty(ref patientId, value); }
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

        public PrescriptionViewModel()
        {
            Selectedproperty();
            
        }

        /// <summary>
        /// 선택된 환자의 데이터 불러옴
        /// </summary>
        private void Selectedproperty()
        {
            TDate = selectedPatient.TDate;
            HoName = selectedPatient.HoName;
            Medicine = selectedPatient.Medicine;
            ParmName = selectedPatient.ParmName;
            HoLocation = selectedPatient.Holocation;
            GetDate = selectedPatient.getdate;
            Plocation = selectedPatient.Plocation;
            PatientId = "1";
        }

    }
}
