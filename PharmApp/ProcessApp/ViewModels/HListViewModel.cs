using MySql.Data.MySqlClient;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using ProcessApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace ProcessApp.ViewModels
{
    public class HListViewModel : BindableBase
    {
        private IRegionManager _regionManager;

        string strConnString = "Server=localhost;Port=3306;" +
                              "Database=Hospital;Uid=root;Pwd=mysql_p@ssw0rd;";


        private DataSet mmList = new DataSet();
        
        public DataSet MmList
        {
            get => mmList;
            set => SetProperty(ref mmList, value);
        }

        private string nname = null;
        public string nName
        {
            get => nname;
            set => SetProperty(ref nname, value);
        }

        private string nmedicine = null;
        public string nMedicine
        {
            get => nmedicine;
            set => SetProperty(ref nmedicine, value);
        }

        private string ndate = null;

        public string nDatee
        {
            get => ndate;
            set => SetProperty(ref ndate, value);
        }

        List<PrescriptionModel> psList;
        public List<PrescriptionModel> PsList
        {
            get => psList;
            set => SetProperty(ref psList, value);
        }

        IEventAggregator _ea;
        //IEventAggregator _Name;
        //IEventAggregator _Date;
        //IEventAggregator _Medicine;

        public HListViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            

            PsList = new List<PrescriptionModel>();

           
        }


   
     
    }
}
