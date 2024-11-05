using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_654_QualityAssurance_Comments")]
    public class NAAC_AC_654_QualityAssurance_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC654QUASC_Id { get; set; }
        public string NCAC654QUASC_Remarks { get; set; }
        public long? NCAC654QUASC_RemarksBy { get; set; }
        public string NCAC654QUASC_StatusFlg { get; set; }
        public bool? NCAC654QUASC_ActiveFlag { get; set; }
        public long? NCAC654QUASC_CreatedBy { get; set; }
        public DateTime? NCAC654QUASC_CreatedDate { get; set; }
        public long? NCAC654QUASC_UpdatedBy { get; set; }
        public DateTime? NCAC654QUASC_UpdatedDate { get; set; }
        public long NCAC654QUAS_Id { get; set; }
    }
}
