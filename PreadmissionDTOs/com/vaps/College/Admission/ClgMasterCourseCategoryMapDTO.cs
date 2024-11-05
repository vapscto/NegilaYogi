using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class ClgMasterCourseCategoryMapDTO : CommonParamDTO
    {
        public long AMCOCM_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMCOC_Id { get; set; }
        public bool AMCOCM_ActiveFlg { get; set; }

        public Array MasterCourseList { get; set; }

        public Array mastercategory { get; set; }

        public Array grid { get; set; }
        public bool returnval { get; set; }

        public string AMCO_CourseName { get; set; }

        public string AMCOC_Name { get; set; }
        public string message { get; set; }

    }
}
