using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterTemplateDTO : CommonParamDTO
    {
        public long IVRMT_Id { get; set; }
        public long MI_Id { get; set; }
        
        public long IVRMP_Id { get; set; }
        public string IVRMT_Name { get; set; }
        public string IVRMMP_PageName { get; set; }
        public string IVRMT_Description { get; set; }
        public bool Is_Active { get; set; }
        public string returnval { get; set; }
        public Array pageDrpdwn { get; set; }
        public string IVRMTML_State_Provider { get; set; }
        public Array templateList { get; set; }
        public Array get_Saletypes { get; set; }
        public string templateName { get; set; }

    }
}
