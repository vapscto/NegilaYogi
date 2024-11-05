using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace DomainModel.Model.NAAC
{
    [Table("NAAC_User_Privilege")]
    public class NAAC_User_PrivilegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NAACUPRI_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public int NAACUPRI_Order { get; set; }
        public bool NAACUPRI_FinalFlg { get; set; }
        public bool NAACUPRI_AddFlg { get; set; }
        public bool NAACUPRI_UpdateFlg { get; set; }
        public bool NAACUPRI_DeleteFlg { get; set; }
        public bool NAACUPRI_TrustUserFlag { get; set; }
        public bool NAACUPRI_IQACInchargeFlg { get; set; }
        public bool NAACUPRI_ConsultantFlg { get; set; }
        public bool NAACUPRI_ActiveFlag { get; set; }
        public long NAACUPRI_CreatedBy { get; set; }
        public DateTime? NAACUPRI_CreatedDate { get; set; }
        public long NAACUPRI_UpdatedBy { get; set; }
        public DateTime? NAACUPRI_UpdatedDate { get; set; }
        public bool? NAACUPRI_ApproverFlg { get; set; }
        public List<NAAC_User_Privilege_SLDMO> NAAC_User_Privilege_SLDMO { get; set; }
        public List<NAAC_User_Privilege_InstitutionDMO> NAAC_User_Privilege_InstitutionDMO { get; set; }

    }
}
