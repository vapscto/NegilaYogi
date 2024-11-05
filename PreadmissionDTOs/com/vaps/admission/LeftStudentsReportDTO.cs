using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class LeftStudentsReportDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long MI_Id { get; set; }
        public long roleid { get; set; }
        public Array academic { get; set; }
        public Array category { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array viewlist { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long AMC_Id { get; set; }
        public string AMC_Name { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_SOL { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMST_MobileNo { get; set; }

        public academicyearss[] academicyears { get; set; }
        public categorylist[] categorylists { get; set; }
        
        public sectionlistarray12[] sectionlistarray { get; set; }
        public classlsttwooo[] classlsttwo { get; set; }
    }
    public class academicyearss
    {
        public long ASMAY_Id { get; set; }
        public long AMC_Id { get; set; }
    }
    //categorylists
    public class categorylist
    {
        public long ASMCL_Id { get; set; }
        public long AMC_Id { get; set; }
    }
    public class classlsttwooo
    {
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
    }
    public class sectionlistarray12
    {
        public long ASMS_Id { get; set; }

        public long ASMCL_Id { get; set; }
    }
}
