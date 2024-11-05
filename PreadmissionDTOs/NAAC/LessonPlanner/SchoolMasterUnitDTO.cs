using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam.LessonPlanner
{
    public class SchoolMasterUnitDTO
    {
        public long LPMU_Id { get; set; }
        public long MI_Id { get; set; }
        public string LPMU_UnitName { get; set; }
        public string LPMU_UnitDescription { get; set; }
        public decimal? LPMU_TotalHrs { get; set; }
        public long? LPMU_TotalPeriods { get; set; }
        public int LPMU_Order { get; set; }
        public bool LPMU_ActiveFlag { get; set; }
        public long LPMU_CreatedBy { get; set; }
        public long LPMU_UpdatedBy { get; set; }
        public Array getdetails { get; set; }
        public Array geteditdetails { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public SchoolMasterUnitTempDTO[] SchoolMasterUnitTempDTO { get; set; }

        // Master Unit Topic Mapping
        public Array getunitdetails { get; set; }
        public Array gettopicdetails { get; set; }
        public Array getgriddetails { get; set; }
        public long LPMT_Id { get; set; }
        public string LPMT_TopicName { get; set; }
        public int LPMT_TopicOrder { get; set; }
        public bool LPMUT_ActiveFlag { get; set; }
        public long LPMTU_Id { get; set; }
        public SchoolMasterUnitTopicMappingTempDTO[] SchoolMasterUnitTopicMappingTempDTO { get; set; }


    }
    public class SchoolMasterUnitTempDTO
    {
        public long LPMU_Id { get; set; }
        public int LPMU_Order { get; set; }
    }
    public class SchoolMasterUnitTopicMappingTempDTO
    {
        public long LPMT_Id { get; set; }
    }
}
