using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission.Criteria8
{
   public class NC_818_EmpCommitteesDTO
    {
        public long NCNC8111EC_Id { get; set; }
        public long NCNC8111ECF_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long NCDC8111EC_Year { get; set; }
        public string NCDC8111EC_FacultyMemberName { get; set; }
        public string NCDC8111EC_CommitteesName { get; set; }
        public string NCDC8111EC_TenureOfService { get; set; }
        public DateTime? NCDC8111EC_CreatedDate { get; set; }
        public DateTime? NCDC8111EC_UpdatedDate { get; set; }
        public long NCDC8111EC_CreatedBy { get; set; }
        public long NCDC8111EC_UpdatedBy { get; set; }
        public bool NCDC8111EC_ActiveFlag { get; set; }
        public Array institutionlist { get; set; }
        public Array yearlist { get; set; }
        public Array alldata { get; set; }
        public long count { get; set; }
        public long count1 { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Naac_CommonFiles_DTO[] filelist { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array viewuploadflies { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public string NCNC8111ECC_Remarks { get; set; }
        public long NCNC8111ECC_RemarksBy { get; set; }
        public string NCNC8111ECC_StatusFlg { get; set; }
        public bool NCNC8111ECC_ActiveFlag { get; set; }
        public long NCNC8111ECC_CreatedBy { get; set; }
        public DateTime? NCNC8111ECC_CreatedDate { get; set; }
        public long NCNC8111ECC_UpdatedBy { get; set; }
        public long filefkid { get; set; }
        public DateTime? NCNC8111ECC_UpdatedDate { get; set; }
        public string UserName { get; set; }
        public string NCNC8111ECFC_Remarks { get; set; }
        public long NCNC8111ECFC_Id { get; set; }
        public long NCNC8111ECFC_RemarksBy { get; set; }
        public string NCNC8111ECFC_StatusFlg { get; set; }
        public bool NCNC8111ECFC_ActiveFlag { get; set; }
        public long NCNC8111ECFC_CreatedBy { get; set; }
        public DateTime? NCNC8111ECFC_CreatedDate { get; set; }
        public long NCNC8111ECFC_UpdatedBy { get; set; }
        public DateTime? NCNC8111ECFC_UpdatedDate { get; set; }
        public string Remarks { get; set; }
    }
}
