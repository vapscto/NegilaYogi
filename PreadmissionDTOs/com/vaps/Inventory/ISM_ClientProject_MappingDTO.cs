using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class ISM_ClientProject_MappingDTO
    {
        public long ISMMCLTPR_Id { get; set; }
        public long ISMMCLT_Id { get; set; }
        public long ISMMMD_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public long ISMMMD_ModuleHeadId { get; set; }
        public bool ISMMCLTPR_ActiveFlag { get; set; }
        public bool Module_ActiveFlag { get; set; }
        public long ISMMCLTPR_CreatedBy { get; set; }
        public long ISMMCLTPR_UpdatedBy { get; set; }
        public long MI_Id { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string ISMMCLT_ClientName { get; set; }
        public string ISMMPR_ProjectName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public bool Active_Flag { get; set; }
        public Array client_list { get; set; }
        public Array project_list { get; set; }
        public Array module_list { get; set; }
        public long IVRMM_Id { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string getproject { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public long UserId { get; set; }
        public Array editlist { get; set; }
        public ISM_ClientProject_MappingDTO[] moduleidss { get; set; }
        public Array get_clentlist { get; set; }
        public Array get_projectdetail { get; set; }
        public Array get_department { get; set; }
        public bool returnval { get; set; }
        public long projectid { get; set; }
        public string ISMMCLTPR_ProposalRefNo { get; set; }
        public DateTime? ISMMCLTPR_ProposalSentDate { get; set; }
        public DateTime? ISMMCLTPR_DealClosureDate { get; set; }
        public string ISMMCLTPR_MOURefNo { get; set; }
        public DateTime? ISMMCLTPR_MOUDate { get; set; }
        public long? ISMMCLTPR_MOURepresentedBy { get; set; }
        public DateTime? ISMMCLTPR_MOUStartDate { get; set; }
        public DateTime? ISMMCLTPR_MOUEndDate { get; set; }
        public string ISMMCLTPR_NodalOfficerName { get; set; }
        public long? ISMMCLTPR_NodalOfficerContactNo { get; set; }
        public string ISMMCLTPR_NodalOfficerEmailId { get; set; }
        public string ISMMCLTPR_ProjectDuration { get; set; }
        public long? ISMMCLTPR_TotalStudent { get; set; }
        public decimal? ISMMCLTPR_CostPerStudent { get; set; }
        public decimal? ISMMCLTPR_EnhancementPerYr { get; set; }
        public string ISMMCLTPR_Image { get; set; }
        public string ISMMCLTPR_WorkOrder { get; set; }
    }
}
