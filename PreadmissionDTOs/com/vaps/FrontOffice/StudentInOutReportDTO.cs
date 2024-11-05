using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.FrontOffice
{
    public class StudentInOutReportDTO
    {
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }

        public long MI_Id { get; set; }
        public long roleid { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array studentlist1 { get; set; }
        public Array studentlist { get; set; }
        public Array viewlist { get; set; }
        public long ASMCL_Id { get; set; }
        public long AMST_Id { get; set; }
        public long amstid { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string inoutflag { get; set; }
        public string optionflag { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string AMST_FirstName { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public classlsttwo[] classlsttwo { get; set; }
        public sectionlistarray1[] sectionlistarray { get; set; }
    }
    public class classlsttwo
    {
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
    }
    public class sectionlistarray1
    {
        public long ASMS_Id { get; set; }

        public long ASMCL_Id { get; set; }
    }
}
