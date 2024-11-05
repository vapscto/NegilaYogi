using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VMS
{
    [Table("ISM_Master_Client")]
    public class ISM_Master_Client_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMMCLT_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string ISMMCLT_Code { get; set; }
        public string ISMMCLT_Desc { get; set; }
        public long ISMMCLT_ContactNo { get; set; }
        public string ISMMCLT_EmailId { get; set; }
        public string ISMMCLT_Address { get; set; }
        public string ISMMCLT_NOName { get; set; }
        public string ISMMCLT_NOEmailId { get; set; }
        public long ISMMCLT_NOContactNo { get; set; }

        public long? ISMMCLT_CordinatorId { get; set; }
        public long? ISMMCLT_RemainderDays { get; set; }
        public string ISMMCLT_GSTNO { get; set; }
        public long? ISMMCLT_TeamLeadId { get; set; }
        public bool ISMMCLT_ActiveFlag { get; set; }
        public long ISMMCLT_CreatedBy { get; set; }
        public long ISMMCLT_UpdatedBy { get; set; }
        public long? IVRM_MI_Id { get; set; }
        public string ISMMCLT_ClientCode { get; set; }
        public string ISMMCLT_IVRM_URL { get; set; }
        public List<ISM_Master_Client_IEMapping_DMO> ISM_Master_Client_IEMapping_DMO { get; set; }
        public List<ISM_Master_Client_UserMappingDMO> ISM_Master_Client_UserMappingDMO { get; set; }


    }
}
