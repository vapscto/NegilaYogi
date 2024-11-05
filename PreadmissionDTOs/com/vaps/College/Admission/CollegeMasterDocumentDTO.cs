using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeMasterDocumentDTO:CommonParamDTO
    {
        public long ACSTD_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ACSMD_Id { get; set; }
        public string ACSTD_Doc_Path { get; set; }
        public string ACSTD_Doc_Name { get; set; }
        public long AMSMD_Id { get; set; }
        public string AMSMD_DocumentName { get; set; }
        public string Document_Path { get; set; }
        public bool AMSMD_FLAG { get; set; }
    }
}
