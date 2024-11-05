using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_642_Funds_File_Comments")]
    public class NAAC_AC_642_Funds_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC642FUNDFC_Id { get; set; }
        public string NCAC642FUNDFC_Remarks { get; set; }
        public long? NCAC642FUNDFC_RemarksBy { get; set; }
        public bool? NCAC642FUNDFC_ActiveFlag { get; set; }
        public long? NCAC642FUNDFC_CreatedBy { get; set; }
        public DateTime? NCAC642FUNDFC_CreatedDate { get; set; }
        public long? NCAC642FUNDFC_UpdatedBy { get; set; }
        public DateTime? NCAC642FUNDFC_UpdatedDate { get; set; }
        public string NCAC642FUNDFC_StatusFlg { get; set; }
        public long NCAC642FUNDF_Id { get; set; }
    }
}
