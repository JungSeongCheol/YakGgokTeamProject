using System;
using System.Collections.Generic;
using System.Text;
using XamarinPrismQrGen.Models;

namespace XamarinPrismQrGen.Helpers
{
    public class Commons
    {
        internal static readonly string CONSTRING =
        "Server=192.168.0.165;Port=3306;Database=member;uid=root;password=mysql_p@ssw0rd"; //192.168.0.165 //희지 210.119.12.53


        internal static readonly string CONSTRINGList = "Server=192.168.0.165;Port=3306;" +
               "Database=gguklist;Uid=root;Pwd=mysql_p@ssw0rd";

        //환자 고유 ID
        public static string ID;
        public static Patient patient;
    }
}
