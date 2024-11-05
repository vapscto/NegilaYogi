using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_HSU_511_NonGovScholarship_Comments")]
    public class NAAC_HSU_511_NonGovScholarship_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC512NGSCHC_Id { get; set; }
        public string NCAC512NGSCHC_Remarks { get; set; }
        public long NCAC512NGSCHC_RemarksBy { get; set; }
        public string NCAC512NGSCHC_StatusFlg { get; set; }
        public bool NCAC512NGSCHC_ActiveFlag { get; set; }
        public long NCAC512NGSCHC_CreatedBy { get; set; }
        public DateTime NCAC512NGSCHC_CreatedDate { get; set; }
        public long NCAC512NGSCHC_UpdatedBy { get; set; }
        public DateTime NCAC512NGSCHC_UpdatedDate { get; set; }
        public long NCAC512NGSCH_Id { get; set; }

    }
}
