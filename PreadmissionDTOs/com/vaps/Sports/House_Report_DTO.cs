using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class House_Report_DTO : CommonParamDTO
    {
        public long SPCCMH_Id { get; set; }
        public string SPCCMH_HouseName { get; set; }
        public Array yearlist { get; set; }
        public Array houseList { get; set; }
        public long MI_Id { get; set; }
        public Array viewlist { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_AdmNo { get; set; }
        public string studentname { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public DateTime AMST_DOB { get; set; }
        public Array classList { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public Array sectionList { get; set; }
        public string Type { get; set; }
        public House_Report_DTO[] selectedhouselist { get; set; }
        public House_Report_DTO[] selectedSectionlist { get; set; }
        public Houser_Class_DTO[] Classelectedlist { get; set; }
        public Array exam_list { get; set; }
        public ExamListHouse[] ExamListHouses { get; set; }
        public Array HouseTotal { get; set; }
        public Array OverallCount { get; set; }
    }
    public class Houser_Class_DTO
    {
        public long ASMCL_Id { get; set; }
    }
    public class ExamListHouse
    {
        public long EME_Id { get; set; }
    }
}
