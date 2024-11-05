using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class StudentSiblingDTO : CommonParamDTO
    {
        public long PASRS_Id { get; set; }
        public long PASR_Id { get; set; }
        public string PASRS_SiblingsName { get; set; }
        public string PASRS_SiblingsClass { get; set; }
        public string PASRS_SiblingsAdmissionNo { get; set; }
        public string PASRS_SiblingsSection { get; set; }
        public string verrejstatus { get; set; }
        public long AMST_Id { get; set; }
        public long? HRME_Id { get; set; }
        public string PASRS_SchoolName { get; set; }
        public string PASRS_Age { get; set; }
        public string PASRS_Gender { get; set; }
        public string PASRS_Institution { get; set; }
        public string PASRS_DOB { get; set; }
    }
}
