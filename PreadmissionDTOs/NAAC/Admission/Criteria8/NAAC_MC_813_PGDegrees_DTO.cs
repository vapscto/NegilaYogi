using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission.Criteria8
{
   public class NAAC_MC_813_PGDegrees_DTO
    {
        public long NCMC813PGDE_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array institutionlist { get; set; }
        public Array ff { get; set; }
        public long NCMC813PGDE_JoinYear { get; set; }
        public long NCMC813PGDE_CompletedYear { get; set; }
        public long NCMC813PGDE_NoOfTeachers { get; set; }
        public string NCMC813PGDE_DegreeFromInst { get; set; }
        public string MI_Name { get; set; }
        public bool NCMC813PGDE_ActiveFlg { get; set; }
        public long NCMC813PGDE_CreatedBy { get; set; }
        public long NCMC813PGDE_UpdatedBy { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public bool duplicate { get; set; }
        public long ASMAY_Id { get; set; }
        public NAAC_MC_813_PGDegrees_DTO[] filelist { get; set; }
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
        public long NCMC813PGDEF_Id { get; set; }
        public long filefkid { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public DateTime? NCMC813PGDEC_UpdatedDate { get; set; }
        public long NCMC813PGDEC_UpdatedBy { get; set; }
        public DateTime? NCMC813PGDEC_CreatedDate { get; set; }
        public long NCMC813PGDEC_CreatedBy { get; set; }
        public bool NCMC813PGDEC_ActiveFlag { get; set; }
        public string NCMC813PGDEC_StatusFlg { get; set; }
        public long NCMC813PGDEC_RemarksBy { get; set; }
        public string NCMC813PGDEC_Remarks { get; set; }
        public string NCMC813PGDEFC_Remarks { get; set; }
        public long NCMC813PGDEFC_Id { get; set; }
        public long NCMC813PGDEFC_RemarksBy { get; set; }
        public string NCMC813PGDEFC_StatusFlg { get; set; }
        public bool NCMC813PGDEFC_ActiveFlag { get; set; }
        public long NCMC813PGDEFC_CreatedBy { get; set; }
        public DateTime? NCMC813PGDEFC_CreatedDate { get; set; }
        public long NCMC813PGDEFC_UpdatedBy { get; set; }
        public DateTime? NCMC813PGDEFC_UpdatedDate { get; set; }
        public string NCMC813PGDE_StatusFlg { get; set; }
    }
}
