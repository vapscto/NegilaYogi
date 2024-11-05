using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class Master_Employee_DocumentsDTO :CommonParamDTO
    {
        public long HRMEDS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRMEDS_DocumentName { get; set; }
        public string HRMEDS_DocumentImageName { get; set; }
        public string HRMEDS_DucumentDescription { get; set; }

        public string retrunMsg { get; set; }

    }
}
