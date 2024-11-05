using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_819_ClinicalLaboratory_Comments")]
   public class MC_819_ClinicalLaboratory_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCMCCL819C_Id { get; set; }
        public long NCMCCL819C_RemarksBy { get; set; }
        public string NCMCCL819C_Remarks { get; set; }
        public string NCMCCL819C_StatusFlg { get; set; }
        public bool NCMCCL819C_ActiveFlag { get; set; }
        public long NCMCCL819C_CreatedBy { get; set; }
        public long NCMCCL819C_UpdatedBy { get; set; }
        public long NCMCCL819_Id { get; set; }
        public DateTime? NCMCCL819C_CreatedDate { get; set; }
        public DateTime? NCMCCL819C_UpdatedDate { get; set; }
    }
}
