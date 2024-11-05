using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs
{
    public class InstitutionUserMappingDTO
    {
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long smscreditalert { get; set; }
        public long UserId { get; set; }
        public long IVRMUL_Id { get; set; }
        public string employeename { get; set; }
        public string subscriptionremarks { get; set; }
        public string employeecode { get; set; }
        public string MI_Name { get; set; }
        public bool returnval { get; set; }
        public Array getinstitution { get; set; }
        public Array getinstitutionloaddata { get; set; }
        public Array getsavedemployee { get; set; }
        public Array getemployeedetails { get; set; }
        public Array getviewemployeedetails { get; set; }
        public saveselectedlist[] saveselectedlist { get; set; }
        public string paymentsubscriptiontype { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public Array getreportdetails { get; set; }
    }
    public class saveselectedlist
    {
        public long HRME_Id { get; set; }
    }   
}