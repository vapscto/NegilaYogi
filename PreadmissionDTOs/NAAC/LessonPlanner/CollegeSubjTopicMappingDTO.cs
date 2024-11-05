using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.LessonPlanner
{
    public class CollegeSubjTopicMappingDTO
    {
        public long LPMTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long LPMMTC_Id { get; set; }
        public string LPMTC_TopicName { get; set; }
        public string LPMTC_LessonPlan { get; set; }
        public long LPMTC_TotalHrs { get; set; }
        public long LPMTC_TotalPeriods { get; set; }      
        public int LPMTC_TopicOrder { get; set; }
        public long LPMTC_CreatedBy { get; set; }
        public long LPMTC_UpdatedBy { get; set; }        
        public long LPMU_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long LPMTRC_Id { get; set; }
        public int ASMAY_Order { get; set; }
        public int LPMMTC_Order { get; set; }
        public int LPMU_Order { get; set; }
        public string message { get; set; }
        public string LPMTC_TeacherGuide { get; set; }
        public string LPMTC_StudentGuide { get; set; }
        public string LPMTC_MaterialNeeded { get; set; }
        public string LPMTC_References { get; set; }
        public string LPMTC_Homework { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string LPMMTC_TopicName { get; set; }
        public string LPMTRC_ResourceType { get; set; }
        public string LPMU_UnitName { get; set; }
        public string LPMTRC_Resources { get; set; }
        public string LPMTRC_FileName { get; set; }
        public string Flag { get; set; }
        public bool returnval { get; set; }
        public bool LPMTC_Activefalg { get; set; }
        public Array getdetails { get; set; }
        public Array activedetails { get; set; }
        public Array topicdetails { get; set; }
        public Array unitdetails { get; set; }
        public Array getsubjectlist { get; set; }
        public Array getorderdetails { get; set; }        
        public Array geteditdetials { get; set; }
        public Array gettopicdetailsorder { get; set; }
        public Array subtopicdetails { get; set; }
        public Array savedetails { get; set; }
        public Array getyear { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array getsubject { get; set; }
        public Array uploadfiles { get; set; }
        public CollegeTeacherGuideUploadDTO[] TeacherGuideUploadDTO { get; set; }
        public CollegeStudnetGuideUploadDTO[] StudnetGuideUploadDTO { get; set; }
        public CollegeMateralGuideUploadDTO[] MateralGuideUploadDTO { get; set; }
        public CollegeReferenceGuideUploadDTO[] ReferenceGuideUploadDTO { get; set; }
        public CollegeSubjectWithMasterTopicMappingTemporderDTO[] CollegeSubjectTopicMappingTemporderDTO { get; set; }
    }
    public class CollegeSubjectWithMasterTopicMappingTempDTO
    {
        public long LPMT_Id { get; set; }
        public string LPMT_TopicName { get; set; }

    }
    public class CollegeSubjectWithMasterTopicMappingTemporderDTO
    {
        public long LPMMTC_Id { get; set; }
        public long LPMTC_Id { get; set; }
        public int LPMTC_TopicOrder { get; set; }

    }
    public class CollegeSubjectWithMasterTopicResourceMappingTempDTO
    {
        public long LPMTR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string LPMTR_FileName { get; set; }
    }
    public class CollegeTeacherGuideUploadDTO
    {
        public long LPMTRC_Id { get; set; }
        public string LPMTRC_Resources { get; set; }
        public string LPMTRC_ResourceType { get; set; }
        public string LPMTRC_FileName { get; set; }
    }
    public class CollegeStudnetGuideUploadDTO
    {
        public long LPMTRC_Id { get; set; }
        public string LPMTRC_Resources { get; set; }
        public string LPMTRC_ResourceType { get; set; }
        public string LPMTRC_FileName { get; set; }
    }
    public class CollegeMateralGuideUploadDTO
    {
        public long LPMTRC_Id { get; set; }
        public string LPMTRC_Resources { get; set; }
        public string LPMTRC_ResourceType { get; set; }
        public string LPMTRC_FileName { get; set; }
    }
    public class CollegeReferenceGuideUploadDTO
    {
        public long LPMTRC_Id { get; set; }
        public string LPMTRC_Resources { get; set; }
        public string LPMTRC_ResourceType { get; set; }
        public string LPMTRC_FileName { get; set; }
    }
}
