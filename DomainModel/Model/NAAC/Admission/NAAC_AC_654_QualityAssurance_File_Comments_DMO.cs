using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_654_QualityAssurance_File_Comments")]
    public class NAAC_AC_654_QualityAssurance_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC654QUASFC_Id { get; set; }
        public string NCAC654QUASFC_Remarks { get; set; }
        public long? NCAC654QUASFC_RemarksBy { get; set; }
        public bool? NCAC654QUASFC_ActiveFlag { get; set; }
        public long? NCAC654QUASFC_CreatedBy { get; set; }
        public DateTime? NCAC654QUASFC_CreatedDate { get; set; }
        public long? NCAC654QUASFC_UpdatedBy { get; set; }
        public DateTime? NCAC654QUASFC_UpdatedDate { get; set; }
        public string NCAC654QUASFC_StatusFlg { get; set; }
        public long NCAC654QUASF_Id { get; set; }
    }
}
