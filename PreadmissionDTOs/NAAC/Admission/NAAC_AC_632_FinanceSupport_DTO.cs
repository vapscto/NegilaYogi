using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_632_FinanceSupport_DTO
    {

        public long NCAC632FINSUP_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC632FINSUP_Year { get; set; }
        public string NCAC632FINSUP_TeacherName { get; set; }
        public string NCAC632FINSUP_Name { get; set; }
        public string NCAC632FINSUP_PAN { get; set; }
        public string NCAC632FINSUP_NameOfMembership { get; set; }
        public bool NCAC632FINSUP_ConferenceProfBodyFlg { get; set; }
        public Nullable<decimal> NCAC632FINSUP_AmountPaid { get; set; }
        public string NCAC632FINSUP_FileDesc { get; set; }
        public string NCAC632FINSUP_FileName { get; set; }
        public string NCAC632FINSUP_FilePath { get; set; }
        public bool NCAC632FINSUP_ActiveFlg { get; set; }
        public long NCAC632FINSUP_CreatedBy { get; set; }
        public long NCAC632FINSUP_UpdatedBy { get; set; }
        public long UserId { get; set; }
        public DateTime NCAC632FINSUP_CreatedDate { get; set; }
        public DateTime NCAC632FINSUP_UpdatedDate { get; set; }
        public string ASMAY_Year { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public long asmaY_Id { get; set; }
       public bool duplicate { get; set; }
       public string msg { get; set; }
       public bool returnval { get; set; }
       public Array editlist { get; set; }
        public NAAC_AC_632_FinanceSupport_DTO[] filelist { get; set; }

        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array viewuploadflies { get; set; }
        public Array editFileslist { get; set; }
        public long NCAC632FINSUPF_Idp { get; set; }
        public long NCAC632FINSUPF_Id { get; set; }


        public long NCAC632FINSUPC_Id { get; set; }
        public string NCAC632FINSUPC_Remarks { get; set; }
        public long? NCAC632FINSUPC_RemarksBy { get; set; }
        public string NCAC632FINSUPC_StatusFlg { get; set; }
        public bool? NCAC632FINSUPC_ActiveFlag { get; set; }
        public long? NCAC632FINSUPC_CreatedBy { get; set; }
        public DateTime? NCAC632FINSUPC_CreatedDate { get; set; }
        public long? NCAC632FINSUPC_UpdatedBy { get; set; }
        public DateTime? NCAC632FINSUPC_UpdatedDate { get; set; }


        public long NCAC632FINSUPFC_Id { get; set; }
        public string NCAC632FINSUPFC_Remarks { get; set; }
        public long? NCAC632FINSUPFC_RemarksBy { get; set; }
        public bool? NCAC632FINSUPFC_ActiveFlag { get; set; }
        public long? NCAC632FINSUPFC_CreatedBy { get; set; }
        public DateTime? NCAC632FINSUPFC_CreatedDate { get; set; }
        public long? NCAC632FINSUPFC_UpdatedBy { get; set; }
        public DateTime? NCAC632FINSUPFC_UpdatedDate { get; set; }
        public string NCAC632FINSUPFC_StatusFlg { get; set; }

        public string NCAC632FINSUP_StatusFlg { get; set; }
        public bool? NCAC632FINSUP_ApprovedFlg { get; set; }
        public string NCAC632FINSUP_Remarks { get; set; }

        public bool? NCAC632FINSUPF_ActiveFlg { get; set; }
        public bool? NCAC632FINSUPF_ApprovedFlg { get; set; }
        public string NCAC632FINSUPF_Remarks { get; set; }
        public string NCAC632FINSUPF_StatusFlg { get; set; }

        public string Remarks { get; set; }
        public string UserName { get; set; }
        public long filefkid { get; set; }
        public long cfileid { get; set; }


        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
    }
}
