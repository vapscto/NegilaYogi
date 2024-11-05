using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class AdmittedStudentSiblingDTO
    {
        public long AMSTS_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMSTS_SiblingsName { get; set; }
        public string AMSTS_SiblingsRelation { get; set; }
        public long AMCL_Id { get; set; }
        public long AMSTS_Siblings_AMST_ID { get; set; }
        public int AMSTS_SiblingsOrder { get; set; }
        public string AMSTS_TCIssuesFlag { get; set; }

    }
}
