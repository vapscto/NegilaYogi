using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_719_DifferentlyAbled")]
    public class NAAC_AC_719_DifferentlyAbledDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public bool NCAC719DIFFAB_ActiveFlg { get; set; }
        public long NCAC719DIFFAB_CreatedBy { get; set; }
        public long NCAC719DIFFAB_UpdatedBy { get; set; }
        public DateTime? NCAC719DIFFAB_CreatedDate { get; set; }
        public DateTime? NCAC719DIFFAB_UpdatedDate { get; set; }
        public string NCAC719DIFFAB_CodeOfConductDisplayedWebsiteFlag { get; set; }
        public string NCAC719DIFFAB_CommitteeMonitorAdherenceCodeConductFlag { get; set; }
        public string NCAC719DIFFAB_ProfProgOrgStuStaffFlag { get; set; }
        public string NCAC719DIFFAB_AnnualAwsProgConductOrganizedFlag { get; set; }
    }
}
