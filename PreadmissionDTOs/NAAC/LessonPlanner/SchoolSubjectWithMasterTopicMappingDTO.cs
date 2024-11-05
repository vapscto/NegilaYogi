using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam.LessonPlanner
{
    public class SchoolSubjectWithMasterTopicMappingDTO
    {
        public long LPMT_Id { get; set; }
        public long MI_Id { get; set; }
        public long LPMMT_Id { get; set; }
        public long LPMU_Id { get; set; }
        public string LPMT_TopicName { get; set; }
        public string LPMU_UnitName { get; set; }
        public string LPMT_LessonPlan { get; set; }
        public long? LPMT_TotalHrs { get; set; }
        public long? LPMT_TotalPeriods { get; set; }
        public string LPMT_TeacherGuide { get; set; }
        public string LPMT_StudentGuide { get; set; }
        public string LPMT_MaterialNeeded { get; set; }
        public string LPMT_References { get; set; }
        public string LPMT_Homework { get; set; }
        public int LPMT_TopicOrder { get; set; }
        public int LPMU_Order { get; set; }
        public bool LPMT_ActiveFlag { get; set; }
        public long LPMT_CreatedBy { get; set; }
        public long LPMT_UpdatedBy { get; set; }
        public Array getdetails { get; set; }
        public Array activedetails { get; set; }
        public Array topicdetails { get; set; }
        public Array unitdetails { get; set; }
        public Array getsubjectlist { get; set; }
        public Array getorderdetails { get; set; }
        public string message { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string LPMMT_TopicName { get; set; }
        public string Flag { get; set; }
        public bool returnval { get; set; }
        public string LPMTR_ResourceType { get; set; }
        public Array uploadfiles { get; set; }
        public long Userid { get; set; }
        public long ISMS_Id { get; set; }
        public int LPMMT_Order { get; set; }
        public Array geteditdetials { get; set; }
        public Array gettopicdetailsorder { get; set; }
        public SchoolSubjectWithMasterTopicMappingTempDTO[] SchoolSubjectWithMasterTopicMappingTempDTO { get; set; }
        public SchoolSubjectWithMasterTopicMappingTemporderDTO[] SchoolSubjectWithMasterTopicMappingTemporderDTO { get; set; }
        public string LPMTR_Resources { get; set; }
        public string LPMTR_FileName { get; set; }
        public long LPMTR_Id { get; set; }
        public Array subtopicdetails { get; set; }
        public Array savedetails { get; set; }
        public SchoolSubjectWithMasterTopicResourceMappingTempDTO[] SchoolSubjectWithMasterTopicResourceMappingTempDTO { get; set; }
        public TeacherGuideUploadDTO[] TeacherGuideUploadDTO { get; set; }
        public StudnetGuideUploadDTO[] StudnetGuideUploadDTO { get; set; }
        public MateralGuideUploadDTO[] MateralGuideUploadDTO { get; set; }
        public ReferenceGuideUploadDTO[] ReferenceGuideUploadDTO { get; set; }
        public Array getclass { get; set; }
        public Array getyear { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public int ASMAY_Order { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
    }
    public class SchoolSubjectWithMasterTopicMappingTempDTO
    {
        public long LPMT_Id { get; set; }
        public string LPMT_TopicName { get; set; }

    }
    public class SchoolSubjectWithMasterTopicMappingTemporderDTO
    {
        public long LPMMT_Id { get; set; }
        public long LPMT_Id { get; set; }       
        public int LPMT_TopicOrder { get; set; }

    }
    public class SchoolSubjectWithMasterTopicResourceMappingTempDTO
    {
        public long LPMTR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string LPMTR_FileName { get; set; }
    }
    public class TeacherGuideUploadDTO
    {
        public long LPMTR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string LPMTR_FileName { get; set; }
    }
    public class StudnetGuideUploadDTO
    {
        public long LPMTR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string LPMTR_FileName { get; set; }
    }
    public class MateralGuideUploadDTO
    {
        public long LPMTR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string LPMTR_FileName { get; set; }
    }
    public class ReferenceGuideUploadDTO
    {
        public long LPMTR_Id { get; set; }
        public string LPMTR_Resources { get; set; }
        public string LPMTR_FileName { get; set; }
    }
}
