using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_HSU_511_NonGovScholarship_File_Comments")]
    public class NAAC_HSU_511_NonGovScholarship_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC512NGSCHFC_Id { get; set; }
        public string  NCAC512NGSCHFC_Remarks { get; set; }
        public long  NCAC512NGSCHFC_RemarksBy { get; set; }
        public bool  NCAC512NGSCHFC_ActiveFlag { get; set; }
        public long  NCAC512NGSCHFC_CreatedBy { get; set; }
        public DateTime  NCAC512NGSCHFC_CreatedDate { get; set; }
        public long  NCAC512NGSCHFC_UpdatedBy { get; set; }
        public DateTime NCAC512NGSCHFC_UpdatedDate { get; set; }
        public string NCAC512NGSCHFC_StatusFlg { get; set; }
        public long  NCAC512NGSCHF_Id { get; set; }

    }
}
