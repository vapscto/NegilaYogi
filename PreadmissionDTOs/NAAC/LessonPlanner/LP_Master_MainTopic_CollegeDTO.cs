using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.LessonPlanner
{
    public class LP_Master_MainTopic_CollegeDTO
    {
        public long ASMAY_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long LPMMTC_Id { get; set; }       
        public long LPMU_Id { get; set; }       
        public long LPMMTC_TotalHrs { get; set; }
        public long LPMMTC_TotalPeriods { get; set; }
        public bool LPMMTC_ActiveFlg { get; set; }
        public long LPMMTC_CreatedBy { get; set; }
        public long LPMMTC_UpdatedBy { get; set; }
        public Array getdetails { get; set; }
        public Array getyear { get; set; }
        public Array editdetails { get; set; }
        public Array activedetactive { get; set; }
        public Array getsubject { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array getsubjecttopicdetails { get; set; }
        public Array getunitlist { get; set; }
        public string message { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string LPMMTC_TopicName { get; set; }
        public string LPMU_UnitName { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string LPMMTC_TopicDescription { get; set; }
        public bool returnval { get; set; }
        public int LPMMTC_Order { get; set; }
        public int ASMAY_Order { get; set; }
        public MastercollegeTopicOrderDTO[] MastercollegeTopicOrderDTO { get; set; }
    }
    public class MastercollegeTopicOrderDTO
    {
        public long LPMMTC_Id { get; set; }
        public int LPMMTC_Order { get; set; }

    }
}
