using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Exit
{
    [Table("ISM_Resignation")]
   public class ISM_Resignation_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMRESG_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime ISMRESG_ResignationDate { get; set; }
        public long ISMRESGMRE_Id { get; set; }
        public long ISMRESG_NoticePeriod { get; set; }
        public DateTime ISMRESG_TentativeLeavingDate { get; set; }
        public string ISMRESG_Remarks { get; set; }
        public string ISMRESG_MgmtApprRejFlg { get; set; }
        public DateTime? ISMRESG_AccRejDate { get; set; }
        public string ISMRESG_ManagementRemarks { get; set; }
        public bool ISMRESG_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMRESG_CreatedBy { get; set; }
        public long ISMRESG_UpdatedBy { get; set; }
        public long ISMRESG_Print_Flg { get; set; }
        public long? ISMRESG_Status_Flg { get; set; }
    }
}
