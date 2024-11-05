using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_522_HrEducation_File_Comments")]
    public class NAAC_AC_522_HrEducation_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long  NCAC522HREDFC_Id { get; set; }
        public string  NCAC522HREDFC_Remarks { get; set; }
        public long  NCAC522HREDFC_RemarksBy { get; set; }
        public bool  NCAC522HREDFC_ActiveFlag { get; set; }
        public long  NCAC522HREDFC_CreatedBy { get; set; }
        public DateTime  NCAC522HREDFC_CreatedDate { get; set; }
        public long  NCAC522HREDFC_UpdatedBy { get; set; }
        public DateTime NCAC522HREDFC_UpdatedDate { get; set; }
        public string NCAC522HREDFC_StatusFlg { get; set; }
        public long  NCAC522HREDF_Id { get; set; }

    }
}
