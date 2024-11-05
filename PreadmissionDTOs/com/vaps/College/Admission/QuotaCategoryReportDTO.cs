using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class QuotaCategoryReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ACQ_Id { get; set; }
        public long ACQC_Id { get; set; }
        public string std_name { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public DateTime? AMCST_DOB { get; set; }
        public string AMCST_FatherName { get; set; }
        public string AMCST_FatherOccupation { get; set; }
        public string AMCST_Sex { get; set; }
        public string std_address { get; set; }
        public string ACQ_QuotaName { get; set; }
        public string ACQC_CategoryName { get; set; }
        public long AMCST_MobileNo { get; set; }

        public Array acdlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semlist { get; set; }
        public Array seclist { get; set; }
        public Array quotalist { get; set; }
        public Array catlist { get; set; }
        public Array datareport { get; set; }
        public branchid[] branchid { get; set; }
        public semester[] semester { get; set; }
        public quota[] quota { get; set; }
        public quotacategry[] quotacategry { get; set; }
        public Array masterinst { get; set; }

    }

    public class branchid
    {
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
    }
    public class semester
    {
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
    }
    public class quota
    {
        public long ACQ_Id { get; set; }
        public string ACQ_QuotaName { get; set; }
    }
    public class quotacategry
    {
        public long ACQC_Id { get; set; }
        public string ACQC_CategoryName { get; set; }
    }
}
