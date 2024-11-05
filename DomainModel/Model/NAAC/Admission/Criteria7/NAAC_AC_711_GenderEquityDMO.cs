using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_711_GenderEquity")]
    public class NAAC_AC_711_GenderEquityDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC711GENEQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC711GENEQ_Year { get; set; }
        public string NCAC711GENEQ_ProgramTitle { get; set; }
        public string NCAC711GENEQ_StatusFlg { get; set; }
        public DateTime? NCAC711GENEQ_FromDate { get; set; }
        public DateTime? NCAC711GENEQ_ToDate { get; set; }
        public long NCAC711GENEQ_NoOfParticipantsMale { get; set; }
        public long NCAC711GENEQ_NoOfParticipantsFeMale { get; set; }
        public bool NCAC711GENEQ_ActiveFlg { get; set; }
        public long NCAC711GENEQ_CreatedBy { get; set; }
        public long NCAC711GENEQ_UpdatedBy { get; set; }
        public DateTime? NCAC711GENEQ_CreatedDate { get; set; }
        public DateTime? NCAC711GENEQ_UpdatedDate { get; set; }
    }
}
