using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
   public class NAAC_AC_719_DifferentlyAbled_DTO : CommonParamDTO
    {
        public long NCAC719DIFFAB_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC719DIFFAB_Year { get; set; }
        public string NCAC719DIFFAB_LIFTFacilityFlg { get; set; }
        public string NCAC719DIFFAB_PhysicalFacilityFlg { get; set; }
        public string NCAC719DIFFAB_BrailleSaoftFlg { get; set; }
        public string NCAC719DIFFAB_RestRoomFlg { get; set; }
        public string NCAC719DIFFAB_ExamScribeFlg { get; set; }
        public string NCAC719DIFFAB_SPLSkilDevFlg { get; set; }
        public string NCAC719DIFFAB_RampFacilityFlg { get; set; }
        public string NCAC719DIFFAB_OtherFacility { get; set; }
        public string NCAC719DIFFABF_Filedesc { get; set; }
        public string NCAC719DIFFABF_FileName { get; set; }
        public string NCAC719DIFFABF_FilePath { get; set; }
        public bool NCAC719DIFFAB_ActiveFlg { get; set; }
        public long NCAC719DIFFAB_CreatedBy { get; set; }
        public long NCAC719DIFFAB_UpdatedBy { get; set; }
        public DateTime? NCAC719DIFFAB_CreatedDate { get; set; }
        public DateTime? NCAC719DIFFAB_UpdatedDate { get; set; }
        public long NCAC719DIFFABF_Id { get; set; }
        public string NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag { get; set; }
        public string NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag { get; set; }
        public string NCAC719DIFFAB_ProfProgOrgStuStaffFlag { get; set; }
        public string NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag { get; set; }


        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public long UserId { get; set; }
        public string retrunMsg { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlisttab1 { get; set; }
        public Array editfilelist { get; set; }
        public NAAC_AC_719_DifferentlyAbled_DTO[] NAACAC7DTO { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionlist { get; set; }
    }
}
