using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_511_GovScholarship_File_Comments")]
    public class NAAC_AC_511_GovScholarship_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long  NCAC511GSCHFC_Id { get; set; }
        public string NCAC511GSCHFC_Remarks { get; set; }
        public long NCAC511GSCHFC_RemarksBy { get; set; }
        public bool NCAC511GSCHFC_ActiveFlag { get; set; }
        public long NCAC511GSCHFC_CreatedBy { get; set; }
        public DateTime NCAC511GSCHFC_CreatedDate { get; set; }
        public long NCAC511GSCHFC_UpdatedBy { get; set; }
        public DateTime NCAC511GSCHFC_UpdatedDate { get; set; }
        public string NCAC511GSCHFC_StatusFlg { get; set; }
        public long NCAC511GSCHF_Id { get; set; }
    }
}
