using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FM6_H.Models
{
    //public class Notification
    //{



    //    public int ID_NOTIFICATION { get; set; }
    //    public string Destinataire { get; set; }
    //    public int ID_USER { get; set; }
    //    public string Redacteur { get; set; }
    //    public string Sujet { get; set; }
    //    public string Message { get; set; }
    //    public string Etat { get; set; }
    //    public DateTime? DATE_EXECUTION { get; set; }
    //    public DateTime DATE_NOTIFICATION { get; set; }



    //}







    public class Notification
    {
        public string Destinataire { get; set; }
        public string Redacteur { get; set; }
        public string Sujet { get; set; }
        public string Message { get; set; }
        public string Etat { get; set; }
        public DateTime? DATE_EXECUTION { get; set; }
        public DateTime DATE_NOTIFICATION { get; set; }
        public int ID_USER { get; set; }
        public int ID_NOTIFICATION { get; set; }
    }
}
