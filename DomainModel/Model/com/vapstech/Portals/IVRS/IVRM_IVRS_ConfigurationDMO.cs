using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.Portals.IVRS
{
    [Table("IVRM_IVRS_Configuration")]
    public class IVRM_IVRS_ConfigurationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IIVRSC_Id { get; set; }
        public string  IIVRSC_VirtualNo { get; set; }
        public long IIVRSC_MI_Id { get; set; }
        public string IIVRSC_SchoolName { get; set; }
        public string IIVRSC_URL { get; set; }
        public string IIVRSC_VFORTTSFlg { get; set; }
        public string IVRS_MOBILE_URL { get; set; }
        public string IVRS_UPDATE_URL { get; set; }
        public long? IIVRSC_PerMonthCall { get; set; }
        public decimal? IIVRSC_CallCharge { get; set; }
        public bool? IIVRSC_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IIVRSC_SchoolFlg { get; set; }
        public string IVRSC_IVRM_Sub_Domain_Name  { get; set; }
        public string IIVRSC_AppLogo { get; set; }
        public string IIVRSC_SchoolCode  { get; set; }

    }
}
