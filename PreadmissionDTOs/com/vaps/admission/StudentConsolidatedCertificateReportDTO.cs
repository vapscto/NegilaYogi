using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentConsolidatedCertificateReportDTO
    {
        public Array AcademicYear { get; set; }
        public long MI_Id { get; set; }
        public Array MasterCertificate { get; set; }
        public long ASMAY_Id { get; set; }

        public Array getyear1 { get; set; }
        //public long ASMAY_Id { get; set; }
        //public long ASMCL_Id { get; set; }
        //public string asmcL_ClassName { get; set; }
        //public Array ClassDetails { get; set; }

    }
    public class StudentConsolidateCertificateGetClassParaDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string asmcL_ClassName { get; set; }
        public Array ClassDetails { get; set; }
        public Temp_ASMCLIds[] Temp_ASMCLIds { get; set; }
        public Temp_ASMS_Ids[] Temp_ASMS_Ids { get; set; }
        public Array getsectionlist { get; set; }
        public string ASC_ReportType { get; set; }
        public Array GetCertificateDet { get; set; }
        public Array GetstudentDet { get; set; }
        public string ASMCL_Ids { get; set; }
        public string ASMS_Ids { get; set; }
    }
    public class Temp_ASMCLIds
    {
        public long ASMC_Id { get; set; }
    }
    public class Temp_ASMS_Ids
    {
        public long ASMS_Id { get; set; }
    }
}
