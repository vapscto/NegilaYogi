using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Inventory
{
    [Table("ISM_TaskCreation")]
    public class ISM_TaskCreationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMTCR_Id { get; set; }
        public string ISMTCR_TaskNo { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public string ISMTCR_BugOREnhancementFlg { get; set; }
        public DateTime? ISMTCR_CreationDate { get; set; }
        public string ISMTCR_Title { get; set; }
        public long HRMPR_Id { get; set; }
        public long? ISMMTCAT_Id { get; set; }
        public long? ISMMTGRP_Id { get; set; }
        public long? ISMMCLT_Id { get; set; }
        public int? ISMTCR_TGOrder { get; set; }
        public string ISMTCR_Desc { get; set; }
        public string ISMTCR_Status { get; set; }
        public bool ISMTCR_ReOpenFlg { get; set; }
        public DateTime? ISMTCR_ReOpenDate { get; set; }
        public bool ISMTCR_ActiveFlg { get; set; }
        public long ISMTCR_CreatedBy { get; set; }
        public long ISMTCR_UpdatedBy { get; set; }
        public long HRME_Id { get; set; }
        public long? ISMIMPPL_Id { get; set; }
        public bool? ISMTCR_MainGroupTaskFlg { get; set; }
        public string ISMTCR_VerifierRemarks { get; set; }
        public long? ISMTCR_VerifiedBy { get; set; }
        public string ISMTCR_Days { get; set; }
        public string ISMTCR_Hours { get; set; }
        //public List<ISM_TaskCreation_AttachmentDMO> ISM_TaskCreation_AttachmentDMO { get; set; }
        //public List<ISM_TaskEnhancement_DeptwiseDMO> ISM_TaskEnhancement_DeptwiseDMO { get; set; }
        //public List<ISM_TaskCreation_AssignedToDMO> ISM_TaskCreation_AssignedToDMO { get; set; }
        //public List<ISM_TaskCreation_ClientDMO> ISM_TaskCreation_ClientDMO { get; set; }
        //public List<ISM_Client_TaskCreation_AssignedToDMO> ISM_Client_TaskCreation_AssignedToDMO { get; set; }
        //public List<ISM_Task_Advance_PlannerDMO> ISM_Task_Advance_PlannerDMO { get; set; }

        //public List<ISM_Tenant_TaskCreationDMO> ISM_Tenant_TaskCreationDMO { get; set; }




    }
}
