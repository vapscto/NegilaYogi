using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner
{
    public class CollegeSubjetMasterTopicMappingDTO
    {
        public long LPMT_Id { get; set; }
        public long MI_Id { get; set; }
        public long LPMMT_Id { get; set; }
        public string LPMT_TopicName { get; set; }
        public string LPMT_LessonPlan { get; set; }
        public long LPMT_TotalHrs { get; set; }
        public long LPMT_TotalPeriods { get; set; }
        public string LPMT_TeacherGuide { get; set; }
        public string LPMT_StudentGuide { get; set; }
        public string LPMT_MaterialNeeded { get; set; }
        public string LPMT_References { get; set; }
        public string LPMT_Homework { get; set; }
        public int LPMT_TopicOrder { get; set; }
        public bool LPMT_ActiveFlag { get; set; }
        public long LPMT_CreatedBy { get; set; }
        public long LPMT_UpdatedBy { get; set; }
        public Array getdetails { get; set; }
        public Array activedetails { get; set; }
        public Array topicdetails { get; set; }
        public Array getsubjectlist { get; set; }
        public Array getorderdetails { get; set; }
        public string message { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string LPMMT_TopicName { get; set; }
        public bool returnval { get; set; }
        public long Userid { get; set; }
        public long ISMS_Id { get; set; }
        public int LPMMT_Order { get; set; }
        public Array geteditdetials { get; set; }
        public Array gettopicdetailsorder { get; set; }
        public CollegeSubjectWithMasterTopicMappingTempDTO[] CollegeSubjectWithMasterTopicMappingTempDTO { get; set; }
        public CollegeSubjectWithMasterTopicMappingTemporderDTO[] CollegeSubjectWithMasterTopicMappingTemporderDTO { get; set; }
        public string LPMTR_Resources { get; set; }
        public long LPMTR_Id { get; set; }
        public Array subtopicdetails { get; set; }
        public Array savedetails { get; set; }
        public collegeSubjectWithMasterTopicResourceMappingTempDTO[] collegeSubjectWithMasterTopicResourceMappingTempDTO { get; set; }
    }
    public class CollegeSubjectWithMasterTopicMappingTempDTO
    {
        public long LPMT_Id { get; set; }
        public string LPMT_TopicName { get; set; }
    }

    public class CollegeSubjectWithMasterTopicMappingTemporderDTO
    {
        public long LPMMT_Id { get; set; }
        public long LPMT_Id { get; set; }
        public int LPMT_TopicOrder { get; set; }

    }
    public class collegeSubjectWithMasterTopicResourceMappingTempDTO
    {
        public long LPMTR_Id { get; set; }
        public string LPMTR_Resources { get; set; }

    }
}

