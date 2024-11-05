using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Preadmission
{
   public class CollegeDocumentDTO :CommonParamDTO
    {
        public long PACSTD_Id { get; set; }
        public long PACA_Id { get; set; }
        public long AMSMD_Id { get; set; }
        public string ACSTD_Doc_Path { get; set; }
        public string ACSTD_Doc_Name { get; set; }

        public string AMSMD_DocumentName { get; set; }

        public string Document_Path { get; set; }

        public string AMSMD_FilePath { get; set; }
        public string AMSMD_FileName { get; set; }
    }
}
