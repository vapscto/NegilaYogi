using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.ClubManagement
{
   public class CMS_MasterDepartmentDTO
    {
        public long MI_Id { get; set; }

        public long UserId { get; set; }

        public Array pages { get; set; }
        public long CMSMDEPT_Id { get; set; }
        public string CMSMDEPT_DepartmentName { get; set; }
        public string CMSMDEPT_DeptCode { get; set; }
        public bool CMSMDEPT_ActiveFlag { get; set; }
        public string returnval { get; set; }
              
    }
}
