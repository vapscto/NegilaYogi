using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class SatRegistrationDTO
    {
        public long PASRE_Id { get; set; }
        public long MI_Id { get; set; }
        public string PASRE_FullName { get; set; }
        public string PASRE_EmailId { get; set; }
        public string PASRE_FatherName { get; set; }
        public string PASRE_SchoolName { get; set; }
        public string PASRE_Gender { get; set; }
        public string PASRE_Address { get; set; }
        public long PASRE_MobileNo { get; set; }
        public long PASRE_WhatsappNo { get; set; }

        public bool returnflag { get; set; }
    }
}
