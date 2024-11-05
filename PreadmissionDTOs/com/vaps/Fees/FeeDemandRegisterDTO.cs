using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeDemandRegisterDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long? ASMS_Id { get; set; }
        public long? AMST_Id { get; set; }
        public long FMG_Id { get; set; }
        public long FMGG_Id { get; set; }
        public long FMT_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public string studentName { get; set; }
        public string regNo { get; set; }
        public string admNo { get; set; }
        public long rollNo { get; set; }
        public long? User_Id { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Array feeconfiguration { get; set; }
        public Array academicYearList { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public Array studentList { get; set; }
        public Array customgrpList { get; set; }
        public Array groupList { get; set; }
        public Array termsList { get; set; }
        public Array userNamesList { get; set; }
        public Array admissinConfiguration { get; set; }
        public string Status { get; set; }
        public string ClassSectionFlag { get; set; }
        public string DetailedFlag { get; set; }
        public string GrandTotalFlag { get; set; }
        public int studentCount { get; set; }
        public FeeDemandRegisterDTO[] selectedCGList { get; set; }
        public FeeDemandRegisterDTO[] selectedGroup { get; set; }
        public FeeDemandRegisterDTO[] selectedTerm { get; set; }
        public string type { get; set; }
        public Array FeedemandregisterReport { get; set; }
        public string fee { get; set; }
        public Array FeeNames { get; set; }
        public Array FeeInstallments { get; set; }
        public int count { get; set; }
        public Array studentdetails { get; set; }
        public Array installmentdetails { get; set; }
        public string install { get; set; }

        public List<FeeDemandRegisterInstallmentDTO> FeeDemandRegisterInstallment { get; set; }
        public FeeDemandRegisterDTO[] selectedStudType { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string newstud { get; set; }

    }
  
}
