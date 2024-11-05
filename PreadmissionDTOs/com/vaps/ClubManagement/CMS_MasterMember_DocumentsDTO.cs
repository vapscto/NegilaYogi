using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
    public class CMS_MasterMember_DocumentsDTO
    {
        public long UserId { get; set; }
        public document[] documents { get; set; }
        public long CMSMMEM_Id { get; set; }
        public long CMSMMEM_Idtwo { get; set; }
        public string returnval { get; set; }
    }
    public class document
    {
        public string CMSMMEMDOC_DocumentName { get; set; }
        public string CMSMMEMDOC_FileName { get; set; }
        public string CMSMMEMDOC_FilePath { get; set; }
    }
}
