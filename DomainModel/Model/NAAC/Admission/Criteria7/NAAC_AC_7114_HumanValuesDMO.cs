using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7114_HumanValues")]
    public class NAAC_AC_7114_HumanValuesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7114HUVAL_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7114HUVAL_Year { get; set; }
        public string NCAC7114HUVAL_ProgramTitle { get; set; }
        public string NCAC7114HUVAL_StatusFlg { get; set; }
        public long NCAC7114HUVAL_NoOfPartcipants { get; set; }
        public DateTime NCAC7114HUVAL_FromDate { get; set; }
        public DateTime NCAC7114HUVAL_ToDate { get; set; }
        public bool NCAC7114HUVAL_ActiveFlg { get; set; }
        public long NCAC7114HUVAL_CreatedBy { get; set; }
        public long NCAC7114HUVAL_UpdatedBy { get; set; }
        public DateTime NCAC7114HUVAL_CreatedDate { get; set; }
        public DateTime NCAC7114HUVAL_UpdatedDate { get; set; }
    }
}
