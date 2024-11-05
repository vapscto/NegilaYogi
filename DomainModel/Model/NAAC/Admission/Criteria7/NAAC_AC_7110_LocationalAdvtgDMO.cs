using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7110_LocationalAdvtg")]
    public class NAAC_AC_7110_LocationalAdvtgDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7110LOCADVTG_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7110LOCADVTG_Year { get; set; }
        public long NCAC7110LOCADVTG_NoOfAddress { get; set; }
        public long NCAC7110LOCADVTG_NoOfEngage { get; set; }
        public DateTime? NCAC7110LOCADVTG_Date { get; set; }
        public long NCAC7110LOCADVTG_Duration { get; set; }
        public string NCAC7110LOCADVTG_InitiativeName { get; set; }
        public string NCAC7110LOCADVTG_IssuesAddressed { get; set; }
        public string NCAC7110LOCADVTG_StatusFlg { get; set; }
        public long NCAC7110LOCADVTG_NoOfParticipant { get; set; }
        public bool NCAC7110LOCADVTG_ActiveFlg { get; set; }
        public long NCAC7110LOCADVTG_CreatedBy { get; set; }
        public long NCAC7110LOCADVTG_UpdatedBy { get; set; }
        public DateTime NCAC7110LOCADVTG_CreatedDate { get; set; }
        public DateTime NCAC7110LOCADVTG_UpdatedDate { get; set; }
    }
}
