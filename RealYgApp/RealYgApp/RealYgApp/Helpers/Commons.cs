using RealYgApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealYgApp.Helpers
{
    public class Commons
    {
        public static readonly string CONNSTRING = "Server=210.119.12.53;Port=3306;" +
               "Database=gguklist;Uid=root;Pwd=mysql_p@ssw0rd";

        //환자 고유 ID
        public static string ID = "1";
        public static Patient patient;
    }
  
}
