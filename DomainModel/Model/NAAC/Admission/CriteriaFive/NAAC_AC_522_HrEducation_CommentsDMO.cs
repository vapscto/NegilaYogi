using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_522_HrEducation_Comments")]
    public class NAAC_AC_522_HrEducation_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC522HREDC_Id { get; set; }
        public string NCAC522HREDC_Remarks { get; set; }
        public long NCAC522HREDC_RemarksBy { get; set; }
        public string NCAC522HREDC_StatusFlg { get; set; }
        public bool NCAC522HREDC_ActiveFlag { get; set; }
        public long NCAC522HREDC_CreatedBy { get; set; }
        public DateTime NCAC522HREDC_CreatedDate { get; set; }
        public long NCAC522HREDC_UpdatedBy { get; set; }
        public DateTime NCAC522HREDC_UpdatedDate { get; set; }
        public long NCAC522HRED_Id { get; set; }

    }
}
