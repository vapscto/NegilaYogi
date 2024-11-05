using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.LessonPlanner
{
    public class LMSStudentDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public Array getallyear { get; set; }
        public Array getcurrentyear { get; set; }
        public Array getstudentdetails { get; set; }
        public Array getsubjectdetails { get; set; }
        public Array getsemesterdetails { get; set; }
        public Array getcurrentsemesterdetails { get; set; }
        public Array gettopiclist { get; set; }
        public Array getunitdetails { get; set; }
        public Array getsubtopicdetails { get; set; }
        public Array getdocumentlist { get; set; }
        public Array getcurrentclass { get; set; }
        public Array getclass { get; set; }

        public long LPMU_Id { get; set; }
        public long LPMMT_Id { get; set; }
        public long LPMT_Id { get; set; }
        public int ASMAY_Order { get; set; }

        public string LPMU_UnitName { get; set; }
        public string LPMMT_TopicName { get; set; }
        public string LPMT_TopicName { get; set; }

        public string LPMU_UnitDescription { get; set; }
        public string LPMMT_TopicDescription { get; set; }
        public string LPMT_LessonPlan { get; set; }

        public int LPMU_Order { get; set; }
        public int LPMMT_Order { get; set; }
        public int LPMT_TopicOrder { get; set; }

        public string LPMTR_Resources { get; set; }
        public string LPMTR_ResourceType { get; set; }
        public string LPMTR_FileName { get; set; }

    }
}
