using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7117_UniversalValues")]
    public class NAAC_AC_7117_UniversalValuesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7117UNIVAL_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7117UNIVAL_Year { get; set; }
        public string NCAC7117UNIVAL_ProgramTitle { get; set; }
        public long NCAC7117UNIVAL_NoOfPartcipants { get; set; }
        public DateTime NCAC7117UNIVAL_FromDate { get; set; }
        public DateTime NCAC7117UNIVAL_ToDate { get; set; }
        public bool NCAC7117UNIVAL_ActiveFlg { get; set; }
        public long NCAC7117UNIVAL_CreatedBy { get; set; }
        public long NCAC7117UNIVAL_UpdatedBy { get; set; }
        public DateTime? NCAC7117UNIVAL_CreatedDate { get; set; }
        public DateTime? NCAC7117UNIVAL_UpdatedDate { get; set; }
        public string NCAC7117UNIVAL_StatusFlg { get; set; }
    }
}
