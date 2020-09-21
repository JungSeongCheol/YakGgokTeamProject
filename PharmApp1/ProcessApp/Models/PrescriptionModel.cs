namespace ProcessApp.Models
{
    public class PrescriptionModel
    {
        public int IdSub { get; set; }

        public string PatientId { get; set; }

        public int HospitalNum { get; set; }

        public int PharmNum { get; set; }

        public string Medicine { get; set; }

        public int A { get; set; }

        public int B { get; set; }

        public int C { get; set; }

        public string TDate { get; set; }

        public string getDate { get; set; }

        public string HoName { get; set; }

        public string Holocation { get; set; }

        public string PharmName { get; set; }

        public string Plocation { get; set; }

        public string PName { get; set; }
    }
}
