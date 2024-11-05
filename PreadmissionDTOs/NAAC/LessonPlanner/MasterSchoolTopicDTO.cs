using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam.LessonPlanner
{
    public class MasterSchoolTopicDTO
    {
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public Array getdetails { get; set; }
        public Array editdetails { get; set; }
        public Array activedetactive { get; set; }
        public Array getsubject { get; set; }
        public Array getsubjecttopicdetails { get; set; }
        public bool returnval { get; set;}
        public long LPMMT_Id { get; set; }       
        public long ISMS_Id { get; set; }
        public long LPMU_Id { get; set; }
        public string LPMMT_TopicName { get; set; }
        public string LPMU_UnitName { get; set; }
        public string LPMMT_TopicDescription { get; set; }
        public long? LPMMT_TotalHrs { get; set; }
        public long? LPMMT_TotalPeriods { get; set; }
        public int LPMMT_Order { get; set; }
        public bool LPMMT_ActiveFlag { get; set; }
        public long LPMMT_CreatedBy { get; set; }
        public long LPMMT_UpdatedBy { get; set; }
        public string message { get; set; }
        public string ISMS_SubjectName { get; set; }
        public Array getunitlist { get; set; }        
        public MasterSchoolTopicOrderDTO[] MasterSchoolTopicOrderDTO { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public Array getyear { get; set; }
        public Array getclass { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int ASMAY_Order { get; set; }
    }
    public class MasterSchoolTopicOrderDTO
    {
        public long LPMMT_Id { get; set; }
        public int LPMMT_Order { get; set; }

    }
}
