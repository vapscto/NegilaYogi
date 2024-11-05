using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentDocumentDTO:CommonParamDTO
    {
        public long AMSTD_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long AMSMD_Id { get; set; }
        public string AMSTD_DOC_Path { get; set; }
        public string AMSTD_DOC_Name { get; set; }


    }
}
