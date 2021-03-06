﻿using LoginApp.Models;
using MySql.Data.MySqlClient;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProcessApp.Models;
using LoginApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProcessApp.ViewModels
{
    public class InfoViewModel:BindableBase
    {
        IRegionManager _regionManager;
        string strConnString = "Server=localhost;Port=3306;" +
                               "Database=gguklist;Uid=root;Pwd=mysql_p@ssw0rd;";

        public DelegateCommand<string> BackCommand { get; set; }

        public InfoViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            PsList = LoginApp.ViewModels.LoginViewModel.Infoo;
            //SelectItem();
            DEV_ID = PsList[0].DEV_ID;
            Name = PsList[0].Name;
            SIDO = PsList[0].SIDO;
            SIGUGUN = PsList[0].SIGUGUN;
            POSTNUM = psList[0].POSTNUM;
            ADDR = psList[0].ADDR;
            CONTACT = psList[0].CONTACT;
            PX = psList[0].PX;
            PY = psList[0].PY;

            BackCommand = new DelegateCommand<string>(Back);
        }

        List<InfoModel> psList;
        public List<InfoModel> PsList
        {
            get => psList;
            set => SetProperty(ref psList, value);
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



        

        private void Back(string uri)
        {
            _regionManager.RequestNavigate("Menu", uri);

            _regionManager.Regions["Hello"].RemoveAll();
        }
    }
}
