using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_711_GenderEquity_Comments")]
   public class NAAC_AC_711_GenderEquity_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC711GENEQC_Id { get; set; }
        public long NCAC711GENEQ_Id { get; set; }
        public long NCAC711GENEQC_RemarksBy { get; set; }
        public string NCAC711GENEQC_Remarks { get; set; }
        public string NCAC711GENEQC_StatusFlg { get; set; }
        public bool NCAC711GENEQC_ActiveFlag { get; set; }
        public long NCAC711GENEQC_CreatedBy { get; set; }
        public long NCAC711GENEQC_UpdatedBy { get; set; }
        public DateTime? NCAC711GENEQC_CreatedDate { get; set; }
        public DateTime? NCAC711GENEQC_UpdatedDate { get; set; }
    }
}
