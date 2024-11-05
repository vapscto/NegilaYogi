using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAAC_AC_SParticipation_123_Students_DTO
    {

        public long NCACSP123_Id { get; set; }
        public long NCACSP123F_Id { get; set;}
        public long MI_Id { get; set; }
        public string NCACSP123_AddOnProgramName { get; set; }
        public long NCACSP123_NoOfStudParticipated { get; set; }
        public string NCACSP123F_FileName { get; set; }
        public string NCACSP123F_FilePath { get; set; }
        public string NCACSP123F_Filedesc { get; set; }
        public Array viewuploadflies { get; set; }


        public bool NCACSP123_ActiveFlg { get; set; }
        public long NCACSP123_CreatedBy { get; set; }
        public long NCACSP123_UpdatedBy { get; set; }
        public DateTime? NCACSP123_CreatedDate { get; set; }
        public DateTime? NCACSP123_UpdatedDate { get; set; }
        public DateTime NCACSP123_Date { get; set; }
        public long NCACSP123_Year { get; set; }

        public long NCACSP123S_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string AMCST_AdmNo { get; set; }
        public bool NCACSP123S_ActiveFlg { get; set; }
        public long NCACSP123S_CreatedBy { get; set; }
        public long NCACSP123S_UpdatedBy { get; set; }
        public DateTime? NCACSP123S_CreatedDate { get; set; }
        public DateTime? NCACSP123S_UpdatedDate { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string studentname { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMCST_RegistrationNo { get; set; }
      
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }

        public long UserId { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array alldata { get; set; }
        public Array branchlist { get; set; }
        public Array studentlist { get; set; }
        public Array mappedstudentlist { get; set; }
        public Array editlist { get; set; }
        public Array reportlist { get; set; }
        public string msg { get; set; }
        public Array editFileslist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array institutionlist { get; set; }

        public NAAC_AC_SParticipation_123_Students_DTO[] filelist { get; set; }
        public NAAC_AC_SParticipation_123_Students_DTO[] studentlstdata {get;set;}
        public long cfileid { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }

        public long NCACSP123C_Id { get; set; }
        public string NCACSP123C_Remarks { get; set; }
        public long? NCACSP123C_RemarksBy { get; set; }
        public string NCACSP123C_StatusFlg { get; set; }
        public bool NCACSP123C_ActiveFlag { get; set; }
        public long? NCACSP123C_CreatedBy { get; set; }
        public DateTime? NCACSP123C_CreatedDate { get; set; }
        public long? NCACSP123C_UpdatedBy { get; set; }
        public DateTime? NCACSP123C_UpdatedDate { get; set; }


        public long NCACSP123FC_Id { get; set; }
        public string NCACSP123FC_Remarks { get; set; }
        public long NCACSP123FC_RemarksBy { get; set; }
        public bool NCACSP123FC_ActiveFlag { get; set; }
        public long? NCACSP123FC_CreatedBy { get; set; }
        public DateTime? NCACSP123FC_CreatedDate { get; set; }
        public long? NCACSP123FC_UpdatedBy { get; set; }
        public DateTime? NCACSP123FC_UpdatedDate { get; set; }
        public string NCACSP123FC_StatusFlg { get; set; }
        public string UserName { get; set; }

        public string NCACSP123F_StatusFlg { get; set; }
        public bool? NCACSP123F_ApprovedFlg { get; set; }
        public long NCACTP113_Id { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public string cfilestatus { get; set; }
        public bool? cfileactive { get; set; }
                      
    }
}
