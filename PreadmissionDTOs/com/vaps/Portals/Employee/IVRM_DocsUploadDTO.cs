using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class IVRM_DocsUploadDTO:CommonParamDTO
    {
        public long IDU_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long Login_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string IDU_Type { get; set; }
        public string IDU_Remarks { get; set; }
        public string IDU_Attachment { get; set; }
        public string IDU_FilePath { get; set; }
        public bool IDU_ActiveFlag { get; set; }
        public string imagePath { get; set; }
        public long AMST_Id { get; set; }
        public string ASMCL_ClassName { get; set; }  
        public string ASMC_SectionName { get; set; }
        public string finalDate { get; set; }

        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
   
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public string docsPath { get; set; }
       
        public Array worklist { get; set; }
        public Array classSectionlist { get; set; }
        public Array yearlist { get; set; }
        public Array docsDetails { get; set; }
        public Array editlist { get; set; }

        
    }
}
