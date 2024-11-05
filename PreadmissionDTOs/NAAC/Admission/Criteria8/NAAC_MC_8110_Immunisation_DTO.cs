using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission.Criteria8
{
   public class NAAC_MC_8110_Immunisation_DTO
    {

        public long NCMC8110IMM_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMC8110IMM_Year { get; set; }
        public long NCMC8110IMM_NoOfAdmStudents { get; set; }
        public long NCMC8110IMM_NoOfImmuStudents { get; set; }
        public bool NCMC8110IMM_ActiveFlg { get; set; }
        public long NCMC8110IMM_CreatedBy { get; set; }
        public long NCMC8110IMM_UpdatedBy { get; set; }
        public DateTime NCMC8110IMM_CreatedDate { get; set; }
        public DateTime NCMC8110IMM_UpdatedDate { get; set; }


        public long UserId { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }
        public long asmaY_Id { get; set; }
        public bool duplicate { get; set; }
        public NAAC_MC_8110_Immunisation_DTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public long NCMC8110IMMF_Id { get; set; }
        public string NCMC8110IMMC_Remarks { get; set; }
        public long NCMC8110IMMC_RemarksBy { get; set; }
        public string NCMC8110IMMC_StatusFlg { get; set; }
        public bool NCMC8110IMMC_ActiveFlag { get; set; }
        public long NCMC8110IMMC_CreatedBy { get; set; }
        public long NCMC8110IMMC_UpdatedBy { get; set; }
        public DateTime? NCMC8110IMMC_CreatedDate { get; set; }
        public DateTime? NCMC8110IMMC_UpdatedDate { get; set; }
        public string UserName { get; set; }
        public string NCMC8110IMMFC_Remarks { get; set; }
        public long NCMC8110IMMFC_Id { get; set; }
        public long NCMC8110IMMFC_RemarksBy { get; set; }
        public string NCMC8110IMMFC_StatusFlg { get; set; }
        public string Remarks { get; set; }
        public bool NCMC8110IMMFC_ActiveFlag { get; set; }
        public long NCMC8110IMMFC_CreatedBy { get; set; }
        public long filefkid { get; set; }
        public DateTime? NCMC8110IMMFC_CreatedDate { get; set; }
        public long NCMC8110IMMFC_UpdatedBy { get; set; }
        public DateTime? NCMC8110IMMFC_UpdatedDate { get; set; }
        public string NCMC8110IMM_StatusFlg { get; set; }
    }
}
