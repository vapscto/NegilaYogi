using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_511_GovScholarship_Comments")]
    public class NAAC_AC_511_GovScholarship_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC511GSCHC_Id { get; set; }
        public string NCAC511GSCHC_Remarks { get; set; }
        public long NCAC511GSCHC_RemarksBy { get; set; }
        public string NCAC511GSCHC_StatusFlg { get; set; }
        public bool NCAC511GSCHC_ActiveFlag { get; set; }
        public long NCAC511GSCHC_CreatedBy { get; set; }
        public DateTime NCAC511GSCHC_CreatedDate { get; set; }
        public long NCAC511GSCHC_UpdatedBy { get; set; }
        public DateTime NCAC511GSCHC_UpdatedDate { get; set; }
        public long NCAC511GSCH_Id { get; set; }

    }
}
