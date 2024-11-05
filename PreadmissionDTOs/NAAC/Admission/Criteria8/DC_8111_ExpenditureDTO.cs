using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission.Criteria8
{
   public class DC_8111_ExpenditureDTO
    {
        public long NCDC8111E_Id { get; set; }
        public long NCDC8111EF_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long NCDC8111E_Year { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public decimal NCDC8111E_Expenditure { get; set; }
        public string NCDC8111E_DentalMaterialsName { get; set; }
        public DateTime? NCDC8111E_CreatedDate { get; set; }
        public DateTime? NCDC8111E_UpdatedDate { get; set; }
        public long NCDC8111E_CreatedBy { get; set; }
        public long NCDC8111E_UpdatedBy { get; set; }
        public bool NCDC8111E_ActiveFlag { get; set; }
        public Array institutionlist { get; set; }
        public Array yearlist { get; set; }
        public Array alldata { get; set; }
        public long count { get; set; }
        public long count1 { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public DC_8111_ExpenditureDTO[] filelist { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array viewuploadflies { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public string NCDC8111EC_Remarks { get; set; }
        public long NCDC8111EC_Id { get; set; }
        public long NCDC8111EC_RemarksBy { get; set; }
        public string NCDC8111EC_StatusFlg { get; set; }
        public bool NCDC8111EC_ActiveFlag { get; set; }
        public long NCDC8111EC_CreatedBy { get; set; }
        public DateTime? NCDC8111EC_CreatedDate { get; set; }
        public long NCDC8111EC_UpdatedBy { get; set; }
        public DateTime? NCDC8111EC_UpdatedDate { get; set; }
        public string UserName { get; set; }
        public string NCDC8111EFC_Remarks { get; set; }
        public long NCDC8111EFC_Id { get; set; }
        public long NCDC8111EFC_RemarksBy { get; set; }
        public string NCDC8111EFC_StatusFlg { get; set; }
        public bool NCDC8111EFC_ActiveFlag { get; set; }
        public long NCDC8111EFC_CreatedBy { get; set; }
        public DateTime? NCDC8111EFC_CreatedDate { get; set; }
        public long NCDC8111EFC_UpdatedBy { get; set; }
        public long filefkid { get; set; }
        public DateTime? NCDC8111EFC_UpdatedDate { get; set; }
        public string Remarks { get; set; }
    }
}
