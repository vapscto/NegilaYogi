using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAAC_AC_IPR_322_DTO
    {
        public long NCACIPR322_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime NCACIPR322_EstablishmentDate { get; set; }
        public long NCACIPR322_Year { get; set; }
        public string NCACIPR322_WorkshopName { get; set; }
        public DateTime NCACIPR322_FromDate { get; set; }
        public DateTime NCACIPR322_ToDate { get; set; }
        public string NCACIPR322_WebisteLink { get; set; }
        public string NCACIPR322_FileName { get; set; }
        public string NCACIPR322_FilePath { get; set; }
        public bool NCACIPR322_ActiveFlg { get; set; }
        public long NCACIPR322_CreatedBy { get; set; }
        public long NCACIPR322_UpdatedBy { get; set; }
        public DateTime NCACIPR322_CreatedDate { get; set; }
        public DateTime NCACIPR322_UpdatedDate { get; set; }
        public Array allacademicyear { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public bool returnval { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public long UserId { get; set; }
        public List<NAAC_AC_IPR_322_DTO> alldata { get; set; }
        public Array alldata1 { get; set; }
        public Array editlist { get; set; }
        public Array viewuploadflies { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public long NCACIPR322F_Id { get; set; }
        public Array editFileslist { get; set; }
        public NAAC_AC_IPR_322_DTO[] filelist { get; set; }
        public Array institutionlist { get; set; }
        public long NCACIPR322_NoOfParticipants { get; set; }
        public string NCACIPR322_NameOfThePrincipalInvst { get; set; }
        public string NCACIPR322_DeptOfPrincialInvst { get; set; }
        public long NCACIPR322C_Id { get; set; }
        public string NCACIPR322C_Remarks { get; set; }
        public long? NCACIPR322C_RemarksBy { get; set; }
        public string NCACIPR322C_StatusFlg { get; set; }
        public bool? NCACIPR322C_ActiveFlag { get; set; }
        public long? NCACIPR322C_CreatedBy { get; set; }
        public DateTime? NCACIPR322C_CreatedDate { get; set; }
        public long? NCACIPR322C_UpdatedBy { get; set; }
        public DateTime? NCACIPR322C_UpdatedDate { get; set; }
        public Array commentlist { get; set; }
        public Array commentlist1 { get; set; }
        public string UserName { get; set; }
        public long NCACIPR322FC_Id { get; set; }
        public string NCACIPR322FC_Remarks { get; set; }
        public long? NCACIPR322FC_RemarksBy { get; set; }
        public bool? NCACIPR322FC_ActiveFlag { get; set; }
        public long? NCACIPR322FC_CreatedBy { get; set; }
        public DateTime? NCACIPR322FC_CreatedDate { get; set; }
        public long? NCACIPR322FC_UpdatedBy { get; set; }
        public DateTime? NCACIPR322FC_UpdatedDate { get; set; }
        public string NCACIPR322FC_StatusFlg { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public string NCACIPR322_StatusFlg { get; set; }
        public string NCACIPR322F_StatusFlg { get; set; }
        public bool NCACIPR322F_ActiveFlg { get; set; }
    }
}
