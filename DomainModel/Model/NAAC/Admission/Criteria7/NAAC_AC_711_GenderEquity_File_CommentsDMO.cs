using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_711_GenderEquity_File_Comments")]
   public class NAAC_AC_711_GenderEquity_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long NCAC711GENEQFC_Id { get; set; }
        public long NCAC711GENEQF_Id { get; set; }
        public long NCAC711GENEQFC_RemarksBy { get; set; }
        public string NCAC711GENEQFC_Remarks { get; set; }
        public string NCAC711GENEQFC_StatusFlg { get; set; }
        public bool NCAC711GENEQFC_ActiveFlag { get; set; }
        public long NCAC711GENEQFC_CreatedBy { get; set; }
        public long NCAC711GENEQFC_UpdatedBy { get; set; }
        public DateTime? NCAC711GENEQFC_CreatedDate { get; set; }
        public DateTime? NCAC711GENEQFC_UpdatedDate { get; set; }
    }
}
