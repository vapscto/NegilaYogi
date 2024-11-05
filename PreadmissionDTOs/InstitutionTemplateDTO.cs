using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class InstitutionTemplateDTO : CommonParamDTO
    {
        public long IVRMIT_Id { get; set; }
        public string Template_name { get; set; }
        public long IVRMT_Id { get; set; }
        public string Institute_Name { get; set; }
        public long IVRMIT_MI_Id { get; set; }
        public long IVRMIT_Category_Id { get; set; }
        public bool IVRMIT_ActiveFlag { get; set; }
        public bool IVRMIT_DeleteFlag { get; set; }
        public Array InstEditlist { get; set; }
        public string returnMsg { get; set; }
    }
}
